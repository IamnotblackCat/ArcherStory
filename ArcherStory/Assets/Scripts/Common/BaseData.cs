
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
}
