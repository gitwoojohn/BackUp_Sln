using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartGeneric
{
    // 모든 형식 다 가능
    class List<T>
    {
        public T[] arr;
        public List() { arr = new T[ 1 ]; }
    }

    // class 만
    class ListClass<T> where T : class
    {
        public T[] arr;
        public ListClass() { arr = new T[ 1 ]; }
    }

    // int, float, string 값 형식
    class PremtiveType<T> where T : struct
    {
    }

    class Parent
    {
        public string name { get; set; }
        public Parent() { name = "부모 클래스\n"; }
    }
    class Children : Parent
    {
        public Children() { name = "자식 클래스\n"; }
    }

    class ListT<T> where T : Parent
    {
        public T[] array;
        public ListT() { array = new T[ 2 ]; }
    }

    class Program
    {
        static void Main( string[] args )
        {
            int intValue = 1;
            string strValue = "string Value";

            print( intValue );
            print( strValue );

            List<int> list1 = new List<int>();
            list1.arr[ 0 ] = 10;

            List<float> list2 = new List<float>();
            list2.arr[ 0 ] = 10.5f;

            // 제약 조건
            List<string> list3 = new List<string>();
            list3.arr[ 0 ] = "List String";

            ListT<Parent> list = new ListT<Parent>();
            list.array[ 0 ] = new Parent();
            list.array[ 1 ] = new Children();

            Console.WriteLine( list.array[ 0 ].name );
            Console.WriteLine( list.array[ 1 ].name );

            string s1 = "target";
            StringBuilder sb = new StringBuilder( "target" );
            string s2 = sb.ToString();
            OpTest( s1, s2 );

            // 제네릭 인터페이스 예제
            Generic_Interface();

            // 연산자 오버로딩.
            Complex_Operator();

            // 삼중 연산자 Bool
            Triple_LogicalBool();
            // wait.
            Console.ReadKey();
        }
        static void print<T>( T value )
        {
            Console.WriteLine( value );
        }
        public static void OpTest<T>( T s, T t ) where T : class
        {
            Console.WriteLine( ( s == t ) + "\n" );
        }
        private static void Triple_LogicalBool()
        {
            DBBool a, b;
            a = DBBool.dbTrue;
            b = DBBool.dbNull;

            Console.WriteLine( "\r\n===삼중 연산자 Bool ===" );
            Console.WriteLine( "!{0} = {1}", a, !a );
            Console.WriteLine( "!{0} = {1}", b, !b );
            Console.WriteLine( "{0} & {1} = {2}", a, b, a & b );
            Console.WriteLine( "{0} | {1} = {2}", a, b, a | b );

            // DBBool 변수의 부울 값을 결정하는 true 연산자를 호출 합니다.
            if( b )
                Console.WriteLine( "b is definitely true" );
            else
                Console.WriteLine( "b is not definitely true" );
        }
        private static void Complex_Operator()
        {
            Complex num1 = new Complex( 5, 7 );
            Complex num2 = new Complex( 2, 3 );

            // Add two Complex objects (num1 and num2) through the
            // overloaded plus operator:
            Complex sum = num1 + num2;

            // Print the numbers and the sum using the overriden ToString method:
            Console.WriteLine( "First complex number:  {0}", num1 );
            Console.WriteLine( "Second complex number: {0}", num2 );
            Console.WriteLine( "The sum of the two numbers: {0}", sum );

            // 추가 코드 ( 빼기 연산자 오버로딩 추가 )
            sum = num1 - num2;
            Console.WriteLine( "The sum of the two numbers minus: {0}", sum );
        }
        private static void Generic_Interface()
        {
            //Declare and instantiate a new generic SortedList class.
            //Person is the type argument.
            SortedList<Person> list = new SortedList<Person>();

            //Create name and age values to initialize Person objects.
            string[] names = new string[]
            {
                "Franscoise",
                "Bill",
                "Li",
                "Sandra",
                "Gunnar",
                "Alok",
                "Hiroyuki",
                "Maria",
                "Alessandro",
                "Raul"
            };

            int[] ages = new int[] { 45, 19, 28, 23, 18, 9, 108, 72, 30, 35 };

            //Populate the list.
            for( int x = 0; x < 10; x++ )
            {
                list.AddHead( new Person( names[ x ], ages[ x ] ) );
            }

            //Print out unsorted list.
            foreach( Person p in list )
            {
                Console.WriteLine( p.ToString() );
            }
            Console.WriteLine( "Done with unsorted list\n\n" );

            //Sort the list.
            list.BubbleSort();

            //Print out sorted list.
            foreach( Person p in list )
            {
                Console.WriteLine( p.ToString() );
            }
            Console.WriteLine( "Done with sorted list" );
        }
    }
}
