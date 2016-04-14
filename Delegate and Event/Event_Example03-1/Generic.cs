using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Example03_1
{
    class Generic
    {
        static void Main( string[] args )
        {
            Person p = new Person( 10, "철수" );

            p.AgeChanged += Person_AgeChanged;
            p.IncreaseAge();
            Console.ReadKey();
        }
        private static void Person_AgeChanged( object sender, AgeChangedEventArgs e )
        {
            Console.WriteLine( string.Format( "{0} -> {1} ", e.OldAge, e.NewAge ) );
        }
    }

    public delegate void AgeChangedEventHandler( object sender, AgeChangedEventArgs e );
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
        public event EventHandler<AgeChangedEventArgs> AgeChanged;
        public void IncreaseAge()
        {
            int oldAge = _age;
            _age++;

            if (AgeChanged != null)
                AgeChanged( this, new AgeChangedEventArgs( oldAge, _age ) );
        }
    }
}
