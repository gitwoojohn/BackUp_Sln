using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 두 목록 간의 차집합 구하기
namespace CompareLists
{
    class Program
    {
        static void Main( string[] args )
        {
            string[] names1 = System.IO.File.ReadAllLines( @"../../../names1.txt" );
            string[] names2 = System.IO.File.ReadAllLines( @"../../../names2.txt" );

            // Create the query. Note that method syntax must be used here.
            // names1에는 있지만 names2는 없는 이름 콘솔에 출력
            IEnumerable<string> differenceQuery =
                names1.Except( names2 );

            // Execute the query.
            Console.WriteLine("The following lines are in names1.txt but not names2.txt");
            foreach(string s in differenceQuery)
                Console.WriteLine(s);

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            
        }
    }
}
