using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    delegate void D은행거래();
    class 사원
    {
        public D은행거래 통장거래;
        public void 통장정리()
        {
            if (통장거래 == null)
            {
                Console.WriteLine( "잔액부족" );
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine( "월급님이 로그인하셨습니다." );
                통장거래();
                Console.WriteLine( "월급님이 로그아웃하셨습니다." );
            }
        }
    }
    /// <summary>
    /// += () => 델리게이트(대리자) 사용시 익명으로 선언해서 결합
    /// " => "  이동 연산자라고도 하며 " = " 할당 연산자와 순위가 같고
    /// 오른쪽 결합성이 있다.
    /// 
    /// " => "  토큰을 람다 연산자라고도 하며 "오른쪽 람다 본문" 과 "왼쪽의 입력 변수" 로 구분
    /// </summary>
    class Program
    {
        static void Main( string[] args )
        {
            사원 박대리 = new 사원();
            박대리.통장정리();
           

            박대리.통장거래 += () => Console.WriteLine( "○○카드 : 퍼가용~♡" );
            박대리.통장거래 += () => Console.WriteLine( "××카드 : 퍼가용~♡" );
            박대리.통장거래 += () => Console.WriteLine( "국민연금 : 퍼가용~♡" );
            박대리.통장거래 += () => Console.WriteLine( "△△카드 : 퍼가용~♡" );
            박대리.통장거래 += () => Console.WriteLine( "의료보험 : 퍼가용~♡" );
            박대리.통장거래 += () => Console.WriteLine( "교통카드 : 퍼가용~♡" );
            박대리.통장정리();
        }
    } 
}
