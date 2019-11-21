/****************************************************
    文件：MainCitySys.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/10/25 9:28:35
	功能：主城业务系统
*****************************************************/
using PEProtocol;
using System;
using UnityEngine;
using UnityEngine.AI;

public class MainCitySys : SystemRoot 
{
    //单例
    public static MainCitySys Instance = null;

    public MainCityWnd mainCityWnd;
    public InfoWnd infoWnd;
    //public GuideWnd guideWnd;
    public StrengthWnd strengthWnd;

    private Transform charactorCamTrans;
    private PlayerController playerCtrl;
    private AutoGuideCfg currentTask;
    private Transform[] npcTransPos;
    private NavMeshAgent nav;
    public override void InitSys()
    {
        base.InitSys();
        Instance = this;
        //PECommon.Log("Main City Init...");
    }

    public void EnterMainCity()
    {
        MapConfig mapData = resSvc.GetMapCfgData(Constants.MainCityMap2ID);
        //Debug.Log(mapData.sceneName);
        GameRoot.instance.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        resSvc.AsyncLoadScene(mapData.sceneName,()=>
        {
            //PECommon.Log("Enter MainCity already");
            //加载主角
            LoadPlayer(mapData);
            //打开UI
            mainCityWnd.SetWndState();
            //背景音乐
            audioSvc.PlayBGMusic(Constants.BGMainCity);
            //GameObject go= GameObject.FindGameObjectWithTag("MapRoot");
            //npcTransPos = go.GetComponent<MainCityMap>().NpcPosTrans;
            //设置人物相机
            if (charactorCamTrans!=null)
            {
                charactorCamTrans.gameObject.SetActive(false);
            }
        });
    }
    private void LoadPlayer(MapConfig mapData)
    {
        GameObject player = resSvc.LoadPrefab(PathDefine.ArcherPrefab, true);
        //Debug.Log(player.transform.position);
        player.transform.position = mapData.playerBornPos;
        player.transform.localEulerAngles = mapData.playerBornRote;
        player.transform.localScale = new Vector3(1.5f,1.5f,1.5f);
        playerCtrl= player.GetComponent<PlayerController>();
        playerCtrl.Init();
        nav = player.GetComponent<NavMeshAgent>();

        //相机初始化
        Camera.main.transform.position = mapData.mainCamPos;
        //Debug.Log("CameraPos"+Camera.main.transform.position+"--mapDataCamPos"+mapData.mainCamPos);
        Camera.main.transform.eulerAngles = mapData.mainCamRote;
    }
    //当检测到摇杆插件信息的时候
    //public void SetMoveDir(Vector2 dir)
    //{
    //    StopNavTask();
    //    if (dir==Vector2.zero)
    //    {
    //        playerCtrl.SetBlend(Constants.blendIdle);
    //    }
    //    else
    //    {
    //        playerCtrl.SetBlend(Constants.blendWalk);
    //    }
    //    playerCtrl.Dir = dir;
    //}
    #region 玩家信息相关
    public void OpenInfoWnd()
    {
        if (charactorCamTrans == null)
        {
            //Debug.Log(GameObject.FindGameObjectWithTag("CharactorCam"));
            charactorCamTrans = GameObject.FindGameObjectWithTag("CharactorCam").transform;
        }
        //设置相机的相对位置
        charactorCamTrans.localPosition = playerCtrl.transform.position + playerCtrl.transform.forward * 3f + new Vector3(0, 1.2f, 0);
        charactorCamTrans.localEulerAngles = new Vector3(0, 180 + playerCtrl.transform.localEulerAngles.y, 0);
        charactorCamTrans.localScale = Vector3.one;
        charactorCamTrans.gameObject.SetActive(true);
        infoWnd.SetWndState();

    }
    public void CloseInfoWndCamera()
    {
        charactorCamTrans.gameObject.SetActive(false);
    }
    private float startPos = 0;
    public void SetStartRotate()
    {
        startPos = playerCtrl.transform.localEulerAngles.y;
    }
    public void SetPlayerRotate(float rotate)
    {
        playerCtrl.transform.localEulerAngles = new Vector3(0, startPos + rotate, 0);
        //改变摄像机的朝向，不影响角色，但是传入的值存在问题，因为相对startPos，即使往回滑，还是负数
        //charactorCamTrans.RotateAround(playerCtrl.transform.position,Vector3.up,rotate*0.3f*Time.deltaTime);
    } 
    #endregion
    //#region 任务引导代码
    //private bool isNavGuide = false;
    //public void RunTask(AutoGuideCfg guideCfg)
    //{
    //    if (guideCfg != null)
    //    {
    //        currentTask = guideCfg;
    //    }
    //    //解析任务数据
    //    nav.enabled = true;//进来就先设置，避免角色在目的地无法调用isStopped方法
    //    if (currentTask.npcID != -1)
    //    {//这个npcID是表格里面定义好的一一对应的NPCID
    //        float distance = Vector3.Distance(playerCtrl.transform.position,
    //            npcTransPos[currentTask.npcID].position);
    //        if (distance < 0.5f)
    //        {
    //            StopNavTask();
    //        }
    //        else
    //        {
    //            nav.enabled = true;
    //            nav.speed = Constants.playerMoveSpeed;
    //            nav.SetDestination(npcTransPos[currentTask.npcID].position);
    //            //导航不会自动播放动画，也不会相机跟随
    //            playerCtrl.SetBlend(Constants.blendWalk);
    //            isNavGuide = true;
    //        }
    //    }
    //    else
    //    {
    //        OpenGuideWnd();
    //    }
    //}
    //private void Update()
    //{
    //    if (isNavGuide)
    //    {
    //        playerCtrl.SetCamera();
    //        IsNavOver();
    //    }
    //}
    //private void StopNavTask()
    //{
    //    //Debug.Log("StopNavTask");
    //    if (isNavGuide)
    //    {
    //        isNavGuide = false;
    //        nav.isStopped = true;
    //        playerCtrl.SetBlend(Constants.blendIdle);
    //        nav.enabled = false;
    //    }
    //}
    //private void IsNavOver()
    //{
    //    float distance = Vector3.Distance(playerCtrl.transform.position,
    //           npcTransPos[currentTask.npcID].position);
    //    //Debug.Log("IsNavOver:distance " + distance);
    //    if (distance < 1f)
    //    {
    //        StopNavTask();
    //        OpenGuideWnd();
    //    }
    //}
    ////没有引导了
    //private void OpenGuideWnd()
    //{
    //    //guideWnd.SetWndState();
    //}
    //public AutoGuideCfg GetGuideData()
    //{
    //   return currentTask;
    //}
    //public void RspondGuide(GameMsg msg)
    //{
    //    RspGuide data = msg.rspGuide;

