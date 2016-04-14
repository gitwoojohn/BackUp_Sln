using System;
using System.Collections.Generic;
using System.Text;

namespace GenericMethodExam
{
    public class Pair
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    class Constrain
    {
        static bool SerializePairs( List<Pair> pairs )
        {
            try
            {
                Serializer.Serialize<List<Pair>>( pairs, "Pairs.xml" );

                return true;
            }
            catch( Exception ex )
            {
                Console.WriteLine( ex.Message );
                return false;
            }
        }

        static List<Pair> DeserializePairs()
        {
            try
            {
                return Serializer.Deserialize<List<Pair>>( "Pairs.xml" );
            }
            catch( Exception ex )
            {
                Console.WriteLine( ex.Message );
                return new List<Pair>();
            }
        }

        static void Main( string[] args )
        {
            // 시리얼, 디시리얼라이즈
            List<Pair> pairs = new List<Pair>
            {
                new Pair{
                    Id=1,
                    Name="홍길동"
                },
                new Pair{
                    Id=2,
                    Name="루저"
                },
                new Pair{
                    Id=3,
                    Name="위너"
                }
            };

            SerializePairs( pairs );
            List<Pair> pairs2 = DeserializePairs();

            foreach( Pair pair in pairs2 )
            {
                Console.WriteLine( $"{pair.Id} : {pair.Name}" );
            }

            // 싱글 링크드 리스트
            // 제약 조건 사용 안 할때 ( 사용시 : 형식 매개변수 사용x 에러 )
            /*
                중첩 Node 클래스에서도 T를 사용할 수 있습니다. GenericList<int>와 같이 GenericList<T>를 
                구체적 형식을 사용하여 인스턴스화하면 모든 T가 int로 대체됩니다.
            */
            //GenericList<int> list = new GenericList<int>();
            //for( int i = 0; i < 10; i++ )
            //{
            //    list.AddHead( i );
            //}

            //foreach( int item in list )
            //{
            //    Console.WriteLine( item + " " );
            //}
            //Console.WriteLine( "\nDone." );

            // 제약 조건 사용시

            GenericList<Employee> employeeList = new GenericList<Employee>();
            string[] names = new string[]
            {
              "홍길동", "서길동", "중길동", "소길동", "독길동",
              "안길동", "정길동", "이길동", "박길동", "지길동"
            };
            int[] ages = new int[] { 45, 19, 28, 23, 18, 9, 108, 72, 30, 35 };
            for( int i = 0; i < names.Length; i++ )
            {
                // 클래스 프로퍼티를 이용해서 값 넣기
                employeeList.AddHead( new Employee( names[ i ], ages[ i ] ) );
            }
            if( employeeList.FindFirstOccurrence( "박길동" ) == null )
            {
                Console.WriteLine("찾는 값이 없습니다.");
            }
            else
            {
                Console.WriteLine("찾는 값이 있습니다.");
            }
            //foreach( var item in employeeList )
            //{
            //    Console.WriteLine( item.ID + ", " + item.Name +" " );
            //}
            //Console.WriteLine( "\nDone." );

            // 
            //string s1 = "target";
            //StringBuilder sb = new StringBuilder( "target" );
            //string s2 = sb.ToString();
            //OpTest<string>( s1, s2 );


            //

            SortedList<Person> personList = new SortedList<Person>();
            for( int i = 0; i < ages.Length; i++ )
            {
                personList.AddHead( new Person( names[ i ], ages[ i ] ) );
            }

            //Print out unsorted list.
            foreach( Person p in personList )
            {
                System.Console.WriteLine( p.ToString() );
            }
            System.Console.WriteLine( "소트 되지 않은 데이터\n" );

            //Sort the list. 나이 오름차순
            personList.BubbleSort();

            //Print out sorted list.
            foreach( Person p in personList )
            {
                System.Console.WriteLine( p.ToString() );
            }
            System.Console.WriteLine( "오름차순으로 소트된 데이터\n" );

            // 제네릭 디폴트 키워드 Default
            Keyword_Default();
            Console.ReadLine();
        }
        private static void Keyword_Default()
        {
            // Test with a non-empty list of integers.
            Generic_Default_Keyword<int> gll = new Generic_Default_Keyword<int>();
            gll.AddNode( 5 );
            gll.AddNode( 4 );
            gll.AddNode( 3 );
            int intVal = gll.GetLast();
            // The following line displays 5.
            System.Console.WriteLine( intVal );

            // Test with an empty list of integers.
            Generic_Default_Keyword<int> gll2 = new Generic_Default_Keyword<int>();
            intVal = gll2.GetLast();
            // The following line displays 0.
            System.Console.WriteLine( intVal );

            // Test with a non-empty list of strings.
            Generic_Default_Keyword<string> gll3 = new Generic_Default_Keyword<string>();
            gll3.AddNode( "five" );
            gll3.AddNode( "four" );
            string sVal = gll3.GetLast();
            // The following line displays five.
            System.Console.WriteLine( sVal );

            // Test with an empty list of strings.
            Generic_Default_Keyword<string> gll4 = new Generic_Default_Keyword<string>();
            sVal = gll4.GetLast();
            // The following line displays a blank line.
            System.Console.WriteLine( sVal );
        }
        public static void OpTest<T>( T s, T t ) where T : class, IComparable<T>
        {
            Console.WriteLine( $"s1과 s2는 같은 값인가? : {s == t}" );
        }
    }
}
/*

클라이언트 코드가 제약조건에서 허용하지 않는 형식을 사용하여 클래스를 인스턴스화 하려고 하면 컴파일러 오류 발생.

제약 조건의 사용 이유 : 제네릭 목록의 항목을 검사하여 그 유효성을 확인하거나 이 항목을 다른 항목과 비교하려는 경우,
컴파일러는 클라이언트 코드에서 지정할 수 있는 모든 형식 인수에 호출해야 할 메서드나 연산자가 지원되는지 확인.

기본 클래스의 제약 조건에서는 이 형식의 개체와 이 형식에서 파생된 개체만이 형식 인수로 사용될 수 있음을 컴파일러에게 알림.

-----------------------------------------------------------------------------------------------------------
where T: struct	 타입 매개변수는 반드시 Nullable을 제외한 값형(value type)이어야만 한다.
-----------------------------------------------------------------------------------------------------------
where T : class	 타입 매개변수는 반드시 참조형(reference type)이어야만 한다.
-----------------------------------------------------------------------------------------------------------
where T : new()	 타입 매개변수는 반드시 public이고 매개변수가 없는 생성자를 갖고 있어야 한다. 
                 그리고 다른 제약 조건이랑 같이 사용될때는 new()가 맨뒤에 와야한다.
-----------------------------------------------------------------------------------------------------------
where T : <base class name>	 타입 매개변수는 반드시 명시된 클래스를 상속 해야한다.
-----------------------------------------------------------------------------------------------------------
where T : <interface name>	 타입 매개변수는 반드시 명시된 인터페이스이거나, 명시된 인터페이스를 구현해야 한다.
-----------------------------------------------------------------------------------------------------------
where T : U	 T에 해당되는 매개변수는 반드시 U에 해당되는 매개변수 타입이거나, U를 상속한 타입이어야 한다. 
-----------------------------------------------------------------------------------------------------------
*/
