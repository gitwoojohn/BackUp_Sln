using System ;

// 오버라이드와 오버로드의 차이점
// 오버로드는 같은 클래스 안에서, 비슷한 역할을 하며, 같은 타입의 결과를 리턴하는 함수를 중복정의 하는 것을 말하며, 
// 오버라이드는 상속관계에서, 부모 클래스가 가진 함수를 자식클래스가 재정의 하는 것을 의미한다.
 
public struct POINT
{
        public POINT(int xi,int yi)
        {
                x = xi;
                y = yi;
        }
        public int x;
        public int y;
}
 
public enum General_Weapon
{
        돌맹이,
        바위,
        총알
}
public enum Special_Weapon
{
        대포,
        유도탄,
        레이져
}

public enum WeaponMode
{
    일반무기,
    특수무기,
    둘다발사
}

// EventArgs 클래스를 상속받아서 사용 
//RobotMessageEventArgs 클래스는 EventArgs 클래스를 상속받아서, 
// EventArgs의 기능을 사용할 수 있도록 한다는 의미이다
// 로봇이 보내는 메세지 매개인자.
public class RobotMessageEventArgs : EventArgs
{
        public string   strMessage;     // 김박사에게 보낼 메세지 

        public RobotMessageEventArgs(string message)
        {
            strMessage = message;
        }
}
     
public class Robot
{
        private string          m_strName;       //이름
        private string          m_Color;         //색깔
        private int             m_nEnergy;       //에너지
        private General_Weapon  m_generalW;      //일반무기
        private Special_Weapon  m_specialW;      //특수무기
        private POINT           m_point;         //현재위치
        private int             m_nPowerUP;      //몇 대가 합체된 상태인지?
        private WeaponMode      m_WeaponMode;    //현재 무기발사 모드
 
        private static  int nRobotCount=0;      //만들어진 로봇의 갯수
 
        public delegate int ShootDelegate(int n);
 
        // 이벤트 핸들러를 위한 델리게이트 작성
        public delegate void RequestRepairEventHandler(object source, RobotMessageEventArgs e);
 
        public ShootDelegate Shoot;
 
        // RequestRepairEventHandler 형으로 이벤트가 발생되었을때 호출될 메소드
        public event RequestRepairEventHandler OnRequestRepairEventHandler;
 
        public Robot():this("","",new POINT(0,0),1)
        {               
        }

        public Robot(string strName)
            : this(strName, "", new POINT(0, 0), 1)
        {
        }

        public Robot(int nPowerUp)
            : this("", "", new POINT(0, 0), nPowerUp)
        {
        }

        public Robot(POINT point)
            : this("", "", point, 1)
        {
        }

        public Robot(string strName, string strColor, POINT point, int nPowerUp)
        {
            m_strName = strName;                 //이름
            m_Color = strColor;                  //색깔
            PowerUP = nPowerUp;                  //파워업 레벨
            m_point = point;                     //현재위치
            m_nEnergy = 50;                      //에너지
            m_generalW = General_Weapon.돌맹이;  //일반무기
            m_specialW = Special_Weapon.유도탄;  //특수무기
            nRobotCount++;                       //만들어진 로봇의 숫자를 증가시킴~
            Weapon_Mode = WeaponMode.일반무기;   //초기 무기발사 모드를 일반무기로 설정

            Console.WriteLine("========================================");
            Console.WriteLine("Robot 클래스가 생성자가 호출되었습니다.");
            Console.WriteLine("========================================"); 
        }

        ~Robot()
        {
            Console.WriteLine("========================================");
            Console.WriteLine("Robot 클래스가 소멸자가 호출되었습니다.");
            Console.WriteLine("========================================");
        } 

        //만들어진 로봇의 개수 프로퍼티(읽기전용)       
        public static int RobotCount
        {
            get
            {
                return nRobotCount;
            }
        }

        public string Name                      //로봇이름
        {
            get
            {
                return m_strName;
            }
        }

        public string Color                     //로봇색깔
        {
            get
            {
                return m_Color;
            }
        }

        public int Energy                       //에너지
        {
            get
            {
                return m_nEnergy;
            }
        }

        public POINT Point                      //현재위치
        {
            get
            {
                return m_point;
            }
        }

