
using System.Collections.Generic;
using UnityEngine;

public class StrengthCfg : BaseData<StrengthCfg>
{
    public int pos;
    public int startlv;
    public int addhp;
    public int addhurt;
    public int adddef;
    public int minlv;
    public int coin;
    public int crystal;
}
public class AutoGuideCfg : BaseData<AutoGuideCfg>
{
    public int npcID;//触发任务的npcID
    public string dilogArr;
    public int actID;
    public int coin;
    public int exp;
}
public class BaseData<T>
{
    public int ID;
}
public class MapConfig : BaseData<MapConfig>
{
    public string mapName;
    public string sceneName;
    public Vector3 mainCamPos;
    public Vector3 mainCamRote;
    public Vector3 playerBornPos;
    public Vector3 playerBornRote;
    public List<MonsterData> monsterList;
}
public class SkillCfg : BaseData<SkillCfg>
{
    public string skillName;
    public float skillFXTime;
    public float animationTime;
    public float delayFXTime;
    public float delayCloseFXTime;
    public float skillCDTime;   
    public int aniAction;
    public string fx;
    public string targetFX;
    public DamageType dmgType;//技能类型，包括指定技能，范围技能，辅助技能
    public int skillMove;
    public List<int> skillActionList;
    public List<int> skillDamageList;
}
public class SkillActionCfg : BaseData<SkillActionCfg>
{
    public int delayTime;
    public float radius;//伤害范围
    public float angle;//伤害角度
}
public class SkillMoveCfg : BaseData<SkillMoveCfg>
{
    public int moveTime;
    public float moveDis;
}
public class MonsterCfg : BaseData<MonsterCfg>
{
    public string mName;
    public string resPath;
    public BattleProps bps;
}
//这不是一个单独的配置文件，是monsterCfg对应的数据,数据存在map配置里面
public class MonsterData : BaseData<MonsterData>
{
    public int mWave;//第几批怪物
    public int mIndex;//第几个
    public MonsterCfg mCfg;
    public Vector3 mBornPos;
    public Vector3 mBornRote;
    public int mLevel;
}

public class BattleProps
{
    public int hp;
    public int attackValue;
    public int defend;
    public int critical;
}

