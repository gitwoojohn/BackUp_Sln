using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWorld
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
    public class Battle
    {
        public static void Main()
        {
            // Robot.cs 클래스가 abstract 추상화로 선언 되었기 때문에
            // new keyword로 인스턴스화 시키면 에러 발생 -> Robot robot = new Robot("로봇"); 
            // 컴파일 에러 : 추상 클래스 또는 인터페이스의 인스턴스를 만들 수 없습니다.
            // 해결을 위해서는 Super_Robot가 Robot에 상속되어 있슴.
            // Super_Robot를 인스턴스화 시키면 문제 해결
            Super_Robot robot = new Super_Robot( "난 슈퍼 로봇\n" );
            robot.PrintRobotCount();
            robot.Change2Plane();
            robot.Shoot_GW();
            robot.OnRequestRepairEventHandler
                   += new Robot.RequestRepairEventHandler( Dr_Kim.RemoteRepair );
            robot.OnDamaged();
            Console.ReadKey();

        }
    }
}
//추상클래스와 추상 메소드에 대하여 살펴보았다. 물론, 추상클래스와 추상메소드가 무엇인지는 대충 알겠지만, 
//왜 사용하는지, 아직 이유를 파악하지 못한 독자도 있으리라고 본다. 

//이렇게 말하는 이유는, 지금 계속 설명하고 있는 객체지향, 상속, 다형성, 캡슐화 등이 
//모두 경험에 의해서 구현되는 것이기 때문이다. 

//즉, 설계 과정에서 어떤 것을 부모클래스로 두고, 어떤 클래스를 추상 클래스로 할지, 
//어떤 멤버를 부모에 두고 어떤 멤버를 자식에서 구현할지, 이 모든 것이 경험을 통해 자연스럽게 얻어지는 것이다.