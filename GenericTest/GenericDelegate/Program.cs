using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GenericDelegate
{
    public partial class Program
    {
        public delegate void Del<T>( T item );
        public static void Notify( int i )
        {
            Console.WriteLine( i );
        }

        // 제네릭 대리자
        static void Main( string[] args )
        {
            Del<int> m1 = new Del<int>( Notify );
            m1( 5 );

            Del<int> m2 = Notify;
            m2( 8 );
            Console.WriteLine( m2 );

            // Stack<T> 이벤트 
            StackEventMethod();

            // Action 예제
            ActionPrint( "ActionPrint", 5 );

            Async_Action_Func();

            Diff_Func_Action_Predicate();
            Console.ReadLine();
        }
        private static void Diff_Func_Action_Predicate()
        {
            Thread.Sleep( 300 );
            Console.WriteLine( "\nFunc : 매개변수가 있거나 없거나 결과값 반환" );
            Func<int, string, decimal, string> ShowEmp = new Func<int, string, decimal, string>( CreateEmployee );
            Console.WriteLine( ShowEmp( 100, "john", 1000 ) );

            Console.WriteLine( "\nAction : 매개변수가 있거나 없거나 결과값을 반환하지 않음." );
            Action<string, string> GetFullName = new Action<string, string>( ShowFullName );
            GetFullName( "Davit", "Carperfield" );

            Console.WriteLine( "\nPredicate : 항상 Boolean 값으로 결과 반환 (형식 매개변수 반공변)" );
            Predicate<int> IsGreater = new Predicate<int>( IsGreaterThan100 );
            Console.WriteLine( IsGreater( 105 ) );
        }

        private static string CreateEmployee( int empNo, string empName, decimal salary )
        {
            return $"Employee # : {empNo}, \nEmployee Name : {empName}, \nSalary : {salary,2:c}";
        }
        private static void ShowFullName( string FirstName, string LastName )
        {
            Console.WriteLine( "Name : {0} {1}", FirstName, LastName );
        }
        private static bool IsGreaterThan100( int value )
        {
            return ( value > 100 ) ? true : false;
        }


        private static void Async_Action_Func()
        {
            Action action1 = () => { Console.WriteLine( "Hello, Action Example..." ); };
            Task.Factory.StartNew( action1 );

            Func<string> func1 = () => { return "Hello, Func Example"; };
            Task.Factory.StartNew( func1 );

            //Func<object, string> func2 = ( objectParam ) => { return objectParam.ToString(); };
            //Task.Factory.StartNew<string>( func2, objectParam );
        }
        // 대리자를 참조하는 코드에서는 다음과 같이 포함하는 클래스의 형식 매개 변수를 지정해야 합니다.
        private static void DoWork( float[] items ) { }
        public static void TestStack()
        {
            GenericStack<float> s = new GenericStack<float>();
            GenericStack<float>.StackDelegate d = DoWork;
        }

        /// <summary>
        /// Stack<T> 이벤트 설정.
        /// </summary>
        private static void StackEventMethod()
        {
            Stack<double> s = new Stack<double>();
            SampleClass o = new SampleClass();
            s.stackEvent += o.HandleStackChange;
        }
    }
    // sender 인수에 강력한 형식을 사용할 수 있고 Object 사이에서 캐스팅할 필요가 없으므로 제네릭 대리자는 
    // 특히 일반적인 디자인 패턴을 기반으로 이벤트를 정의할 때 유용합니다.
    delegate void StackEventHandler<T, U>( T sender, U eventArgs );
    class Stack<T>
    {
        public class StackEventArgs : EventArgs { }
        public event StackEventHandler<Stack<T>, StackEventArgs> stackEvent;

        protected virtual void OnStackChanged( StackEventArgs e )
        {
            stackEvent( this, e );
        }
    }
    class SampleClass
    {
        // 스택 이벤트 발생시 동작할 메소드
        public void HandleStackChange<T>( Stack<T> stack, Stack<T>.StackEventArgs args ) { }
    }

    class GenericStack<T>
    {
        T[] items;
        int index;

        public delegate void StackDelegate( T[] items );
    }

}
