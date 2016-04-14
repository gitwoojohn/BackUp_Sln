using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstract_EX_02
{
    /*
     추상 클래스의 특징( Features of Abstract Class )

     인스턴스화 하지 않는다.
     An abstract class cannot be instantiated. 

     추상 클래스는 추상 멤버뿐만 아니라 비 추상 멤버가 포함되어 있습니다.
     An abstract class contain abstract members as well as non-abstract members.

     봉인된(Sealed)클래스는 추상클래스가 될 수 없습니다. 왜냐하면 봉인된(sealed) 한정자(modifier) 클래스는 상속을 방지하기 때문에 추상클래스 한정자는 클래스 상속을 필요로 합니다. 
     An abstract class cannot be a sealed class because the sealed modifier prevents a class from being inherited and the abstract modifier requires a class to be inherited.

     추상 클래스에서 파생 된 비 추상 클래스는 상위 추상 클래스의 모든 추상 멤버의 실제 구현을 포함해야합니다.
     A non-abstract class which is derived from an abstract class must include actual implementations of all the abstract members of parent abstract class.

     추상 클래스는 클래스와 하나의 인터페이스 또는 그 이상의 인터페이스에서 상속 할 수 있습니다.   
     An abstract class can be inherited from a class and one or more interfaces.

     추상 클래스는 클래스 멤버와 내부, 보호, 개인 등의 액세스 한정자를 가질 수 있습니다. 그러나 추상 Members들은 개인 액세스 한정자를 가질 수 없습니다.   
     An Abstract class can has access modifiers like private, protected, internal with class members. But abstract members cannot have private access modifier.

     추상 클래스는 가변적으로 인스턴스 할 수 있습니다.( 상수나 필드 멤버 처럼)
     An Abstract class can has instance variables (like constants and fields).

     추상 클래스는 생성자와 소멸자를 가집니다.
     An abstract class can has constructors and destructor.

     추상 클래스의 가상 메서드는 암시적 메서드 입니다.
     An abstract method is implicitly a virtual method.

     추상 속성들은 추상 메서드처럼 동작 합니다.
     Abstract properties behave like abstract methods.

     추상 클래스는 구조체에 의해 상속될 수 없습니다.
     An abstract class cannot be inherited by structures.

     추상 클래스는 다중 상속을 지원하지 않습니다.
     An abstract class cannot support multiple inheritance.

     When to use:
      
     구성요소의 여러 버전을 만들 필요가 있는 경우 추상클래스라면 문제가 되지 않습니다. 
     모든 상속 클래스가 자동으로 변화와 업데이트 없이 추상 클래스에 속성 또는 메서드를 추가 할 수 있습니다.
     Need to create multiple versions of your component since versioning is not a problem with abstract class. You can add properties or methods to an abstract class without breaking the code and all inheriting classes are automatically updated with the change.

     기본 동작뿐만 아니라 여러 파생 클래스가 공유하고 대체 할 수있는 일반적인 동작을 제공 할 필요가 있습니다.
     Need to to provide default behaviors as well as common behaviors that multiple derived classes can share and override.
    */
    abstract class ShapesClass
    {
        abstract public int Area();
    }
    class Square : ShapesClass
    {
        int side = 0;
        public Square( int n )
        {
            side = n;
        }

        // Override area method
        public override int Area()
        {
            return side * side;
        }
    }

    class Rectangle : ShapesClass
    {
        int length = 0, width = 0;
        public Rectangle( int length, int width )
        {
            this.length = length;
            this.width = width;
        }

        public override int Area()
        {
            return length * width;
        }
    }

    /*
    // Common design guidelines for Abstract Class

    추상 클래스에서 public 생성자를 정의하지 마십시요. 
    추상 클래스의 인스턴스가 public 액세스 한정자와 생성자는 인스턴스화 할 수 있는 클래스에 가시성을 제공 할 수 없기 때문입니다.

    추상 클래스내에서 보호 또는 내부 생성자를 정의합니다.
    보호된 생성자는 서브 클래스가 생성되고 내부 생성자 클래스를 포함하는 어셈블리에 추상 클래스의 구체적인 구현을 
    제한 할 수 있는 경우 기본 클래스는 자체 초기화를 수행 할 수 있기 때문입니다.

    */
    class Program
    {
        static void Main( string[] args )
        {
            Square square = new Square( 10 );
            Console.WriteLine( "Square Area : {0}", square.Area() );

            Rectangle rectangle = new Rectangle( 10, 20 );
            Console.WriteLine( "Rectangle Area : {0}", rectangle.Area() );

            Console.ReadKey();
        }
    }
}
