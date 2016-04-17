using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallel_For_07
{
    class For07
    {
        static void NormalFor( int[] nums )
        {
            for( int i = 0; i < nums.Length; i++ )
            {
                nums[ i ] = nums[ i ] * nums[ i ];
            }
        }

        static void ParallelFor( int[] nums )
        {
            Parallel.For( 0, nums.Length, ( i ) =>
            {
                nums[ i ] = nums[ i ] * nums[ i ];
            } );
        }

        // 파티셔너를 사용한 Parallel.ForEach가 성능이 더 좋음.
        static void RangeParallelForEach( int[] nums )
        {
            var part = Partitioner.Create( 0, nums.Length );

            Parallel.ForEach( part, ( num, state ) =>
            {
                for( int i = num.Item1; i < num.Item2; i++ )
                {
                    nums[ i ] = nums[ i ] * nums[ i ];
                }
            } );
        }

        static void Main( string[] args )
        {
            int max = 50000000;
            int[] nums1 = Enumerable.Range( 1, 10 ).ToArray<int>();
            int[] nums2 = Enumerable.Range( 1, 10 ).ToArray<int>();
            int[] nums3 = Enumerable.Range( 1, 10 ).ToArray<int>();

            //JIT컴파일을 위해.
            NormalFor( nums1 );
            ParallelFor( nums2 );
            RangeParallelForEach( nums3 );

            nums1 = Enumerable.Range( 1, max ).ToArray<int>();
            nums2 = Enumerable.Range( 1, max ).ToArray<int>();
            nums3 = Enumerable.Range( 1, max ).ToArray<int>();

            DateTime normalForStart = DateTime.Now;
            NormalFor( nums1 );
            DateTime normalForEnd = DateTime.Now;
            TimeSpan normalForResult = normalForEnd - normalForStart;

            DateTime parallelForStart = DateTime.Now;
            ParallelFor( nums2 );
            DateTime parallelForEnd = DateTime.Now;
            TimeSpan parallelForResult = parallelForEnd - parallelForStart;

            DateTime rangeForStart = DateTime.Now;
            RangeParallelForEach( nums3 );
            DateTime rangeForEnd = DateTime.Now;
            TimeSpan rangeForResult = rangeForEnd - rangeForStart;

            Console.WriteLine( "Single-threaded for : {0}", normalForResult );
            Console.WriteLine( "Load-balancing Parallel.For : {0}", parallelForResult );
            Console.WriteLine( "Range-partition Parallel.ForEach : {0}", rangeForResult );
        }
    }
}
