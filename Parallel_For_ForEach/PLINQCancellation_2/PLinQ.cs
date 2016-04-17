using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PLINQCancellation_2
{
    class PLinQ
    {
        static void Main( string[] args )
        {
            int[] source = Enumerable.Range( 1, 1000000 ).ToArray();

            // 시간 선지정 가능
            //var ct = new CancellationTokenSource( TimeSpan.FromSeconds( 30 ) ).Token;
            CancellationTokenSource cts = new CancellationTokenSource();

            // Start a new asynchronous task that will cancel the 
            // operation from another thread. Typically you would call
            // Cancel() in response to a button click or some other
            // user interface event.
            Task.Factory.StartNew( () =>
            {
                UserClicksTheCancelButton( cts );
            } );

            double[] results = null;
            try
            {
                results = ( from num in source.AsParallel().WithCancellation( cts.Token )
                            where num % 3 == 0
                            select Function( num, cts.Token ) ).ToArray();

            }
            catch( OperationCanceledException e )
            {
                Console.WriteLine( '\n' + e.Message );
            }
            catch( AggregateException ae )
            {
                if( ae.InnerExceptions != null )
                    foreach( Exception e in ae.InnerExceptions )
                    {
                        Console.WriteLine( e.Message );
                    }
            }

            finally {
                cts.Dispose();
            }

            if( results != null )
            {
                if( results.Count() < 200 )
                {
                    foreach( var v in results )
                        Console.WriteLine( v );
                }
                else
                {
                    Console.WriteLine("출력 결과가 200개 보다 많기 때문에 출력 취소");
                }
            }
            Console.WriteLine("press any key exit...");
            Console.ReadKey();
        }
        // A toy method to simulate work.
        static double Function( int n, CancellationToken ct )
        {
            // If work is expected to take longer than 1 ms
            // then try to check cancellation status more
            // often within that work.
            for( int i = 0; i < 5; i++ )
            {
                // Work hard for approx 1 millisecond.
                Thread.SpinWait( 5000 );

                // Check for cancellation request.
                ct.ThrowIfCancellationRequested();
            }
            // Anything will do for our purposes.
            return Math.Sqrt( n );
        }

        static void UserClicksTheCancelButton( CancellationTokenSource cs )
        {
            // Wait between 150 and 500 ms, then cancel.
            // Adjust these values if necessary to make
            // cancellation fire while query is still executing.
            Random rand = new Random();
            Thread.Sleep( rand.Next( 150, 350 ) );

            Console.WriteLine( "Press 'c' to cancel" );
            if( Console.ReadKey().KeyChar == 'c' )
            {
                cs.Cancel();
            }
        }
    }
}
    
