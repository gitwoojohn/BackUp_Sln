using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StopOrBreak
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    // Parallel.For 루프를 중단 하거나 중지 할 때 Stop와 Break 사용
    // Stop는 모든 쓰레드 작업에서 가능한 즉시 중단. 
    // Break는 반복이전의 작업은 완료하고, 중지 가능한 남은 작업에서 루프 중지
    // https://msdn.microsoft.com/ko-kr/library/dd460721(v=vs.100).aspx
    class Stop_or_Break
    {
        static void Main()
        {
            StopLoop();
            BreakAtThreshold();

            Console.WriteLine( "Press any key to exit." );
            Console.ReadKey();
        }

        private static void StopLoop()
        {
            Console.WriteLine( "Stop loop..." );
            double[] source = MakeDemoSource( 1000, 1 );
            ConcurrentStack<double> results = new ConcurrentStack<double>();

            // i is the iteration variable. loopState is a 
            // compiler-generated ParallelLoopState
            Parallel.For( 0, source.Length, ( i, loopState ) =>
            {
                // Take the first 100 values that are retrieved
                // from anywhere in the source.
                if( i < 100 )
                {
                    // Accessing shared object on each iteration
                    // is not efficient. See remarks.
                    double d = Compute( source[ i ] );
                    results.Push( d );
                }
                else
                {                    
                    loopState.Stop();
                    return;
                }

            } // Close lambda expression.
            ); // Close Parallel.For

            Console.WriteLine( "Results contains {0} elements", results.Count() );           
        }


        static void BreakAtThreshold()
        {
            double[] source = MakeDemoSource( 10000, 1.0002 );
            ConcurrentStack<double> results = new ConcurrentStack<double>();

            // Store all values below a specified threshold.
            Parallel.For( 0, source.Length, ( i, loopState ) =>
            {
                double d = Compute( source[ i ] );
                results.Push( d );
                if( d > .2 )
                {
                    // Might be called more than once!
                    loopState.Break();
                    Console.WriteLine( "Break called at iteration {0}. d = {1} ", i, d );
                    Thread.Sleep( 1000 );
                }
            } );

            Console.WriteLine( "results contains {0} elements", results.Count() );
        }

        static double Compute( double d )
        {
            //Make the processor work just a little bit.
            return Math.Sqrt( d );
        }


        // Create a contrived array of monotonically increasing
        // values for demonstration purposes. 
        static double[] MakeDemoSource( int size, double valToFind )
        {
            double[] result = new double[ size ];
            double initialval = .01;
            for( int i = 0; i < size; i++ )
            {
                initialval *= valToFind;
                result[ i ] = initialval;
            }

            return result;
        }
    }

}


