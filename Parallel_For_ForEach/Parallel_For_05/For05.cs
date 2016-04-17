using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Parallel_For_05
{
    class ThreadLocalForWithOptions
    {
        // The number of parallel iterations to perform.
        const int N = 10000;

        static void Main()
        {
            // The result of all thread-local computations.
            int result = 0;
            int[] intArray = Enumerable.Range( 1, N ).ToArray();

            // This example limits the degree of parallelism to four.
            // You might limit the degree of parallelism when your algorithm
            // does not scale beyond a certain number of cores or when you 
            // enforce a particular quality of service in your application.
            // body 대리자가 반복 범위에 있는 각 값에 대해 한 번만 호출됩니다
            // ( fromInclusive, toExclusive). 반복 횟수(Int32), 조기에 루프를 차단하는 데 사용할 수 있는 
            // ParallelLoopState 인스턴스 및 같은 스레드에서 실행하는 반복 간에 공유될 수 있는 
            // 몇 가지 로컬 상태 등의 매개 변수가 제공됩니다. 

            // localInit 대리자는 루프의 예외에 참가하는 각 스레드에 대해 한 번 호출되며 
            // 이러한 각 스레드에 대한 초기 로컬 상태를 반환합니다.이러한 초기 상태는 
            // 각 스레드에서 첫 번째 body 호출로 전달됩니다. 

            // 그런 다음 모든 후속 본문 호출은 다음 본문 호출에 전달되는 가능한 수정된 상태 값을 반환합니다.
            // 마지막으로, 각 스레드에서 마지막 본문 호출은 localFinally 대리자에 전달되는 상태 값을 반환합니다.
            // localFinally 대리자는 각 스레드의 로컬 상태에서 최종 작업을 수행하는 스레드 당 한 번씩 호출됩니다. 
            // 동시에 여러 스레드에서 이 대리자를 호출할 수 있습니다. 따라서 공유 변수에 대한 액세스를 동기화해야 합니다.
            // fromInclusive가 toExclusive보다 크거나 같은 경우 메서드는 반복을 수행하지 않고 즉시 반환합니다.

            // 이 예제는 병렬처리 수준을 4 스레드로 제한 합니다.

            Parallel.For
                (
                    // 시작 인덱스, 끝 인덱스, 시스템의 동시 작업 개수 설정
                    0, N, new ParallelOptions { MaxDegreeOfParallelism = 4 },
                    // 각 스레드에 대한 로컬 데이터의 초기 상태를 반환하는 함수 대리자입니다.
                    () => 0,
                    // 반복당 한 번씩 호출되는 대리자입니다.
                    ( i, LoopState, localValue ) =>
                    {
                        localValue += intArray[ i ];     //Compute( i );
                        return localValue; //+ Compute( i );
                    },
                    // 각 스레드의 로컬 상태에 대해 최종 동작을 수행하는 대리자입니다.
                    localValue => Interlocked.Add( ref result, localValue )
                );

            // 합산된 값 출력.
            Console.WriteLine( "Actual result: {0}. Expected 1000.", result );
            Console.ReadKey();
        }

        // Simulates a lengthy operation.
        private static int Compute( int n )
        {
            for( int i = 0; i < 100000; i++ ) ;
            return 1;
        }
    }
}
/*
public static ParallelLoopResult For<TLocal>(
	int fromInclusive,
	int toExclusive,
	ParallelOptions parallelOptions,
	Func<TLocal> localInit,
	Func<int, ParallelLoopState, TLocal, TLocal> body,
	Action<TLocal> localFinally
)

** Type 매개 변수 **
TLocal : 스레드 로컬 데이터의 형식입니다.

** 매개 변수 **
매개 변수              형 식                       설  명
-----------------------------------------------------------------------------------------------------------------
fromInclusive      System.Int32             시작 인덱스(포함)입니다.
-----------------------------------------------------------------------------------------------------------------
toExclusive        System.Int32             끝 인덱스(제외)입니다.
-----------------------------------------------------------------------------------------------------------------
parallelOptions                             이 작업의 동작을 구성하는 ParallelOptions 인스턴스입니다.
                   System.Threading.Tasks.ParallelOptions 
-----------------------------------------------------------------------------------------------------------------
localInit                                   각 스레드에 대한 로컬 데이터의 초기 상태를 반환하는 함수 대리자입니다.
                   System.Func<TLocal> 
-----------------------------------------------------------------------------------------------------------------
body                                        반복당 한 번씩 호출되는 대리자입니다.
                   System.Func<Int32, ParallelLoopState, TLocal, TLocal> 
-----------------------------------------------------------------------------------------------------------------
localFinally       System.Action<TLocal>    각 스레드의 로컬 상태에 대해 최종 동작을 수행하는 대리자입니다.
-----------------------------------------------------------------------------------------------------------------
반환 값                                     완료된 루프의 부분에 대한 정보가 포함된 ParallelLoopResult 구조체입니다.  
                   System.Threading.Tasks.ParallelLoopResult 
-----------------------------------------------------------------------------------------------------------------
*/
