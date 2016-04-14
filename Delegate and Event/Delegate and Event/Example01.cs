using System;

namespace ConsoleApplication1
{
    class Example01
    {

        static void Main( string[] args )
        {
            Person p = new Person( 10, "철수" );
            //p.AgeChanged = new Person.AgeChangedDelegate( Person_AgeChanged );
            // 위 코드와 같은 기능을 하는 코드(축약코드)
            p.AgeChanged = Person_AgeChanged;
            Console.WriteLine( "나이:{0}, 이름:{1} ", p.Age, p.Name );
            p.IncreaseAge();
            //Console.WriteLine( "나이:{0}, 이름:{1} ", p.Age, p.Name );

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

            AgeChanged?.Invoke( oldAge, _age );
            //Console.WriteLine( string.Format( "{0} -> {1} ", oldAge, _age ));
        }
    }



}
