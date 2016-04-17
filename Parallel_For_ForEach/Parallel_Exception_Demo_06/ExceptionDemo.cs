using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Parallel_Exception_Demo_06
{
    class ExceptionDemo2
    {
        static void Main( string[] args )
        {
            // Create some random data to process in parallel.
            // There is a good probability this data will cause some exceptions to be thrown.
            byte[] data = new byte[ 5000 ];
            Random r = new Random();
            r.NextBytes( data );
            Console.WriteLine("{0:x}", 0x3);
            try
            {
                ProcessDataInParallel( data );
            }

            catch( AggregateException ae )
            {
                // This is where you can choose which exceptions to handle.
                // 익셉션으로 받은 내부 에러 메세지
                foreach( var ex in ae.InnerExceptions )
                {
                    // ArgumentException이면 출력
                    if( ex is ArgumentException )
                        Console.WriteLine( ex.Message );
                    else
                        throw ex;
                }
            }

            Console.WriteLine( "Press any key to exit." );
            Console.ReadKey();
        }

        private static void ProcessDataInParallel( byte[] data )
        {
            // Use ConcurrentQueue to enable safe enqueueing from multiple threads.
            // 스레드 안전, 형식 안전한 큐 - ConcurrentQueue
            var exceptions = new ConcurrentQueue<Exception>();

            // Execute the complete loop and capture all exceptions.
            Parallel.ForEach( data, d =>
            {
                try
                {
                    // Cause a few exceptions, but not too many.
                    // 3보다 작은 숫자를 내부 익셉션 시키고
                    if( d < 0x3 )
                        throw new ArgumentException( 
                            String.Format( "value is {0:x}. Elements must be greater than 0x3.", d ) );
                    else
                        Console.Write( d + "\t" );
                }
                // Store the exception and continue with the loop.                    
                catch( Exception e ) { exceptions.Enqueue( e ); }
            } );

            // Throw the exceptions here after the loop completes.
            // 내부 익셉션이 있다면 AggregateException에 익셉션으로 넘김
            if( exceptions.Count > 0 )
                throw new AggregateException( exceptions );
        }
    }

}
