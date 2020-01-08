/****************************************************
	文件：ResSvc.cs
	作者：Echo
	邮箱: 350383921@qq.com
	日期：2019/11/15 17:59   	
	功能：资源服务
*****************************************************/

using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResSvc : MonoBehaviour 
{
    public static ResSvc instance = null;
    public Player player;

    public void InitSvc()
    {
        instance = this;
        //InitRDNameCfg(PathDefine.RDName);
        InitMonsterCfg(PathDefine.MonsterCfg);
        InitMapCfg(PathDefine.MapCfg);
        InitSkillCfg(PathDefine.SkillCfg);
        InitSkillMoveCfg(PathDefine.SkillMoveCfg);
        InitSkillAction(PathDefine.SkillActionCfg);
        //读取玩家数据
        GameRoot.instance.ReadPlayerData();
        player.Init();
        //InitGuideCfg(PathDefine.GuideCfg);
       // InitStrengthCfg(PathDefine.StrengthCfg);
        //PECommon.Log("启动资源加载...");
    }
    private Action prgCB = null;//这个委托为了能在update里面实时更新进度值
    public void AsyncLoadScene(string sceneName,Action loaded)//参数委托为了复用这个函数
    {//loading界面是复用的代码

        //GameRoot.instance.loadingWnd.setwind;
        GameRoot.instance.loadingWnd.SetWndState(true);
        //Debug.Log("出来了"+GameRoot.instance.loadingWnd.gameObject.activeSelf);
        AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(sceneName);
        prgCB = () =>
        {
            float val = sceneAsync.progress;
            GameRoot.instance.loadingWnd.SetProgress(val);
            if (val==1)
            {
                if (loaded!=null)
                {
                    loaded();
                }
                prgCB = null;
                sceneAsync = null;
                GameRoot.instance.loadingWnd.SetWndState(false);
            }
        };
        
    }
    private void Update()
    {
        if (prgCB!=null)
        {
            prgCB();
        }
    }
    private Dictionary<string, AudioClip> audioDic = new Dictionary<string, AudioClip>();
    public AudioClip LoadAudio(string path,bool cache=false)
    {
        AudioClip au = null;
        if (!audioDic.TryGetValue(path,out au))
        {
            au = Resources.Load<AudioClip>(path);
            
            if (cache)//如果需要缓存
            {
                audioDic.Add(path, au);
            }
            //PECommon.Log(path+"au: "+au.name);
        }
        return au;
    }

    private Dictionary<string, GameObject> goDic = new Dictionary<string, GameObject>();
    public GameObject LoadPrefab(string path, bool cache = false)
    {
        GameObject prefab = null;
        //如果字典里面没有
        if (!goDic.TryGetValue(path,out prefab))
        {
            prefab = Resources.Load<GameObject>(path);
            if (cache)
            {
                goDic.Add(path,prefab);
            }
        }
        GameObject go = null;
        if (prefab!=null)
        {
            go = Instantiate(prefab);
        }
        //Debug.Log(go.name+"---"+prefab.name+"--"+path.ToString());
        return go;
    }

    private Dictionary<string, Sprite> spriteDic = new Dictionary<string, Sprite>();
    public Sprite LoadSprite(string path,bool cache =false)
    {
        Sprite sp = null;
        if (!spriteDic.TryGetValue(path,out sp))
        {
            sp = Resources.Load<Sprite>(path);
            if (cache)
            {
                spriteDic.Add(path,sp);
            }
        }
        return sp;
    }
    #region InitCfgs

    #region 地图配置
    private Dictionary<int, MapConfig> mapCfgDataDic = new Dictionary<int, MapConfig>();
    private void InitMapCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            //PECommon.Log("指定文件不存在，路径：" + path, LogType.Error);
            Debug.Log("指定文件不存在，路径：" + path);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            //选中子节点集合
            XmlNodeList nodList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodList.Count; i++)
            {
                XmlElement ele = nodList[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {//不包含ID的节点，直接跳到下一个遍历，安全校验
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                MapConfig mapCfg = new MapConfig
                {
                    ID = ID,
                    monsterList = new List<MonsterData>(),
                };

                foreach (XmlElement element in nodList[i].ChildNodes)
                {
                    switch (element.Name)
                    {
                        case "mapName":
                            mapCfg.mapName = element.InnerText;
                            break;
                        case "sceneName":
                            mapCfg.sceneName = element.InnerText;
                            break;
                        case "mainCamPos":
                            {//加括号是形成块，让这个变量仅在块里面使用
                                string[] valArray = element.InnerText.Split(',');
                                //Debug.Log(valArray[0]+","+valArray[1]+","+valArray[2]);
                                mapCfg.mainCamPos = new Vector3(float.Parse(valArray[0]), float.Parse(valArray[1]), float.Parse(valArray[2]));
                            }
                            break;
                        case "mainCamRote":
                            {
                                string[] valArray = element.InnerText.Split(',');
                                mapCfg.mainCamRote = new Vector3(float.Parse(valArray[0]), float.Parse(valArray[1]), float.Parse(valArray[2]));
                            }
                            break;
                        case "playerBornPos":
                            {
                                string[] valArray = element.InnerText.Split(',');
                                //Debug.Log(valArray[0]+","+valArray[1]+","+valArray[2]);
                                mapCfg.playerBornPos = new Vector3(float.Parse(valArray[0]), float.Parse(valArray[1]), float.Parse(valArray[2]));
                            }
                            break;
                        case "playerBornRote":
                            {
                                string[] valArray = element.InnerText.Split(',');
                                mapCfg.playerBornRote = new Vector3(float.Parse(valArray[0]), float.Parse(valArray[1]), float.Parse(valArray[2]));
                            }
                            break;
                        case "monsterList":
                            {
                                string[] valArray = element.InnerText.Split('#');
                                for (int waveIndex = 0; waveIndex < valArray.Length; waveIndex++)
                                {
                                    if (waveIndex==0)//因为第一个#最前面是没有数据的
                                    {
                                        continue;
                                    }
                                    string[] tempArr = valArray[waveIndex].Split('|');
                                    for (int j = 0; j < tempArr.Length; j++)
                                    {
                                        if (j==0)
                                        {
                                            continue;
                                        }
                                        string[] arr = tempArr[j].Split(',');
                                        MonsterData md = new MonsterData//读取怪物的生成点位置
                                        {
                                            ID = int.Parse(arr[0]),
                                            mWave = waveIndex,
                                            mIndex = j,
                                            mCfg = GetMonsterCfgData(int.Parse(arr[0])),
                                            mBornPos = new Vector3(float.Parse(arr[1]), float.Parse(arr[2]), float.Parse(arr[3])),
                                            mBornRote = new Vector3(0, float.Parse(arr[4]), 0),
                                            mLevel = int.Parse(arr[5]),
                                        };
                                        mapCfg.monsterList.Add(md);
                                    }
                                }

                            }
                            break;
                    }
                }
                mapCfgDataDic.Add(ID,mapCfg);
                //Debug.Log("ID:"+ID+"  mapCfg:"+mapCfg.ToString());
            }
        }
    }
    public MapConfig GetMapCfgData(int id)
    {
        MapConfig data;
        if (mapCfgDataDic.TryGetValue(id, out data))
        {
            //Debug.Log(data);
            return data;
        }
        return null;
    }
    #endregion 地图

    #region 自动任务
    private Dictionary<int, AutoGuideCfg> autoGuideDic = new Dictionary<int, AutoGuideCfg>();
    private void InitGuideCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("指定文件不存在，路径：" + path, LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            //选中子节点集合
            XmlNodeList nodList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodList.Count; i++)
            {
                XmlElement ele = nodList[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {//不包含ID的节点，直接跳到下一个遍历，安全校验
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                AutoGuideCfg guideCfg = new AutoGuideCfg();
                guideCfg.ID = ID;

                foreach (XmlElement element in nodList[i].ChildNodes)
                {
                    switch (element.Name)
                    {
                        case "npcID":
                            guideCfg.npcID =int.Parse(element.InnerText);
                            break;
                        case "dilogArr":
                            {
                                //string strArray = element.InnerText;
                                guideCfg.dilogArr = element.InnerText;
                            }
                            break;
                        case "actID":
                            guideCfg.actID = int.Parse(element.InnerText);
                            break;
                        case "coin":
                            guideCfg.coin = int.Parse(element.InnerText);
                            break;
                        case "exp":
                            guideCfg.exp = int.Parse(element.InnerText);
                            break;
                        default:
                            break;
                    }
                }
                autoGuideDic.Add(ID, guideCfg);
                //Debug.Log("ID:"+ID+"  mapCfg:"+mapCfg.ToString());
            }
        }
    }
    public AutoGuideCfg GetGuideCfgData(int id)
    {
        AutoGuideCfg agc = null;

        //Debug.Log(id);
        if (autoGuideDic.TryGetValue(id, out agc))
        {
            //Debug.Log(data);
            return agc;
        }
        return null;
    }
    #endregion
    #region 技能配置
    private Dictionary<int, SkillCfg> skillCfgDic = new Dictionary<int, SkillCfg>();
    private void InitSkillCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("指定文件不存在，路径：" + path, LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            //选中子节点集合
            XmlNodeList nodList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodList.Count; i++)
            {
                XmlElement ele = nodList[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {//不包含ID的节点，直接跳到下一个遍历，安全校验
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                SkillCfg skillCfgData = new SkillCfg
                {
                    ID = ID,
                    skillActionList = new List<int>(),
                    skillDamageList = new List<int>(),
                };

                foreach (XmlElement element in nodList[i].ChildNodes)
                {
                    switch (element.Name)
                    {
                        case "skillName":
                            {
                                skillCfgData.skillName = element.InnerText;
                            }
                            break;
                        case "skillTime":
                            skillCfgData.skillFXTime = float.Parse(element.InnerText);
                            break;
                        case "cdTime":
                            skillCfgData.skillCDTime = float.Parse(element.InnerText);
                            break;
                        case "animTime":
                            skillCfgData.animationTime = float.Parse(element.InnerText);
                            break;
                        case "delayFXTime":
                            skillCfgData.delayFXTime = float.Parse(element.InnerText);
                            break;
                        case "delayCloseFXTime":
                            skillCfgData.delayCloseFXTime = float.Parse(element.InnerText);
                            break;
                        case "aniAction":
                            skillCfgData.aniAction = int.Parse(element.InnerText);
                            break;
                        case "isBreak":
                            skillCfgData.cantStop = element.InnerText.Equals("1");
                            break;
                        case "fx":
                            skillCfgData.fx = element.InnerText;
                            break;
                        case "targetFX":
                            skillCfgData.targetFX = element.InnerText;
                            break;
                        case "dmgType":
                            if (element.InnerText.Equals("1"))
                            {
                                skillCfgData.dmgType = DamageType.TargetSkill;
                            }
                            else if (element.InnerText.Equals("2"))
                            {
                                skillCfgData.dmgType = DamageType.AreaSkill;
                            }
                            else if (element.InnerText.Equals("3"))
                            {
                                skillCfgData.dmgType = DamageType.SupportSkill;
                            }
                            else
                            {
                                Debug.Log("伤害类型错误");
                            }
                            break;
                        case "skillMove":
                            skillCfgData.skillMove = int.Parse(element.InnerText);
                            break;
                        case "skillActionList":
                            {
                                string[] valArray = element.InnerText.Split('|');
                                for (int j = 0; j < valArray.Length; j++)
                                {
                                    if (valArray[j] != "")
                                    {
                                        skillCfgData.skillActionList.Add(int.Parse(valArray[j]));
                                    }
                                }
                            }
                            break;
                        case "skillDamageList":
                            {
                                string[] valArray = element.InnerText.Split('|');
                                for (int j = 0; j < valArray.Length; j++)
                                {
                                    if (valArray[j] != "")
                                    {
                                        skillCfgData.skillDamageList.Add(int.Parse(valArray[j]));
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
                skillCfgDic.Add(ID, skillCfgData);
                //Debug.Log("ID:"+ID+"  mapCfg:"+mapCfg.ToString());
            }
        }
    }
    public SkillCfg GetSkillCfgData(int id)
    {
        SkillCfg agc = null;

        //Debug.Log(id);
        if (skillCfgDic.TryGetValue(id, out agc))
        {
            //Debug.Log(data);
            return agc;
        }
        return null;
    }
    #endregion
    #region 技能伤害配置
    private Dictionary<int, SkillActionCfg> skillActionDic = new Dictionary<int, SkillActionCfg>();
    private void InitSkillAction(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            Debug.Log("指定文件不存在，路径：" + path);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            //选中子节点集合
            XmlNodeList nodList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodList.Count; i++)
            {
                XmlElement ele = nodList[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {//不包含ID的节点，直接跳到下一个遍历，安全校验
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                SkillActionCfg skillActionData = new SkillActionCfg
                {
                    ID = ID,
                };

                foreach (XmlElement element in nodList[i].ChildNodes)
                {
                    switch (element.Name)
                    {
                        case "delayTime":
                            skillActionData.delayTime = int.Parse(element.InnerText);
                            break;
                        case "radius":
                            skillActionData.radius = float.Parse(element.InnerText);
                            break;
                        case "angle":
                            skillActionData.angle = float.Parse(element.InnerText);
                            break;
                    }
                }
                skillActionDic.Add(ID, skillActionData);
                //Debug.Log("ID:"+ID+"  mapCfg:"+mapCfg.ToString());
            }
        }
    }
    public SkillActionCfg GetSkillActionData(int id)
    {
        SkillActionCfg agc = null;

        //Debug.Log(id);
        if (skillActionDic.TryGetValue(id, out agc))
        {
            //Debug.Log(data);
            return agc;
        }
        return null;
    }
    #endregion
    #region 技能移动配置
    private Dictionary<int, SkillMoveCfg> skillMoveCfgDic = new Dictionary<int, SkillMoveCfg>();
    private void InitSkillMoveCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("指定文件不存在，路径：" + path, LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            //选中子节点集合
            XmlNodeList nodList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodList.Count; i++)
            {
                XmlElement ele = nodList[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {//不包含ID的节点，直接跳到下一个遍历，安全校验
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                SkillMoveCfg SkillMoveCfgData = new SkillMoveCfg();
                SkillMoveCfgData.ID = ID;

                foreach (XmlElement element in nodList[i].ChildNodes)
                {
                    switch (element.Name)
                    {
                        case "moveTime":
                            SkillMoveCfgData.moveTime = int.Parse(element.InnerText);
                            break;
                        case "moveDis":
                            SkillMoveCfgData.moveDis = float.Parse(element.InnerText);
                            break;
                    }
                }
                skillMoveCfgDic.Add(ID, SkillMoveCfgData);
            }
        }
    }
    public SkillMoveCfg GetSkillMoveCfgData(int id)
    {
        SkillMoveCfg agc = null;
        if (skillMoveCfgDic.TryGetValue(id, out agc))
        {
            return agc;
        }
        return null;
    }
    #endregion
    #region 怪物配置
    private Dictionary<int, MonsterCfg> monsterCfgDic = new Dictionary<int, MonsterCfg>();
    private void InitMonsterCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            Debug.Log("指定文件不存在，路径：" + path);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            //选中子节点集合
            XmlNodeList nodList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodList.Count; i++)
            {
                XmlElement ele = nodList[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {//不包含ID的节点，直接跳到下一个遍历，安全校验
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                MonsterCfg monsterCfg = new MonsterCfg
                {
                    ID = ID,
                    bps = new BattleProps(),
                };

                foreach (XmlElement element in nodList[i].ChildNodes)
                {
                    switch (element.Name)
                    {
                        case "mName":
                            monsterCfg.mName = element.InnerText;
                            break;
                        case "mType":
                            if (element.InnerText.Equals("1"))
                            {
                                monsterCfg.mType =MonsterType.Normal ;
                            }
                            else
                            {
                                monsterCfg.mType = MonsterType.Boss;
                            }
                            break;
                        case "resPath":
                            monsterCfg.resPath = element.InnerText;
                            break;
                        case "skillID":
                            monsterCfg.skillID = int.Parse(element.InnerText);
                            break;
                        case "atkDis":
                            monsterCfg.atkDis = float.Parse(element.InnerText);
                            break;
                        case "hp":
                            monsterCfg.bps.hp = int.Parse(element.InnerText);
                            break;
                        case "attackValue":
                            monsterCfg.bps.attackValue = int.Parse(element.InnerText);
                            break;
                        case "defend":
                            monsterCfg.bps.defend = int.Parse(element.InnerText);
                            break;
                        case "critical":
                            monsterCfg.bps.critical = int.Parse(element.InnerText);
                            break;
                    }
                }
                monsterCfgDic.Add(ID, monsterCfg);
            }
        }
    }
    public MonsterCfg GetMonsterCfgData(int id)
    {
        MonsterCfg agc = null;
        if (monsterCfgDic.TryGetValue(id, out agc))
        {
            return agc;
        }
        return null;
    }
    #endregion
    #region 强化配置
    private Dictionary<int, Dictionary<int, StrengthCfg>> strengthDic = new Dictionary<int, Dictionary<int, StrengthCfg>>();
    private void InitStrengthCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("指定文件不存在，路径：" + path, LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            //选中子节点集合
            XmlNodeList nodList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodList.Count; i++)
            {
                XmlElement ele = nodList[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {//不包含ID的节点，直接跳到下一个遍历，安全校验
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                StrengthCfg sd = new StrengthCfg {ID=ID };

                foreach (XmlElement element in nodList[i].ChildNodes)
                {
                    switch (element.Name)
                    {
                        case "pos":
                            sd.pos = int.Parse(element.InnerText);
                            break;
                        case "starlv":
                            sd.startlv = int.Parse(element.InnerText);
                            break;
                        case "coin":
                            sd.coin = int.Parse(element.InnerText);
                            break;
                        case "crystal":
                            sd.crystal = int.Parse(element.InnerText);
                            break;
                        case "addhp":
                            sd.addhp = int.Parse(element.InnerText);
                            break;
                        case "addhurt":
                            sd.addhurt = int.Parse(element.InnerText);
                            break;
                        case "adddef":
                            sd.adddef = int.Parse(element.InnerText);
                            break;
                        case "minlv":
                            sd.minlv = int.Parse(element.InnerText);
                            break;
                        default:
                            break;
                    }
                }
                Dictionary<int, StrengthCfg> dic = null;
                if (strengthDic.TryGetValue(sd.pos, out dic))
                {
                    dic.Add(sd.startlv, sd);
                }
                else
                {
                    dic = new Dictionary<int, StrengthCfg>();
                    dic.Add(sd.startlv, sd);

                    strengthDic.Add(sd.pos, dic);
                }
                //strengthDic.Add(ID, strengthCfg);
                //Debug.Log("ID:"+ID+"  mapCfg:"+mapCfg.ToString());
            }
        }
    }
    public StrengthCfg GetStrengthCfgData(int pos,int startlv)
    {
        StrengthCfg sc = null;
        Dictionary<int, StrengthCfg> dic = null;
        //Debug.Log(id);
        if (strengthDic.TryGetValue(pos,out dic))
        {
            //Debug.Log(data);
            if (dic.ContainsKey(startlv))
            {
                sc = dic[startlv];
            }
        }
        return sc;
    }

    public int GetPropAddValPreLv(int pos,int starlv,int type)
    {
        Dictionary<int, StrengthCfg> posDic = null;
        int val = 0;
        if (strengthDic.TryGetValue(pos,out posDic))
        {
            for (int i = 0; i < starlv; i++)
            {
                StrengthCfg sc;
                if (posDic.TryGetValue(i,out sc))
                {
                    switch (type)
                    {
                        case 1://hp
                            val += sc.addhp;
                            break;
                        case 2://伤害
                            val += sc.addhurt;
                            break;
                        case 3://防御
                            val += sc.adddef;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        return val; 
    }
    #endregion
    #endregion
    public void InitCfgData()
    {
        skillCfgDic.Clear();
        skillMoveCfgDic.Clear();
        InitSkillCfg(PathDefine.SkillCfg);
        InitSkillMoveCfg(PathDefine.SkillMoveCfg);
    }
}