        public int PowerUP                      //파워 업 단계 프로퍼티
        {
            get
            {
                return m_nPowerUP;
            }
            set
            {
                m_nPowerUP = value;

                //파워 업 단계별로 무기가 강화됨
                switch (m_nPowerUP)
                {
                    case 1:
                        m_generalW = General_Weapon.돌맹이;
                        m_specialW = Special_Weapon.대포;
                        break;
                    case 2:
                        m_generalW = General_Weapon.바위;
                        m_specialW = Special_Weapon.유도탄;
                        break;
                    case 3:
                        m_generalW = General_Weapon.총알;
                        m_specialW = Special_Weapon.레이져;
                        break;
                    default:
                        m_nPowerUP = 1;
                        m_generalW = General_Weapon.돌맹이;
                        m_specialW = Special_Weapon.대포;
                        break;
                }
            }
        }

        public WeaponMode Weapon_Mode    //무기 발사모드 프로퍼티
        {
            get
            {
                return m_WeaponMode;
            }
            set
            {
                m_WeaponMode = value;

                if (m_WeaponMode == WeaponMode.일반무기)
                    Shoot = new ShootDelegate(Shoot_DDGW);
                else if (m_WeaponMode == WeaponMode.특수무기)
                    Shoot = new ShootDelegate(Shoot_DDSW);
            }
        }

        //이벤트를 발생시킬 때 사용되는 매개인자는 두 개가 필요하다. 
        //이벤트를 보낼 객체는 로봇 자신이므로 this 키워드를 사용하고, 이벤트의 매개인자로 사용될 
        //RobotMessageEventArgs 객체를 생성한다. 
        //그리고 이벤트 핸들러인 OnRequestRepairEventHandler를 this 키워드와 이벤트 매개인자 e를 이용해서 호출한다.
        //로봇이 손상을 입었음
        public void OnDamaged()
        {
            Console.WriteLine("Robot : 로봇이 손상을 입었습니다.");

            RobotMessageEventArgs e = new RobotMessageEventArgs("박사님, 수리해주세요");
            OnRequestRepairEventHandler(this, e);

            Console.WriteLine("Robot : 김박사에게 수리를 요청했습니다.");
        }
 
        //로봇을 진단하고 원격으로 고치는 함수
        public void CheckNRepair()
        {
                Console.WriteLine("Robot : 수리가 완료되었습니다.");
        }

        public General_Weapon GW                        //일반무기 프로퍼티
        {
                get
                {
                        return m_generalW;
                }
                set
                {
                        m_generalW=value;
                        m_nEnergy--;                            //일반무기 변경시 에너지 1감소
                }
        }

        public Special_Weapon SW                        //특수무기 프로퍼티
        {
            get
            {
                return m_specialW;
            }
            set
            {
                m_specialW = value;
                m_nEnergy -= 2;                           //특수무기 변경시 에너지 2감소
            }
        }
 
        //앞으로 가기
        public void Go_Ahead()
        {
                m_point.x++;
        }
 
        //뒤로 가기
        public void Go_Back()
        {
                m_point.x--;
        }
 
        //위로 가기
        public void Go_Up()
        {
                m_point.y--;
        }
 
        //아래로 가기
        public void Go_Down()
        {
                m_point.y++;
        }
 
        //일반무기 발사
        public void Shoot_GW()
        {
            Console.WriteLine(m_generalW.ToString() + "발사");
        }
 
        //특수무기 발사
        public virtual void Shoot_SW()
        {
            Console.WriteLine(m_specialW.ToString() + "발사");
        }

        //만들어진 로봇의 개수를 출력하고, 개수를 리턴하는 함수
        public static int PrintRobotCount()
        {
            Console.WriteLine("현재 만들어진 로봇의 갯수는 ? "+ nRobotCount + "개");
            return nRobotCount;
        }        

        //에너지 충전
        public void Recharge()
        {
            m_nEnergy = 100;
        }
 
        //일반무기 따따블 발사
        public int Shoot_DDGW(int n)
        {
            int i;

            for (i = 0; i < n; Shoot_GW()) ;

            return i;
        }

        public int Shoot_DDSW(int n)
        {
            int i;

            for (i = 0; i < n; Shoot_SW()) ;

            return i;
        }
}
 
public class Dr_Kim
{
    // 매개인자 오브젝트와 이벤트 매개 인자 e 사용
    public static void RemoteRepair(object obj, RobotMessageEventArgs e)
    {
        if (e.strMessage == "박사님, 수리해주세요")
        {
            Console.WriteLine("김박사 : 로봇으로부터 수리요청을 받았습니다.");

            // 이벤트 매개인자를 통해서 현재 로봇의 레퍼런스를 알 수 있다.
            Robot robot = (Robot)obj;

            Console.WriteLine("김박사 : 원격으로 로봇을 진단하고, 수리합니다.");

            //로봇을 진단하고 치료함
            robot.CheckNRepair();
        }
    }
}

