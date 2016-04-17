using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Parallel_For_03
{
    class parallelOption
    {
        // Demonstrated features:
        // 		Parallel.For()
        //		ParallelOptions
        // Expected results:
        // 		An iteration for each argument value (0, 1, 2, 3, 4, 5, 6, 7, 8, 9) is executed.
        //		The order of execution of the iterations is undefined.
        //		Verify that no more than two threads have been used for the iterations.
        // Documentation:
        //		http://msdn.microsoft.com/en-us/library/system.threading.tasks.parallel.for(VS.100).aspx

        static void Main( string[] args )
        {
            Parallel.For
                (
                    0,       // 시작 인덱스
                    9,       // 끝 인덱스
                    i =>    // 시작과 끝 인덱스 범위에 해당하는 매개 변수
                    {
                        Console.WriteLine( "Thread={0}, i={1}", Thread.CurrentThread.ManagedThreadId, i );
                    }
                );


            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = 2; // -1 is for unlimited. 1 is for sequential.

            try
            {
                Parallel.For
                    (
                        0,          // 시작 인덱스
                        9,          // 끝 인덱스
                        options,    // cpu 코어 사용 개수
                        ( i ) =>    // 시작과 끝 인덱스 범위에 해당하는 매개 변수
                        {
                            Console.WriteLine( "Thread={0}, i={1}", Thread.CurrentThread.ManagedThreadId, i );
                        }
                    );

            }
            // No exception is expected in this example, but if one is still thrown from a task,
            // it will be wrapped in AggregateException and propagated to the main thread.
            catch( AggregateException e )
            {
                Console.WriteLine( "Parallel.For has thrown the following (unexpected) exception:\n{0}", e );
            }
            Console.ReadKey();
        }
    }
}
namespace ParallelFor_int32_int32_Action_int32
{

}