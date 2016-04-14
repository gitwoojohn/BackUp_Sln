using System;
namespace RobotWorld
{
    public struct POINT
    {
        public POINT(int xi, int yi)
        {
            x = xi;
            y = yi;
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
    public enum SuperRobot_General_Weapon
    {
        돌맹이,
        바위,
        총알
    }
    public enum SuperRobot_Special_Weapon
    {
        대포,
        유도탄,
        레이져
    }
}