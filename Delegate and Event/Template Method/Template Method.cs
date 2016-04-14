using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template_Method
{
    public class Person
    {
        private int _age;
        public int Age
        {
            get { return _age; }
            set { _age = value; }
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public Person( int age, string name )
        {
            _age = age;
            _name = name;
        }

        //public event AgeChangedEventHandler AgeChanged;
        // 이벤트 핸들러를 제네릭으로 변경

        // 3. 이벤트를 선언
        public event EventHandler<AgeChangedEventArgs> AgeChanged;
        //public virtual void IncreaseAge()
        //{
        //    int oldAge = _age;
        //    _age++;

        //    if (AgeChanged != null)
        //        AgeChanged( this, new AgeChangedEventArgs( oldAge, _age ) );
        //}

        // 4. 이벤트를 호출하는 가상 메서드를 작성한다.
        protected virtual void OnAgeChanged(AgeChangedEventArgs e)
        {
            if (AgeChanged != null)
                AgeChanged( this, e );
        }
        public void IncreaseAge()
        {
            int oldAge = _age;
            _age++;
            OnAgeChanged( new AgeChangedEventArgs( oldAge, _age ) );
        }
    }
    // 1. 이벤트에서 사용할 대리자(델리게이트) 추가 또는 제너릭 EventHandler을 사용.
    public delegate void AgeChangedEventHandler( object sender, AgeChangedEventArgs e );
    // 2. EventArg를 상속하는 이벤트 인자 클래스를 만든다.
    public class AgeChangedEventArgs : EventArgs
    {
        private int _oldAge;
        private int _newAge;

        public int OldAge
        {
            get { return _oldAge; }
            set { _oldAge = value; }
        }
        public int NewAge
        {
            get { return _newAge; }
            set { _newAge = value; }
        }
        public AgeChangedEventArgs( int oldAge, int newAge )
        {
            _oldAge = oldAge;
            _newAge = newAge;
        }
    }
    public class Entertainer : Person
    {
        public Entertainer(int age, string name) : base(age, name)
        {
        }
        //public void IncreaseAge()
        //{
        //    //int oldAge = _age;
        //    //_age++;
        //    //if (AgeChanged != null)
        //    //    AgeChanged( this, new AgeChangedEventArgs( oldAge, oldAge ) );           
        //}
        
        // 5. 가상 메서드를 재정의 
        protected override void OnAgeChanged( AgeChangedEventArgs e )
        {
            base.OnAgeChanged( new AgeChangedEventArgs( e.OldAge, e.OldAge ) );
        }   

    }
    class Program
    {
        static void Main( string[] args )
        {
            Person p = new Person( 38, "원빈" );
            p.AgeChanged += Person_AgeChanged;
            p.IncreaseAge();

            Person entertainer = new Entertainer( 34, "김태희" );
            entertainer.AgeChanged += Person_AgeChanged;
            entertainer.IncreaseAge();
            Console.ReadKey();
        }
        private static void Person_AgeChanged( object sender, AgeChangedEventArgs e )
        {
            Person p = sender as Person;
            if( p !=null )
            {
                Console.WriteLine(string.Format ("{0} : {1} -> {2}", p.Name, e.OldAge, e.NewAge ));
            }
        }
    }
}