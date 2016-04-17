using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cancellation_Token_02
{
    class Cancellation_Token
    {
        static void Main( string[] args )
        {
            int[] source = Enumerable.Range( 1, 1000000 ).ToArray();
            CancellationTokenSource cts = new CancellationTokenSource();

            // Start a new asynchronous task that will cancel the 
            // operation from another thread. Typically you would call
            // Cancel() in response to a button click or some other
            // user interface event.
            Task.Factory.StartNew( () =>
            {
                UserClicksTheCancelButton( cts );
            } );

            int[] results = null;
            try
            {
                results = ( from num in source.AsParallel().WithCancellation( cts.Token ) 
                            where num % 10 == 0
                            orderby num descending
                            select num ).ToArray();

            }
            catch( OperationCanceledException e )
            {
                Console.WriteLine( e.Message );
            }
            catch( AggregateException ae )
            {
                if( ae.InnerExceptions != null )
                {
                    foreach( Exception e in ae.InnerExceptions )
                        Console.WriteLine( e.Message );
                }
            }
            finally
            {
                //cts.Dispose();
            }

            if( results != null )
            {
                foreach( var v in results )
                    Console.WriteLine( v );
            }
            Console.WriteLine();
            Console.ReadKey();
        }
        static void UserClicksTheCancelButton( CancellationTokenSource cs )
        {
            // Wait between 150 and 500 ms, then cancel.
            // Adjust these values if necessary to make
            // cancellation fire while query is still executing.
            Random rand = new Random();
            Thread.Sleep( rand.Next( 150, 350 ) );
            cs.Cancel();
        }
    }
}