public class Super_Robot : Robot
{
    // 현재 비행기로 변신한 상태인지, 로봇 상태인지 
    private bool m_bPlane;

    // 현재 슈퍼로봇의 속도 
    private int m_nSpeed;

    public Super_Robot(string strName)
        : base(strName)
    {
        // 초기를 로봇으로 설정 
        m_bPlane = false;

        // 초기 로봇의 속도 
        m_nSpeed = 100;

        // 초기 에너지를 100으로 
       //  this.Recharge();
        base.Recharge();

        Console.WriteLine("========================================");
        Console.WriteLine("Super_Robot 클래스의 생성자가 호출되었습니다.");
        Console.WriteLine("========================================"); 
    }

    ~Super_Robot()
    {
        Console.WriteLine("========================================");
        Console.WriteLine("Super_Robot 클래스의 소멸자가 호출되었습니다..");
        Console.WriteLine("========================================");
    }


    // new keyword를 이용한 오버라이드
    // public new void Shoot_SW()
    // override keyword를 사용 하려면 Robot 부모 클래스의 멤버 함수를 virtual로 선언
    // 특수무기 발사 
    public override void Shoot_SW()
    {
        Console.WriteLine("Super_Robot용 특수무기 발사");
    } 

    public void Change2Plane()
    {
        // 현재 상태가 비행기이면 리턴 
        if (m_bPlane)
            return;

        m_bPlane = true;

        //속도를 높임 
        m_nSpeed = 200;

        Console.WriteLine(this.Name + "비행기로 변신! " + "현재 속도" +
                   m_nSpeed);
    }

    public void Change2Robot()
    {
        // 현재 상태가 로봇이면 리턴 
        if (!m_bPlane)
            return;

        m_bPlane = false;

        //속도를 낮춤 
        m_nSpeed = 100;

        Console.WriteLine(this.Name + "비행기로 변신! " + "현재 속도" + m_nSpeed);
    }    
} 


public class Battle
{
    public static void Main()
    {
        // 슈퍼로봇 생성
        Super_Robot superRobot = new Super_Robot("난 슈퍼로봇");
        superRobot.Shoot_SW();


        // new 키워드를 사용하면 상속관계 무시하고 오버라이드 함
        // 아래의 예는 슈퍼로봇을 일반로봇으로 형(껍데기만 변환 했지만 슈퍼로봇의 특수 무기를 사용하지 못함)
        // 그러나, 미리 Virtual Override 해 두었기 때문에 일반로봇으롤 형변환(껍데기만) 했지만 슈퍼 로봇의 무기 사용 가능
        
        // 위와 같이, 껍데기는 바뀌었지만, 인스턴스는 분명히 Super_Robot이므로 껍데기를 Robot으로 바꾸어서 전쟁에 나가도
        // Super_Robot의 특수무기를 그대로 쓸 수 있는 것이다.
        // 즉, new 키워드를 사용하면, 상속관계를 무시하여, 껍데기만 바꾸어도, 실제로 그 객체가 어떤 객체인지 고려하지 않지만, 
        // virtual~override를 사용하면, 형변환이 되어도, 실제 객체를 인지하여 원래의 인스턴스의 메소드를 호출 할 수 있는 것이다. 

        // 슈퍼로봇을 일반 로봇으로 형변환 
        Robot robot = superRobot;
        robot.Shoot_SW();


        Super_Robot SR = new Super_Robot("슈퍼로봇");
        Console.WriteLine("슈퍼로봇 이름 " + SR.Name);
        Console.WriteLine("슈퍼로봇 에너지 " + SR.Energy);
        Console.WriteLine("슈퍼로봇 일반무기 " + SR.GW);
        Console.WriteLine("슈퍼로봇 특수무기 " + SR.SW);

       // Robot robot = new Robot("일반로봇");

        ////Robot의 갯수 출력 
        Super_Robot.PrintRobotCount();


        SR.Change2Plane();
        SR.Change2Robot();
        Robot robotA = new Robot();
        robotA.OnRequestRepairEventHandler
                += new Robot.RequestRepairEventHandler(Dr_Kim.RemoteRepair);
        robotA.OnDamaged();

        Console.ReadKey();
    }
}