    //    GameRoot.instance.AddTips(Constants.Color("任务奖励 金币： " + currentTask.coin + " 经验： " + currentTask.exp,TxtColor.Blue));
    //    switch (currentTask.actID)
    //    {
    //        case 0:
    //            //智者对话
    //            break;
    //        case 1:
    //            //进入副本
    //            break;
    //        case 2:
    //            //强化
    //            break;
    //        case 3:
    //            //体力购买
    //            break;
    //        case 4:
    //            //铸造
    //            break;
    //        case 5:
    //            //世界聊天
    //            break;
    //        default:
    //            break;
    //    }
    //    //GameRoot.instance.SetPlayerDataByGuide(data);
    //    mainCityWnd.RefreshUI();
    //}
    //#endregion

    #region 强化信息相关
    public void OpenStrengthWnd()
    {
        strengthWnd.SetWndState();
    }
    //public void RspStrong(GameMsg msg)
    //{
    //    int fightValuePre = PECommon.GetFightByPlayerData(GameRoot.instance.Playerdata);
    //    GameRoot.instance.SetPlayerDataByStrong(msg.resStrong);
    //    int fightValueNow= PECommon.GetFightByPlayerData(GameRoot.instance.Playerdata);
    //    GameRoot.instance.AddTips(Constants.Color("战力提升 "+(fightValueNow-fightValuePre),TxtColor.Blue));
    //    strengthWnd.UpdateUI();
    //    mainCityWnd.RefreshUI();
    //}
    #endregion
}