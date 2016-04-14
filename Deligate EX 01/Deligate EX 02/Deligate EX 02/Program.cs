using System;

public delegate void MyEventDelegate(string senderName);

namespace Deligate_EX_02
{
    public class Count
    {
        static int nCount;
        public static int GetCount()
        {
            return nCount++;
        }
    }

    public class MyEventClass1
    {   // event 지시어         이벤트 프로퍼티 이름
        public event MyEventDelegate FireEvent;

        public void FireEventNow()
        {
            if (FireEvent != null)
            {
                Console.WriteLine("{0} FireEventNow was called", Count.GetCount());
                FireEvent("myEventClass1");
            }
        }
    }

    public class MyEventClass2
    {
        public event MyEventDelegate FireEvent;

        public void FireEventNow()
        {
            if (FireEvent != null)
            {
                Console.WriteLine("{0} FireEventNow was called", Count.GetCount());
                FireEvent("myEventClass2");
            }
        }
    }

    public class Mainclass
    {
        static private void EventCallThisFunction(string senderName)
        {
            Console.WriteLine("parameter is {0}", senderName);
            Console.WriteLine("{0} EventCallThisFunction Was called", Count.GetCount());
        }

        static public void Main()
        {
            MyEventClass1 eventclass1 = new MyEventClass1();
            eventclass1.FireEvent += new MyEventDelegate(EventCallThisFunction);
            eventclass1.FireEventNow();

            Console.WriteLine("===============================================");

            MyEventClass2 eventclass2 = new MyEventClass2();
            eventclass2.FireEvent += new MyEventDelegate(EventCallThisFunction);
            eventclass2.FireEventNow();
            
            Console.ReadKey();
        }
    }

}
