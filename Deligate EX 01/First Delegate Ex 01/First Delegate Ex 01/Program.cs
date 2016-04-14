using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    /*      
        대리자는 C++의 포인터 함수 비슷하고 형식 안정
        (Delegates are like C++ function pointers but are type safe.)
        
        대리자 메서드는 매개변수를 보낼수 있습니다.
        (Delegates allow methods to be passed as parameters.)
        
        대리자는 이벤트 핸들링을 위해 콜백 메서드를 사용할 수 있습니다.
        (Delegates are used in event handling for defining callback methods.)
        
        대리자는 다른 메서드를 결합할 수 있습니다. 즉, 실행 가능한 메서드들을 결합해서 하나의 unit으로 만들수 있습니다.
        (Delegates can be chained together i.e. these allow defining a set of methods that executed as a single unit.)

        대리자가 만들어지면 그 메서드의 타입을 변경할 수 없습니다. ( int a ) -> ( double a )  왜냐하면 변경 불가능한 타입이기 때문에
        (Once a delegate is created, the method it is associated will never changes because delegates are immutable in nature.)

        대리자는 런타임에 method 실행 방법을 제공합니다.
        (Delegates provide a way to execute methods at run-time.)

        모든 대리자는 System.Delegate 클래스로에서 상속되며, System.MulticastDelegate 클래스로 부터 암시적으로 파생됩니다.
        (All delegates are implicitly derived from System.MulticastDelegate, class which is inheriting from System.Delegate class.)

        대리자 타입들은 시그니처가 동일한 경우에도 서로 호환하지 않습니다. 대리자는 동일한 메서드 참조를 가지고 있으면 동일한 것으로 간주합니다.
        Delegate types are incompatible with each other, even if their signatures are the same. These are considered equal if they have the reference of same method.
    */
    delegate void deleMath1( int Value ); // 델리게이트 선언
    delegate void deleMath2( int a, int b );

    class MainClass
    {
        static void Main( string[] args )
        {
            //Math클래스 선언및 인스턴스화
            MathClass MathClass = new MathClass();

            // 델리게이트 선언 및 인스턴스화(덧셈)
            deleMath1 Math = new deleMath1( MathClass.Plus );

            // 대리자 연산(뺄셈,곱셈추가)
            Math += new deleMath1( MathClass.Minus );
            Math += new deleMath1( MathClass.Multiply );
            Math += new deleMath1( MathClass.Divide );

            //결과1
            MathClass.Number = 10;
            Math( 10 );
            Console.WriteLine( "Result: {0}", MathClass.Number );

            Math -= new deleMath1( MathClass.Minus );
            //결과2
            MathClass.Number = 10;
            Math( 10 );
            Console.WriteLine( "Result: {0}", MathClass.Number );

            Math -= new deleMath1( MathClass.Multiply );
            //결과3
            MathClass.Number = 10;
            Math( 10 );
            Console.WriteLine( "Result: {0}", MathClass.Number );

            Console.WriteLine( "\n멀티 델리게이트 예제 2" );
            MathClass math2 = new MathClass();
            deleMath2 multicastDelegate = new deleMath2( math2.Sum );
            multicastDelegate += new deleMath2( math2.Diff );

            multicastDelegate( 20, 10 );
            Console.ReadLine();
        }

        public class MathClass
        {
            public int Number;
            public void Plus( int Value ) // 더하기
            {
                Number += Value;
            }
            public void Minus( int Value ) // 빼기
            {
                Number -= Value;
            }
            public void Multiply( int Value ) // 곱하기
            {
                Number *= Value;
            }
            public void Divide( int Value ) // 나누기
            {
                Number /= Value;
            }

            public MathClass() { }
            public MathClass( int a, int b ) { }

            public void Sum( int a, int b )
            {
                Console.WriteLine( "Both Number Sum : {0}", a + b );
            }
            public void Diff( int a, int b )
            {
                Console.WriteLine( "Both Number Diff : {0}", a - b );
            }
        }
    }
}
