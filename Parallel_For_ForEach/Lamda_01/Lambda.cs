using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lamda_01
{
    delegate bool D();
    delegate bool D2( int i );

    class Lambda
    {
        D del;
        D2 del2;
        public void TestMethod( int input )
        {
            int j = 0;
            // Initialize the delegates with lambda expressions.
            // Note access to 2 outer variables.
            // del will be invoked within this method.
            del = () => { j = 10; return j > input; };

            // del2 will be invoked after TestMethod goes out of scope.
            del2 = ( x ) => { return x == j; };

            // Demonstrate value of j:
            // Output: j = 0 
            // The delegate has not been invoked yet.
            Console.WriteLine( "j = {0}", j );        // Invoke the delegate.
            bool boolResult = del();

            // Output: j = 10 b = True
            Console.WriteLine( "j = {0}. b = {1}", j, boolResult );
        }

        static void Main( string[] args )
        {
            // 결과 홀수 5개
            int[] numbers = { 4, 8, 6, 3, 9, 0, 1, 7, 2, 5 };
            var oddNumbers = numbers.Count( n => n % 2 == 1 );
            Console.WriteLine( oddNumbers );

            // 결과 : false
            Func<int, bool> myFunc = x => x == 5;
            bool result = myFunc( 4 );


            var firstSmallNumbers = numbers.TakeWhile( ( n, index ) => n >= index );
            foreach( var item in firstSmallNumbers )
            {
                Console.Write( "{0}  ", item );
            }

            // 
            Lambda test = new Lambda();
            test.TestMethod( 5 );

            // Prove that del2 still has a copy of
            // local variable j from TestMethod.
            bool result1 = test.del2( 10 );

            // Output: True
            Console.WriteLine( result1 );

            Console.ReadKey();
        }
    }
}
