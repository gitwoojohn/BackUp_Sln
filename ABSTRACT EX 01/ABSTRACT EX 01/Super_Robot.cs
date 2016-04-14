using System;
namespace RobotWorld
{
    public class Super_Robot : Robot
    {
        // 현재 비행기로 변신한 상태인지, 로봇 상태인지
        private bool m_bPlane;
        private static int nRobotCount = 0; //만들어진 로봇의 갯수
        private SuperRobot_General_Weapon m_generalW; //일반무기
        private SuperRobot_Special_Weapon m_specialW; //특수무기
        // 현재 슈퍼로봇의 속도
        private int m_nSpeed;

        public new int PowerUP //파워 업 단계 프로퍼티
        {
            get
            {
                return base.PowerUP;
            }

            set
            {
                base.PowerUP = value;
                //파워 업 단계별로 무기가 강화됨
                switch (base.PowerUP)
                {
                    case 1:
                        m_generalW = SuperRobot_General_Weapon.돌맹이;
                        m_specialW = SuperRobot_Special_Weapon.대포;
                        break;
                    case 2:
                        m_generalW = SuperRobot_General_Weapon.바위;
                        m_specialW = SuperRobot_Special_Weapon.유도탄;
                        break;
                    case 3:
                        m_generalW = SuperRobot_General_Weapon.총알;
                        m_specialW = SuperRobot_Special_Weapon.레이져;
                        break;
                    default:
                        base.PowerUP = 1;
                        m_generalW = SuperRobot_General_Weapon.돌맹이;
                        m_specialW = SuperRobot_Special_Weapon.대포;
                        break;
                }
            }
        }

        public SuperRobot_General_Weapon GW //일반무기 프로퍼티
        {
            get
            {
                return m_generalW;
            }
            set
            {
                m_generalW = value;
                Energy--; //일반무기 변경시 에너지 1감소
            }
        }

        public SuperRobot_Special_Weapon SW //특수무기 프로퍼티
        {
            get
            {
                return m_specialW;
            }
            set
            {
                m_specialW = value;
                Energy -= 2; //특수무기 변경시 에너지 2감소
            }
        }

        public Super_Robot(string strName)
            : base(strName)
        {
            // 초기를 로봇으로 설정
            m_bPlane = false;
            // 초기 로봇의 속도
            m_nSpeed = 100;
            // 초기 에너지를 100으로
            base.Recharge();
            m_generalW = SuperRobot_General_Weapon.돌맹이; //일반무기
            m_specialW = SuperRobot_Special_Weapon.유도탄; //특수무기
            nRobotCount++;
        }

        ~Super_Robot()
        {
            Console.WriteLine("슈퍼 로봇 클래스를 소멸 합니다.");
        }

        public void Change2Plane()
        {
            // 현재 상태가 비행기이면 리턴
            if (m_bPlane)
                return;
            m_bPlane = true;
            //속도를 높임
            m_nSpeed = 200;
            Console.WriteLine(this.Name + "비행기로 변신! " + "현재 속도" + m_nSpeed);
        }

        //일반무기 발사
        public override void Shoot_GW()
        {
            Console.WriteLine(m_generalW.ToString() + "발사");
        }

        //특수무기 발사
        public override void Shoot_SW()
        {
            Console.WriteLine(m_specialW.ToString() + "발사");
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

        public override int PrintRobotCount()
        {
            Console.WriteLine("현재 만들어진 로봇의 갯수는 ? " + nRobotCount + "개");
            return nRobotCount;
        }
    }
}