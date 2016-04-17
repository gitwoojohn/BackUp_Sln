using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Parallel_For_06
{
    class For06
    {
        static void RangeParallelForEach( int[] nums )
        {
            List<int> threadIds = new List<int>();
            var part = Partitioner.Create( 0, nums.Length );

            Parallel.ForEach( part, ( num, state ) =>
            {
                if( !threadIds.Contains( Task.CurrentId.Value ) )
                {
                    threadIds.Add( Task.CurrentId.Value );
                }

                for( int i = num.Item1; i < num.Item2; i++ )
                {
                    nums[ i ] = nums[ i ] * nums[ i ];
                }
            } );
            Console.WriteLine("\n\nPartitioner를 사용해서 구간 분활한 Parallel.ForEach");
            Console.WriteLine( "Thread ID list of RangeForEach" );
            foreach( var id in threadIds )
            {
                Console.WriteLine( "{0}", id.ToString() );
            }
        }

        static void ParallelFor( int[] nums )
        {
            List<int> threadIds = new List<int>();

            Parallel.For( 0, nums.Length, ( i ) =>
            {
                if( !threadIds.Contains( Task.CurrentId.Value ) )
                {
                    threadIds.Add( Task.CurrentId.Value );
                }

                nums[ i ] = nums[ i ] * nums[ i ];
            } );
            Console.WriteLine("로드 밸런싱을 사용한 Parallel.For : 많은 스레드 생성으로 Context Switching으로 인한 오버헤드가 늘어남");
            Console.WriteLine( "Thread ID list of ParallelFor" );
            foreach( var id in threadIds )
            {
                Console.WriteLine( "{0}", id.ToString() );
            }
        }

        static void Main( string[] args )
        {
            int max = 50000000;
            int[] nums1 = Enumerable.Range( 1, max ).ToArray<int>();
            int[] nums2 = Enumerable.Range( 1, max ).ToArray<int>();

            ParallelFor( nums1 );
            RangeParallelForEach( nums2 );
        }
    }
}
