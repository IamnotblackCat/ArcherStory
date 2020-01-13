/****************************************************
    文件：PathDefine.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/10/15 18:8:4
	功能：路径常量
*****************************************************/

using UnityEngine;

public class PathDefine 
{
    #region Config
    public const string RDName = "ResCfgs/rdName";
    public const string MapCfg = "XML/map_v1";
    public const string GuideCfg = "ResCfgs/guideCfg";
    public const string StrengthCfg = "ResCfgs/strengthCfg";
    public const string SkillCfg = "XML/skill";
    public const string SkillMoveCfg = "XML/skillmove";
    public const string SkillActionCfg = "XML/skillAction";
    public const string MonsterCfg = "XML/monster";
    
    #endregion

    #region 强化
    public const string ItemArrorBG = "ResImages/btnstrong";
    public const string ItemPlatBG = "ResImages/charbg3";

    public const string ItemHead = "ResImages/toukui";
    public const string ItemBody = "ResImages/body";
    public const string ItemWaist = "ResImages/yaobu";
    public const string ItemHand = "ResImages/hand";
    public const string ItemLeg = "ResImages/leg";
    public const string ItemFoot = "ResImages/foot";

    public const string StarIcon1 = "ResImages/star1";//空心
    public const string StarIcon2 = "ResImages/star2";//实心
    #endregion

    #region AutoGuide
    public const string TaskHead = "ResImages/task";
    public const string WiseHead = "ResImages/wiseman";
    public const string GeneralHead = "ResImages/general";
    public const string ArtisanHead = "ResImages/artisan";
    public const string TraderHead = "ResImages/trader";

    public const string SelfIcon = "ResImages/assassin";
    public const string GuideIcon = "ResImages/npcguide";
    public const string WiseIcon = "ResImages/npc0";
    public const string GeneralIcon = "ResImages/npc1";
    public const string ArtisanIcon = "ResImages/npc2";
    public const string TraderIcon = "ResImages/npc3";
    #endregion


    #region Player
    public const string ArcherPrefab = "Character/Archer/Female/Archer_Female_03/Archer_Female_00";
    public const string ArcherBattle = "Character/Archer/Female/Archer_Female_03/ArcherBattle";

    public const string HPDynamic = "PrefabUI/HPDynamic";
    #endregion

    //技能prefab路径
    public const string skill3Path = "PrefabSkillFX/skill3_ground";
    public const string skill4Path = "PrefabSkillFX/skill4_groundNew";
    public const string skillAreaIcon = "PrefabSkillFX/skillAreaIcon";
    public const string boss_SkillLoop = "PrefabSkillFX/boss_skill2Area";
}