using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashSet.ConsoleApp
{
    class Program
    {
        //int _max = 10000000;
        static void Main( string[] args )
        {
            // 배열에 cat 2개 중복
            string[] array1 = { "Zibra", "dog", "cat", "leopard", "tiger", "cat" };

            // Display the array.
            Console.WriteLine( string.Join( ",", array1 ) );

            // 고유한 문자열을 사용하기 위해 HashSet을 사용
            var hash = new HashSet<string>( array1 );

            // 문자열을 다시 배열로 변환
            string[] array2 = hash.ToArray();

            Console.WriteLine( string.Join( ",", array2 ) + "\r\n" );

            // 시간 측정
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            SortedSet_01();

            stopWatch.Stop();
            Console.WriteLine( stopWatch.ElapsedMilliseconds / 1000d );


            SymmertricExcepWith_01();

            Overlaps_01();

            Console.ReadLine();
        }
        /// <summary>
        /// SortedSet 예제 ( 중복 제거 및 오름차순 정렬 )
        /// </summary>
        static void SortedSet_01()
        {
            string[] array1 = { "Zibra", "dog", "cat", "leopard", "tiger", "cat" };
            //string[] array2 = TextRead();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Random r = new Random();

            List<int> oldData = new List<int>();
            for( int i = 0; i < 100; i++ )
            {
                oldData.Add( r.Next( 1, 10 ) );
            }

            //List<int> tt = TextRead().ToList();            

            List<int> newTemp = new List<int>();

            // 제네릭 중복제거 방법 1 - 새로운 제네릭 리스트의 Contains를 이용 중복 제거
            foreach( int item in oldData )
            {
                if( !newTemp.Contains( item ) )
                {
                    newTemp.Add( item );
                }
            }

            // 제네릭 중복 제거 방법 2 - Distinct() 이용
            IEnumerable<int> distinctNumber = TextRead().Distinct();
            List<int> distList = distinctNumber.ToList();
            distList.Sort();

            // 제네릭 중복 제거 방법 3 - HashSet 이용
            Product dupErase = new Product();
            dupErase.distinctProduct_02();

            stopWatch.Stop();
            //Console.WriteLine( stopWatch.ElapsedMilliseconds / 1000d );

            //SortedSet<int> sortedSet = new SortedSet<int>( tt );

            //foreach( var item in sortedSet )
            //{
            //    Console.WriteLine( item );
            //}
        }
        /// <summary>
        /// 두 배열사이에서 중복 되지 않는 고유 요소만 반환
        /// </summary>
        static void SymmertricExcepWith_01()
        {
            char[] array1 = { 'a', 'b', 'c' };
            char[] array2 = { 'b', 'c', 'd' };

            // HashSet로 고유한 요소를 전부 구하고
            var hash = new HashSet<char>( array1 );

            // hash로 구한 중복없는 결과를 배열2와 비교해서 고유 요소만 반환
            hash.SymmetricExceptWith( array2 );

            Console.WriteLine( "HashSet & SymmetricExcepWith" );
            Console.WriteLine( hash.ToArray() );
        }
        /// <summary>
        /// 두 배열 비교해서 같은 요소가 있으면 true 아니면 false
        /// </summary>
        static void Overlaps_01()
        {
            int[] array1 = { 1, 2, 3 };
            int[] array2 = { 3, 4, 5 };
            int[] array3 = { 9, 10, 11 };

            HashSet<int> set = new HashSet<int>( array1 );
            bool a = set.Overlaps( array2 );
            bool b = set.Overlaps( array3 );

            // Display results. true / false
            Console.WriteLine( a );
            Console.WriteLine( b );
        }
        static int[] TextRead()
        {
            FileStream fs = null;
            int _max = 10000000;
            int[] intHash = new int[ _max ];
            try
            {
                fs = new FileStream( @"C:\Temp\10Number.txt", FileMode.Open );
                using( StreamReader reader = new StreamReader( fs ) )
                {
                    string line;
                    int i = 0;
                    while( ( line = reader.ReadLine() ) != null )
                    {

                        intHash[ i++ ] = int.Parse( line );
                    }
                }
            }
            finally
            {
                if( fs != null )
                    fs.Dispose();
            }
            return intHash;
        }

    }
}
