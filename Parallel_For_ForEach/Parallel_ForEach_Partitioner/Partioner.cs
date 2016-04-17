using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Parallel_ForEach_Partitioner
{
    class Parallel_Partitioner
    {
        static void Main( string[] args )
        {
            // 정적 범위 파티셔너
            // Source must be array or IList.
            var source = Enumerable.Range( 0, 100000 ).ToArray();

            // Partition the entire source array.
            // 소스 크기에 맞게 자동으로 배열 크기 분리
            var rangePartitioner = Partitioner.Create( 0, source.Length );

            double[] results = new double[ source.Length ];

            // Loop over the partitions in parallel.
            // 분리된 배열 Partitioner 병렬처리
            Parallel.ForEach( rangePartitioner, ( range, loopState ) =>
            {
                // Loop over each range element without a delegate invocation.
                for( int i = range.Item1; i < range.Item2; i++ )
                {
                    results[ i ] = source[ i ] * Math.PI;
                }
            } );

            Console.WriteLine( "Operation complete. Print results? y/n" );
            char input = Console.ReadKey().KeyChar;
            if( input == 'y' || input == 'Y' )
            {
                foreach( double d in results )
                {
                    Console.Write( "{0} ", d );
                }
            }

        }
    }
}
