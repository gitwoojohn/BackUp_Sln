using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadLocalFor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    // https://msdn.microsoft.com/ko-kr/library/dd460703(v=vs.100).aspx
    // 스레드 로컬 변수를 사용하여 For 루프에 의해 만들어진 각 개별 작업의 상태를 저장하고 검색하는 예제.

    // 스레드 로컬 데이터를 사용하면 공유 상태에 대한 수많은 액세스를 동기화할 때 
    // 발생하는 오버헤드를 방지할 수 있습니다. 

    // 각 반복에서 공유 리소스에 쓰는 것이 아니라 작업에 대한 모든 반복이 완료될 때까지 
    // 값을 계산하고 저장합니다. 

    // 그런 다음 최종 결과를 공유 리소스에 한 번만 쓰거나 다른 메서드에 전달할 수 있습니다. 

    class ThreadLocalFor
    {
        static void Main()
        {
            int[] nums = Enumerable.Range( 0, 101 ).ToArray();
            long total = 0;

            // Use type parameter to make subtotal a long, not an int
            // subtotal : Thread별 원자 변수, loop : 컴파일러가 자동으로 상태 설정
            // 다섯개의 매개변수( 0, nums.Length, loop, subtotal, Interlocked.Add 메서드)
            // 시작, 끝, 각 For 초기화변수, 순환상태, 각 For당 한 번 수행 되는 메서드
            Parallel.For<long>(
                // 시작, 끝 
                0, nums.Length, () => 
                // 스레드 지역변수 x = 0
                0, ( x, loop, subtotal ) =>
            {
                subtotal += nums[ x ];
                return subtotal;
            },
                ( subtotal ) => Interlocked.Add( ref total, subtotal )
            );

            Console.WriteLine( "The total is {0}", total );
            Console.WriteLine( "Press any key to exit" );
            Console.ReadKey();
        }
    }
}

// The first two parameters of every For method specify the beginning and ending iteration values.
// 모든 For 메소드는 두개의 매개 변수를 가지며, 이 변수는 시작 및 종료 반복 값을 지정합니다.

// 이 메서드 오버로드의 세 번째 매개 변수에서는 로컬 상태를 초기화합니다.
// 이때 "로컬 상태"란 현재 스레드에서 루프의 첫 번째 반복 직전부터 
// 마지막 반복 직후까지의 수명 기간을 갖는 변수를 의미합니다. 

// 세 번째 매개 변수의 형식은 "Func<TResult>" 입니다.
// 여기서 "http://TResult는_스레드_로컬상태를_저장" 하는 변수의 형식입니다. 
// 이 예제에서는 제네릭 버전의 메서드가 사용되며 형식 매개 변수는 long(Visual Basic에서는 Long)입니다.

// 이 형식 매개 변수는 스레드 로컬 상태를 저장하는 데 사용될 임시 변수의 형식을 컴파일러에 알려 줍니다.
// 이 예제에서 () => 0( Function() 0 ) 식은 스레드 지역 변수가 0으로 초기화됨을 나타냅니다.
// 이 형식 매개 변수가 참조 형식이거나 사용자 정의 값 형식인 경우 이 Func는 다음과 같이 나타납니다.

// 네 번째 형식 매개 변수에서는 루프 논리를 정의합니다.

// IntelliSense에서는 해당 형식이 Func<int, ParallelLoopState, long, long>
// 또는 Func(Of Integer, ParallelLoopState, Long, Long)임을 보여 줍니다.

// 람다 식에서는 세 개의 입력 매개 변수를 이러한 형식에 해당하는 순서와 동일한 순서로 사용해야 합니다.
// 마지막 형식 매개 변수는 반환 값입니다.

// 이 예제의 경우에는 For 형식 매개 변수에 지정한 형식이 long이므로 long 형식이 사용됩니다.
// 람다 식에서 해당 변수 subtotal을 호출하고 반환합니다.

// 반환 값은 각 후속 반복에서 부분합을 초기화하는 데 사용됩니다.
// 이 마지막 매개 변수는 각 반복에 전달되다가 마지막 반복이 완료될 때는 localFinally 대리자에 
// 전달되는 값이라고 생각할 수도 있습니다. 

// 5번째 매개 변수에서는 이 스레드의 모든 반복이 완료된 후 한 번만 호출되는 메서드를 정의합니다. 
// 이 입력 매개 변수의 형식은 다시 For 메서드의 형식 매개 변수와 본문 람다 식에서 반환되는 형식에 해당합니다.
// 이 예제에서는 스레드로부터 안전한 방식으로 클래스 범위의 변수에 값이 추가됩니다.

// 지금까지 모든 스레드의 모든 반복에서 이 클래스 변수에 값을 쓸 필요가 없도록 
// 스레드 로컬 변수를 사용하는 방법을 살펴보았습니다. 
