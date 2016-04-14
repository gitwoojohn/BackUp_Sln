using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Basic_02
{
    // 대리자 선언
    public delegate void Say();
    public class Button
    {
        // 대리자 이벤트 선언
        public event Say Click;
        public void OnClick()
        {
            if( Click != null)
            {
                Click();
            }
        }
    }
    public class Hello
    {
        public static void Hi1()
        {
            Console.WriteLine( "안녕하세요!" );
        }
        public static void Hi2()
        {
            Console.WriteLine( "반갑습니다." );
        }
    }
}
