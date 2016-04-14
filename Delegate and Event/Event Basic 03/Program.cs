using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Basic_03
{
    public class Button
    {
        public event SayHandler Click;

        public void OnClick(string msg)
        {
            if(Click != null)
            {
                Click( msg );
            }
        }
    }
    public delegate void SayHandler(string msg);
    class Program
    {
        public static void Say(string msg)
        {
            Console.WriteLine( msg );
        }
        static void Main( string[] args )
        {
            Program.Say( "안녕" ); Say( "안녕" );

            SayHandler sayHandler = new SayHandler(Say);
            sayHandler += new SayHandler( Program.Say );
            sayHandler( "방가" );

            Button btn = new Button();

            btn.Click += new SayHandler( Say );
            btn.Click += Say;

            btn.OnClick( "또 봐" );

            SayHandler hi = delegate( string msg )
            {
                Console.WriteLine( msg );
            };

            hi( "언제" ); hi( "언제" );

            Button button = new Button();

            button.Click += delegate( string msg )
            {
                Console.WriteLine( msg );
            };
            button.Click += delegate( string msg )
            {
                Console.WriteLine( msg );
            };
            
            button.OnClick( "내일" );
        }
    }
}
