using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Parallel_ForEach_02
{
    class ForEachWithThreadLocal
    {
        // Demonstrated features:
        // 		Parallel.ForEach()
        //		Thread-local state
        // Expected results:
        //      This example sums up the elements of an int[] in parallel.
        //      Each thread maintains a local sum. When a thread is initialized, that local sum is set to 0.
        //      On every iteration the current element is added to the local sum.
        //      When a thread is done, it safely adds its local sum to the global sum.
        //      After the loop is complete, the global sum is printed out.
        // Documentation:
        //      https://msdn.microsoft.com/ko-kr/library/dd997393(v=vs.100).aspx


        // ForEach 루프가 실행되면 소스 컬렉션이 여러개의 파티션으로 분활. 
        // 각 파티션은 "스레드 로컬" 이라고 하는  고유의 복사본을 가짐.

        /*
        원자성( Atomicity)
        아래 조건 중에서 하나를 만족하면 원자성을 만족.
        
            1. 작업을 처리하기 위한 명령어 집합은 다른 작업이 끼어들기 전에 반드시 완료되어야 한다.

            2. 작업을 처리 중인 시스템의 상태는, 작업을 처리하기 전의 시스템의 상태로 돌아갈 수 있어야 한다.
                즉, 아무 작업도 처리되지 않은 시스템의 상태로 돌아갈 수 있어야 한다.
        */
        static void Main()
        {
            int[] input = Enumerable.Range( 1, 10 ).ToArray(); //{ 4, 1, 6, 2, 9, 5, 10, 3 };
            long sum = 0;

            try
            {
                // For와 ForEach가 다른점. ForEach는 배열의 끝을 알려 줄 필요가 없다는 점.
                Parallel.ForEach<int,long>(
                        input,                        // source collection
                        () => 0,                      // thread local initializer(로컬데이터 초기 상태 반환하는 함수 대리자)
                        ( n, loopState, localSum ) => // body(반복당 한 번씩 호출되는 대리자)
                        {
                            localSum += n;
                            Console.WriteLine( "Thread = {0}, n = {1}, localSum = {2}",
                                Thread.CurrentThread.ManagedThreadId, n, localSum );
                            return localSum;
                        },
                        // Atomicity 원자성( 다른 작업이 완료되기 전에 끼어들지 못하게 )
                        ( localSum ) => Interlocked.Add( ref sum, localSum ) // thread local aggregator
                        );

                Console.WriteLine( "\nSum = {0}", sum );
            }
            // No exception is expected in this example, but if one is still thrown from a task,
            // it will be wrapped in AggregateException and propagated to the main thread.
            catch( AggregateException e )
            {
                Console.WriteLine( "Parallel.ForEach has thrown an exception. THIS WAS NOT EXPECTED.\n{0}", e );
            }
            Console.ReadKey();
        }
    }
}
