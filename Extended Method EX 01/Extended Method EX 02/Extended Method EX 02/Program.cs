
// IMyInterface 인터페이스 정의. 
namespace DefineIMyInterface
{

    public interface IMyInterface
    {
        // IMyInterface를 각 클래스에서 아래 서명과 일치하는 메서드를 정의 해야 합니다.
        void MethodB();
    }
}

// 기존에 클래스에는 없는 메서드를 추가 사용하기 위해서 확장 메서드 구현
namespace Extensions
{
    using System;

    // 인터페이스 네임스페이스 추가
    using DefineIMyInterface;

    // 아래 확장 메서드는 IMyInterface를 구현하는 모든 클래스에서 인스턴스 접근 가능.
    public static class Extension
    {
        public static void MethodA( this IMyInterface myInterface, int i )
        {
            Console.WriteLine
                ( "Extension.MethodA(this IMyInterface myInterface, int i)" );
        }

        public static void MethodA( this IMyInterface myInterface, string s )
        {
            Console.WriteLine
                ( "Extension.MethodA(this IMyInterface myInterface, string s)" );
        }

        public static double MethodA( this IMyInterface myInterface, double i )
        {
            Console.WriteLine
                ( "Extension.MethodA(this IMyInterface myInterface, double i)" );
            return i;
        }

        // ExtensionMethodsDemo1의 이 메서드(MethodB)는 절대 호출 되지 않는다.
        // 왜나하면 세개의 클래스 A, B, C에는 시그니쳐와 매칭되는 MethodB가 있기 때문에.
        public static void MethodB( this IMyInterface myInterface )
        {
            Console.WriteLine
                ( "Extension.MethodB(this IMyInterface myInterface)" );
        }
    }
}


// IMyInterface의 구현을 세개의 클래스에서 정의. 확장 메소드를 사용하여 테스트
namespace ExtensionMethodsDemo1
{
    using System;
    using DefineIMyInterface;
    using Extensions;

    class A : IMyInterface
    {
        public void MethodB() { Console.WriteLine( "A.MethodB()" ); }
    }

    class B : IMyInterface
    {
        public void MethodB() { Console.WriteLine( "B.MethodB()" ); }
        public void MethodA( int i ) { Console.WriteLine( "B.MethodA(int i)" ); }
    }

    class C : IMyInterface
    {
        public void MethodB() { Console.WriteLine( "C.MethodB()" ); }
        public void MethodA( object obj )
        {
            Console.WriteLine( "C.MethodA(object obj)" );
        }
    }

    class ExtMethodDemo
    {
        static void Main( string[] args )
        {
            // Declare an instance of class A, class B, and class C.
            A a = new A();
            B b = new B();
            C c = new C();

            // For a, b, and c, call the following methods: 
            //      -- MethodA with an int argument 
            //      -- MethodA with a string argument 
            //      -- MethodB with no argument. 

            // A contains no MethodA, so each call to MethodA resolves to  
            // the extension method that has a matching signature.
            // 클래스 A에는 MethodA 메서드가 구현되 있지 않으므로 Extension의
            // MethodA 에 맞는 데이터 타입(오버로딩) 호출
            a.MethodA( 1 );          // Extension.MethodA(object, int)
            a.MethodA( "hello" );    // Extension.MethodA(object, string) 
            // 추가 코드 ( 반환값도 되나 안되나)
            Console.WriteLine( $"반환값 : {a.MethodA( 1.1 )}" );  // Extension.MethodA(object, double) 
            Console.WriteLine( "----------------------------------------------------------" );
            // A has a method that matches the signature of the following call 
            // to MethodB.
            a.MethodB();            // A.MethodB() 
            Console.WriteLine( "----------------------------------------------------------" );
            // B has methods that match the signatures of the following 
            // nethod calls.
            b.MethodA( 1 );         // B.MethodA(int)
            b.MethodB();            // B.MethodB() 
            Console.WriteLine( "----------------------------------------------------------" );
            // B has no matching method for the following call, but  
            // class Extension does.
            b.MethodA( "hello" );   // Extension.MethodA(object, string) 
            Console.WriteLine( "----------------------------------------------------------" );
            // C contains an instance method that matches each of the following 
            // method calls.
            c.MethodA( 1 );         // C.MethodA(object)
            c.MethodA( "hello" );   // C.MethodA(object)
            c.MethodB();            // C.MethodB()

            Console.ReadLine();
        }
    }
}
/* Output:
    Extension.MethodA(this IMyInterface myInterface, int i)
    Extension.MethodA(this IMyInterface myInterface, string s)
    A.MethodB()
    B.MethodA(int i)
    B.MethodB()
    Extension.MethodA(this IMyInterface myInterface, string s)
    C.MethodA(object obj)
    C.MethodA(object obj)
    C.MethodB()
 */
