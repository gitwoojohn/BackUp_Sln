using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegate_Basic_02
{
    public class Delegate_Sample
    {
        public delegate void Del( string message );
        public delegate void Del2( string str );

        public static void Notify( string name )
        {
            Console.WriteLine( "Notification received for : {0} ", name );
        }

        public static void Main()
        {
            Del handler = DelegateMethod;
            handler( "안녕" );

            MethodWithCallback( 1, 2, handler );

            //
            Del2 dell2 = new Del2( Notify );

            // C# 2.0
            Del2 dell3 = Notify;

            // C# 2.0 Over
            Del2 dell4 = delegate( string name )
            { Console.WriteLine( "Notification received for : {0}", name ); };

            // C# 3.0 over Rambda Expression
            Del2 dell5 = name => { Console.WriteLine( "Notification received for : {0}", name ); };

            dell5( "Test" );
            Console.ReadKey();
        }
        public static void DelegateMethod( string message )
        {
            Console.WriteLine( message );
        }
        public static void MethodWithCallback( int param1, int param2, Del callback )
        {
            callback( "The number is: " + ( param1 + param2 ).ToString() );
        }
    }
}
