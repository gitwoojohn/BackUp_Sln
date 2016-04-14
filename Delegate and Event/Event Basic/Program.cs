using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 대리자는 메서드를 안전하게 캡슐화하는 형식으로 C및 C++의 함수 포인터와 유사하다.
// 그러나, C나 C++보다 개체 지향적이고 형식상의 안전성을 제공하며 보안상 안전하다.

namespace Event_Basic
{
    public delegate void MyEventHandler( string message );
    class Publisher
    {
        public event MyEventHandler Active;
        public void DoActive( int number )
        {
            if (number % 10 == 0)
                Active( "Active : " + number );
            else
                Console.WriteLine( number );
        }
    }
    class Subscriber
    {      
        static public void MyHandler(string message)
        {
            Console.WriteLine( message );
        }
        static void Main( string[] args )
        {
            Publisher publisher = new Publisher();
            publisher.Active += new MyEventHandler(MyHandler);

            for(int i = 1; i < 21; i++)
            {
                publisher.DoActive( i );
            }
        }
    }
}
