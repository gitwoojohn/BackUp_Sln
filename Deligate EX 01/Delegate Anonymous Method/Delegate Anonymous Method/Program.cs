using System;

namespace ConsoleApplication1
{
    // Declare a delegate.
    delegate void Printer( string s );

    /// <summary>
    /// 대지자를 인스턴스화 시키는 2가지 예제
    /// 대리자를 무명 메서드에 연결
    /// 대리자를 명명된 메서드(DoWork)에 연결
    /// </summary>
    class TestClass
    {
        static void Main()
        {
            // Instatiate the delegate type using an anonymous method.
            // 익명 메서드 형식을 사용해서 대리자 인스턴스
            Printer p = delegate ( string outputString )
            {
                Console.WriteLine( outputString );
            };

            // Results from the anonymous delegate call.
            p( "The delegate using the anonymous method is called." );

            // The delegate instantiation using a named method "DoWork".
            p = new Printer( DoWork );

            // Results from the old style delegate call.
            p( "The delegate using the named method is called." );
        }

        // The method associated with the named delegate.
        static void DoWork( string outputString )
        {
            Console.WriteLine( outputString );
        }
    }
    /* Output:
        The delegate using the anonymous method is called.
        The delegate using the named method is called.
    */
}

//무명 메서드의 매개 변수 범위는 무명 메서드 블록입니다.

//무명 메서드 블록 안에서 블록 밖을 대상으로 goto, break 또는 continue와 같은 점프 문을 사용하면 오류가 발생합니다. 

//지역 변수 및 매개 변수의 범위에 무명 메서드 선언이 포함되는 경우 이러한 변수를 무명 메서드의 외부 변수라고 합니다. 
//예를 들어, 다음 코드 단편에서 n은 외부 변수입니다.

//int n = 0;
//Del d = delegate() { System.Console.WriteLine("Copy #:{0}", ++n); };

