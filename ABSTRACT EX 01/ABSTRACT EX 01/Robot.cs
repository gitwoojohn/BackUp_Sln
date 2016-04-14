using System;
namespace RobotWorld
{
    // 로봇이 보내는 메세지 매개인자.
    public class RobotMessageEventArgs : EventArgs
    {
        public string strMessage; // 김박사에게 보낼 메세지
        // 생성자
        public RobotMessageEventArgs(string message)
        {
            strMessage = message;
        }
    }

    public abstract class Robot
    {
        private string m_strName; //이름
        private int m_nPowerUP; //몇 대가 합체된 상태인지?
        private WeaponMode m_WeaponMode; //현재 무기발사 모드
        //private static int nRobotCount=0; //만들어진 로봇의 갯수

        private string m_Color; //색깔
        private int m_nEnergy; //에너지
        private POINT m_point; //현재위치
        public delegate int ShootDelegate(int n);
        public ShootDelegate Shoot;

        // 이벤트 핸들러를 위한 델리게이트 작성
        public delegate void RequestRepairEventHandler(object source, RobotMessageEventArgs e);
        // RequestRepairEventHandler 형으로 이벤트가 발생되었을때 호출될 메소드           
        public event RequestRepairEventHandler OnRequestRepairEventHandler;
        
        //일반무기 발사    
        public abstract void Shoot_GW();
        //특수무기 발사
        public abstract void Shoot_SW();
        
        //만들어진 로봇의 개수를 출력하고, 개수를 리턴하는 함수 
        public abstract int PrintRobotCount();
        
        //생성자 초기화
        public Robot()
            :this("", "", new POINT(0, 0), 1)
        {
        }

        public Robot(string strName)
            :this(strName, "", new POINT(0, 0), 1)
        {                    
        }

        public Robot(int nPowerUp)
            :this("", "", new POINT(0, 0), nPowerUp)
        {                   
        }

        public Robot(POINT point)
            :this("", "", point, 1)  
        {                  
        }

        public int PowerUP //파워 업 단계 프로퍼티
        {
            get
            {
                return m_nPowerUP;
            }
            set
            {
                m_nPowerUP = value;
            }
        }
        public Robot(string strName, string strColor, POINT point, int nPowerUp)
        {
            m_strName = strName; //이름
            m_Color = strColor; //색깔
            m_point = point; //현재위치
            m_nEnergy = 50; //에너지
            PowerUP = nPowerUp; //파워업 레벨
            Weapon_Mode = WeaponMode.일반무기; //초기 무기발사 모드를 일반무기로 설정
        }
        ~Robot()
        {
        }
        public WeaponMode Weapon_Mode //무기 발사모드 프로퍼티
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
                else if (m_WeaponMode == WeaponMode.둘다발사)
                {
                    Shoot = new ShootDelegate(Shoot_DDGW) + new ShootDelegate(Shoot_DDSW);
                }
            }
        }
        public string Name //로봇이름
        {
            get
            {
                return m_strName;
            }
        }

        public string Color //로봇색깔
        {
            get
            {
                return m_Color;
            }
        }
        public int Energy //에너지
        {
            get
            {
                return m_nEnergy;
            }
            set
            {
                m_nEnergy = value;
            }
        }
        public POINT Point //현재위치
        {
            get
            {
                return m_point;
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
        //에너지 충전
        public void Recharge()
        {
            m_nEnergy = 100;
        }
        //일반무기 따따블 발사
        private int Shoot_DDGW(int n)
        {
            int i;
            for (i = 0; i < n; i++)
                Shoot_GW();
            return i;
        }
        //특수무기 따따블 발사
        private int Shoot_DDSW(int n)
        {
            int i;
            for (i = 0; i < n; i++)
                Shoot_SW();
            return i;
        }

        //로봇이 손상을 입었음
        public virtual void OnDamaged()
        {
            Console.WriteLine("Robot : 로봇이 손상을 입었습니다.\n");
            RobotMessageEventArgs e = new RobotMessageEventArgs("박사님, 수리해주세요\n");
            OnRequestRepairEventHandler(this, e);
            Console.WriteLine("Robot : 김박사에게 요청한 수리를 완료 했습니다.\n");
        }
        //로봇을 진단하고 원격으로 고치는 함수
        public virtual void CheckNRepair()
        {
             Console.WriteLine("Robot : 수리가 완료되었습니다.\n");
        }
    }

    public class Dr_Kim
    {
        // 매개인자 오브젝트와 이벤트 매개 인자 e 사용
        public static void RemoteRepair(object obj, RobotMessageEventArgs e)
        {
            if (e.strMessage == "박사님, 수리해주세요\n")
            {
                Console.WriteLine("김박사 : 로봇으로부터 수리요청을 받았습니다.\n");

                // 이벤트 매개인자를 통해서 현재 로봇의 레퍼런스를 알 수 있다.
                Robot robot = (Robot)obj;

                Console.WriteLine("김박사 : 원격으로 로봇을 진단하고, 수리합니다.\n");

                //로봇을 진단하고 치료함
                robot.CheckNRepair();
            }
        }
    }
}