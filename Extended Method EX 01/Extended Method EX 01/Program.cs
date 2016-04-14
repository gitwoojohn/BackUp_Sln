using System;

// 확장 메소드가 있는 네임 스페이스
using ExtendedMethod;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main( string[] args )
        {
            string s = "Hello WordCount Class Test?";
            int i = s.WordCount();
            Console.WriteLine( "Word Count : {0}", i );
            Console.ReadKey();
        }
    }
}
