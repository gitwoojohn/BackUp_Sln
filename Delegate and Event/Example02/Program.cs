using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example02
{
    class Example01
    {
        static void Main( string[] args )
        {
            int birthYear = 1976;

            Person p = new Person( 10, "철수" );
            // 무명 메서드 사용
            p.AgeChanged = delegate( int oldAge, int newAge )
            {
                Console.WriteLine( string.Format( "{0} -> {1} : {2}년생 ", oldAge, newAge, birthYear ) );
            };

            p.IncreaseAge();   
            Console.ReadKey();
        }
        private static void Person_AgeChanged( int oldAge, int newAge )
        {
            Console.WriteLine( string.Format( "{0} -> {1} ", oldAge, newAge ) );
        }
    }
    public class Person
    {
        public delegate void AgeChangedDelegate( int oldAge, int NewAge );
        public AgeChangedDelegate AgeChanged;

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
        public void IncreaseAge()
        {
            int oldAge = _age;
            _age++;

            if (AgeChanged != null)
                AgeChanged( oldAge, _age );
            //Console.WriteLine( string.Format( "{0} -> {1} ", oldAge, _age ));
        }
    }
}
