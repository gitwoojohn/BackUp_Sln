using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadLocalForEach
{
    // ForEach02와 같음.
    class ThreadLocalforEach
    {
        static void Main( string[] args )
        {
            int[] nums = Enumerable.Range( 0, 1000001 ).ToArray();
            long total = 0;

            // First type parameter is the type of the source elements
            // Second type parameter is the type of the local data (subtotal)
            // <int, long> long 생략하면 값이 짤림.
            Parallel.ForEach<int, long>( nums, // source collection
                                         () => 0, // method to initialize the local variable
                                         ( nIndex, loop, subtotal ) => // method invoked by the loop on each iteration
                                         {
                                            subtotal += nums[ nIndex ]; //modify local variable
                                            return subtotal; // value to be passed to next iteration
                                         },
                                         // Method to be executed when all loops have completed.
                                         // finalResult is the final value of subtotal. supplied by the ForEach method.
                                         ( subtotal ) => Interlocked.Add( ref total, subtotal )
                                         );

            Console.WriteLine( "The total from Parallel.ForEach is {0}", total );
            Console.WriteLine( "Press any key to exit" );
            Console.ReadKey();
        }
    }
}
