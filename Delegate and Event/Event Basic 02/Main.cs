using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Basic_02
{   
    class Program
    {
        static void Main( string[] args )
        {
            // 대리자 예제
            Hello.Hi1(); Hello.Hi2();

            Say say = new Say(Hello.Hi1);
            //say += new Say( Hello.Hi1 );
            say += new Say( Hello.Hi2 );
            say();

            // 이벤트 예제
            Button btn = new Button();
            btn.Click += new Say( Hello.Hi1 );
            btn.Click += new Say( Hello.Hi2 );

            Console.WriteLine( "================" );
            btn.OnClick();
        }
    }
}
