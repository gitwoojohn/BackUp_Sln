using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq_Basic_Ex_01
{
    class Program
    {
        public static Array GetLinqVar( ProductInfo[] pi )
        {
            return ( from p in pi
                     where p.Weight > 13
                     select new
                     {
                         p.Name,
                         p.Description
                     } ).ToArray();
        }
        static void Main( string[] args )
        {
            Example_01();

            Example_02();

            VariousQueryBuildTest.QueryStringWithOperators();

            // wait.
            Console.ReadLine();
        }

        /// <summary>
        /// LINQ 시작 예제 01
        /// </summary>
        private static void Example_01()
        {
            // 예제 1
            //Create a list of strings.
            var salmons1 = new List<string>();
            salmons1.Add( "chinook" );
            salmons1.Add( "coho" );
            salmons1.Add( "pink" );
            salmons1.Add( "sockeys" );

            foreach( var salmon1 in salmons1 )
            {
                Console.WriteLine( salmon1 + " " );
            }
            Console.WriteLine( "============================================\n" );

            // 예제 2
            //Create a list of strings by using a collection initializer.
            var salmons2 = new List<string> { "chinook", "coho", "pink", "sockeye" };

            //Iterate through the list.
            foreach( var salmon2 in salmons2 )
            {
                Console.WriteLine( salmon2 + " " );
            }
            Console.WriteLine( "============================================\n" );

            // 예제 3
            //Create a list of strings by using a collection initializer.
            var salmons3 = new List<string> { "chinook", "coho", "pink", "sockeye" };

            for( var index = 0; index < salmons3.Count; index++ )
            {
                Console.WriteLine( salmons3[ index ] + " " );
            }
            Console.WriteLine( "============================================\n" );

            // 예제4
            salmons3.Remove( "coho" );

            for( var index = 0; index < salmons3.Count; index++ )
            {
                Console.WriteLine( salmons3[ index ] + " " );
            }

            // 예제 5
            var numbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            // Remove odd numbers.
            for( var index = numbers.Count - 1; index >= 0; index-- )
            {
                if( numbers[ index ] % 2 == 1 )
                {
                    // Remove the element by specifying
                    // the zero-based index in the list.
                    numbers.RemoveAt( index );
                }
            }

            // Iterate through the list.
            // A lambda expression is placed in the ForEach method
            // of the List(T) object.
            numbers.ForEach(
                number => Console.Write( number + " " ) );
        }
        /// <summary>
        /// LINQ 시작 예제 02
        /// </summary>
        private static void Example_02()
        {
            ProductInfo[] pi = new ProductInfo[]
                {
                    new ProductInfo("제품1", "첫제품이다", 11),
                    new ProductInfo("제품2", "<제품이다", 12),
                    new ProductInfo("제품A", "OP제품이다", 13),
                    new ProductInfo("제품B", "LKJ제품이다", 14),
                    new ProductInfo("제품C", "jasdf제품이다", 15),
                };

            foreach( var v in GetLinqVar( pi ) )
            {
                Console.WriteLine( v );
            }

            // LINQ Enumerable 의 확장메소드를 사용해 보기

            // - 확장 메소드 Reverse<T>()
            // 추가 사용예로
            //    - 인덱스를 포함하여 select하기
            //    - 명시적으로 Linq 의 IEnumerable<T>.Select() 함수 쓰기
            var result = ( from p in pi
                           where p.Weight <= 12
                           select p ).ToArray();
            foreach( var prod in result.Select( ( p, index ) =>
                                                new
                                                {
                                                    Index = index,
                                                    Name = p.Name,
                                                } ).Reverse() )
            {
                Console.WriteLine( prod );
            }

            List<string> myCars = new List<String> { "Yugo", "Aztec", "BMW" };
            List<string> yourCars = new List<String> { "BMW", "Saab", "Aztec" };

            // - 확장 메소드 Except(), Union(), Intersect(), Concat(), Distinct()
            var allMyCar = ( from car in myCars select car );
            var allYourCars = ( from car in yourCars select car );

            var concatUniqueCar = allMyCar.Concat( allYourCars ).Distinct();
            PrintAll( "concatUnique", concatUniqueCar );

            var carUnion = allMyCar.Union( allYourCars );
            PrintAll( "union", carUnion );

            var carIntersect = allMyCar.Intersect( allYourCars );
            PrintAll( "intersect", carIntersect );

            var carExceptYours = allMyCar.Except( allYourCars );
            PrintAll( "Mine except Yours", carExceptYours );
        }
        public static void PrintAll( string desc, IEnumerable enumurable )
        {
            Console.WriteLine( "\nPrinting [{0}] {1}...",
                              desc, enumurable.GetType() );
            foreach( var item in enumurable )
            {
                Console.WriteLine( item );
            }
        }
    }

}
// 테스테에 사용할 자료구조
public struct ProductInfo
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Weight { get; set; }
    public ProductInfo( string name, string desc, int weight )
        : this()
    {
        Name = name; Description = desc; Weight = weight;
    }
    public override string ToString()
    {
        return string.Format( "Name:{0}, Desc:{1}, Weight:{2}" );
    }
}
class VariousQueryBuildTest
{
    /// <summary>
    /// 1 ~ 5가지의 쿼리식 ( 모두 같은 결과를 반환 ), 실행해도 실제 결과 값은 반환 하지 않음.
    /// </summary>
    public static void QueryStringWithOperators()
    {
        Console.WriteLine( "*** Using Query Operators ***" );
        string[] currentVideoGames = {"Morrowind", "Uncharted 2",
                                      "Fallout 3", "Daxter", "System Shock 2"};

        // 방법 1
        //
        // LINQ 구문으로 쿼리식 생성
        var subset1 = from game in currentVideoGames
                      where game.Contains( " " )
                      orderby game
                      select game;

        // 방법 2
        // 
        // Enumerable 형을 통해 기존 System.Array 배열에 추가된
        // 확장메소드를 사용해 쿼리식을 생성.
        var subset2 = currentVideoGames.Where( game => game.Contains( " " ) )
                      .OrderBy( game => game ).Select( game => game );

        // 방법 3
        // 
        // 위의 내용을 쪼개어서 생성
        var gamesWithSpaces = currentVideoGames.Where( game => game.Contains( " " ) );
        var orderedGames = gamesWithSpaces.OrderBy( game => game );
        var subset3 = orderedGames.Select( game => game );

        // 방법 4
        // 
        // 익명 메소드로 Func<> 대리자(delegate)를 만들어
        // 만들어진 delegate를 Enumerable 확장메소드에 전달해서 쿼리식 생성
        Func<string, bool> searchFilter = delegate ( string game ) { return game.Contains( " " ); };
        Func<string, string> itemToProcess = delegate ( string s ) { return s; };
        var subset4 = currentVideoGames.Where( searchFilter )
                      .OrderBy( itemToProcess ).Select( itemToProcess );

        // 방법 5
        //
        // 일반적인(?) 정적 메소드를 Func<> 대리자(delegates)로 하여
        // Enumerable 확장메소드에 전달해서 쿼리식 생성
        Func<string, bool> searchFilter1 = new Func<string, bool>( Filter );
        Func<string, string> itemToProcess1 = new Func<string, string>( ProcessItem );
        var subset5 = currentVideoGames
                      .Where( searchFilter ).OrderBy( itemToProcess ).Select( itemToProcess );
    }
    // 방법5에 사용된 대리자 함수(Delegate targets)
    public static bool Filter( string game ) { return game.Contains( " " ); }
    public static string ProcessItem( string game ) { return game; }
}

