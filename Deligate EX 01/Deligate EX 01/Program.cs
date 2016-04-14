using System ;
 
public struct POINT
{
        public POINT(int xi,int yi)
        {
                x=xi;
                y=yi;
        }
        public int x;
        public int y;
}
 
public enum WeaponMode
{
        일반무기,
        특수무기,
        둘다발사
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
 
    public class Robot
    {
        private string          m_strName;     //이름
        private string          m_Color;       //색깔
        private int             m_nEnergy;     //에너지
        private General_Weapon  m_generalW;    //일반무기
        private Special_Weapon  m_specialW;    //특수무기
        private POINT           m_point;       //현재위치
        private int             m_nPowerUP;    //몇 대가 합체된 상태인지?
        private WeaponMode      m_WeaponMode;  //현재 무기발사 모드
 
        private static  int nRobotCount=0;   //만들어진 로봇의 갯수
 
        //델리 게이트 선언한 함수 멤버
        public delegate int ShootDelegate(int n);
        //델리게이트 함수 멤버 참조
        public ShootDelegate Shoot;


        // 생성자의 인수를 아래와 같이 해주지 않고  생성자에 인수를 지정 하면 
        // 인수를 0개 사용하는 생성자가 없습니다. 에러 발생
        public Robot()
            : this("", "", new POINT(0, 0), 1)
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
 
        // 생성자
        public Robot(string strName, string strColor, POINT point, int nPowerUp)
        {
                m_strName=strName;             //이름
                m_Color=strColor;                  //색깔
                PowerUP=nPowerUp;              //파워업 레벨
                m_point=point;                       //현재위치
                m_nEnergy=50;                      //에너지
                m_generalW=General_Weapon.돌맹이; //일반무기
                m_specialW=Special_Weapon.유도탄;  //특수무기
                nRobotCount++;                     //만들어진 로봇의 숫자를 증가시킴~
                Weapon_Mode=WeaponMode.일반무기; //초기 무기발사 모드를 일반무기로 설정
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
                else if (m_WeaponMode == WeaponMode.둘다발사)
                {
                    Shoot = new ShootDelegate(Shoot_DDGW) + new ShootDelegate(Shoot_DDSW);
                }
            }
        }
 

        //일반무기 따따블 발사
        private int Shoot_DDGW(int n)
        {
            int i;
 
            for(i = 0;i < n;i++)
                    Shoot_GW();
 
            return i;
        }

        //일반무기 따따블 발사
        private int Shoot_DDSW(int n)
        {
            int i;

            for (i = 0; i < n; i++)
                     Shoot_SW();

            return i;
        }

        //일반무기 발사
        public void Shoot_GW()
        {
            Console.WriteLine(m_generalW.ToString() + "발사");
        }

        //특수무기 발사
        public void Shoot_SW()
        {
            Console.WriteLine(m_specialW.ToString() + "발사");
        }
       
    }

    public class Battle
    {
        public static void Main()
        {
            Robot robotA = new Robot();
            Console.WriteLine("현재 무기 모드는 {0} 입니다.", robotA.Weapon_Mode);
            robotA.Shoot(3);

            robotA.Weapon_Mode = WeaponMode.특수무기;
            Console.WriteLine("현재 무기 모드는 {0} 입니다.", robotA.Weapon_Mode);
            robotA.Shoot(5);

            robotA.Weapon_Mode = WeaponMode.둘다발사;
            Console.WriteLine("현재 무기 모드는 {0}입니다.", robotA.Weapon_Mode);
            robotA.Shoot(2);
            Console.ReadKey();
        }
    }
