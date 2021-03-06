/****************************************************
    文件：Constants.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/10/12 15:16:59
	功能：存储常量类名
*****************************************************/

using UnityEngine;
public enum TxtColor
{
    Red,
    Green,
    Blue,
    Yellow,

}
public enum DamageType
{
    None,
    TargetSkill = 1,
    AreaSkill = 2,
    SupportSkill=3
}
public enum EntityType
{
    None,
    Player,
    Monster
}
public enum MonsterType
{
    None,
    Normal,
    Elite,
    Boss
}
public class Constants 
{
    //字体颜色
    public const string ColorRed = "<color=#FF0000FF>";
    public const string ColorGreen = "<color=#00FF00FF>";
    public const string ColorBlue = "<color=#00B4FFFF>";
    public const string ColorYellow = "<color=#FFFF00FF>";
    public const string ColorEnd = "</color>";

    public static string Color(string str,TxtColor color)
    {
        string result = "";
        switch (color)
        {
            case TxtColor.Red:
                result = ColorRed + str + ColorEnd;
                break;
            case TxtColor.Green:
                result = ColorGreen + str + ColorEnd;
                break;
            case TxtColor.Blue:
                result = ColorBlue + str + ColorEnd;
                break;
            case TxtColor.Yellow:
                result = ColorYellow + str + ColorEnd;
                break;
            default:
                break;
        }
        return result;
    }
    //AutoGuideNPC
    public const int NPCWiseMan = 0;
    public const int NPCGeneral = 1;
    public const int NPCArtisan = 2;
    public const int NPCTrader = 3;

    //场景类名、ID
    public const string SceneLogin = "SceneLogin";
    //public const string SceneMainCity = "SceneMainCity";
    public const int MainCityMapID = 1001;
    public const int MainCityMap2ID = 1002;
    public const int Duplicate = 1003;
    //音效名
    public const string BGLogin = "bgLogin";
    public const string BGMainCity = "BG";
    public const string BGFuben = "BGFuben";
    public const string BGBoss = "BGBoss";
    public const string uiClick = "uiClickBtn";
    public const string uiExtenBtn = "uiExtenBtn";
    public const string uiOpenPage = "uiOpenPage";
    public const string eatApple = "eatApple";
    public const string EarnCoin = "EarnCoin";
    public const string FBLose = "fblose";
    public const string FBItemEnter = "fbitem";
    public const string FBWin = "fbwin";
    public const string strengthSuccess = "fbitem";
    public const string archerWound = "wound";
    public const string archerSpecialWound = "player_Special_wound";
    public const string skill1 = "skill1";
    public const string skill2 = "skill2";
    public const string skill3 = "skill3";
    public const string skill4 = "skill4";
    public const string skill5 = "skill5";
    public const string skill6 = "skill6";
    public const string skill7 = "skill7";
    public const string skill8 = "skill8";
    //登陆音效
    public const string uiLogin = "uiLoginBtn";

    //屏幕标准宽高
    public const int screenStandardWidth = 1334;
    public const int screenStandardHeight = 750;

    //摇杆点中心小圆点最多移动的距离
    public const int screenOperationDistant = 90;

    //移动速度
    public const float playerMoveSpeed = 8;
    public const float monsterMoveSpeed = 3;
    public const float BossCrazyMoveSpeed = 6;

    //动画混合树平滑加快速率
    public const float accelerateSpeed = 5;
    //血量渐变速率
    public const float accelerateHPSpeed = 0.3f;

    //Action触发参数
    public const int ActionDefault = -1;
    public const int ActionDie = 100;
    public const int ActionWound = 101;

    public const int DieAniLength = 5;
    //混合树动画设定值
    public const float blendIdle = 0;
    public const float blendRun = 1;
    public const float blendWalk = 0.5f;

    //json文件目录
    public const string jsonPath = "/Json/playerData.json";
    public const string jsonInitPath = "/Json/playerInitData.json";

    //技能特效名字
    public const string skill3Name = "skill3_ground";
    public const string skill4Name = "skill4_ground";
    public const string boss_Unbreakable = "boss_Unbreakable";
    public const string boss_Crazy = "boss_crazy";

    //范围技能最大施法距离
    public const float skill3MaxArea = 12;
    public const float skill4MaxArea = 15;
    public const float skill7MaxArea = 20;

    //范围技能图标显示技能范围
    public static Vector3 skill3Scale = new Vector3(3f,3f,1);
    public static Vector3 skill4Scale = new Vector3(4.5f,4.5f,1);
    public static Vector3 skill7Scale = new Vector3(1,1,1);
    //角色技能id
    public const int id_PlayerSkill_Heal = 105;
    public const int id_PlayerSkill_Buff = 106;
    public const int id_PlayerSkill_Blink = 107;
    public const int id_PlayerSkill_Dodge = 108;

    //Boss技能ID
    public const int id_BossSkill_bladeStorm = 304;
    public const int id_BossSkill_sifangzhen = 305;

    public static Vector3 sifangzhanPos = new Vector3(110,42.93f,73.39f);
}