/****************************************************
	文件：GameRoot.cs
	作者：Echo
	邮箱: 350383921@qq.com
	日期：2019/11/20 10:18   	
	功能：游戏启动入口
*****************************************************/

//using PEProtocol;
using UnityEngine;

public class GameRoot : MonoBehaviour 
{
    public static GameRoot instance = null;
    public LoadingWnd loadingWnd;
    public DynamicWnd dynamicWnd;

    private void Start()
    {
        instance = this;
        DontDestroyOnLoad(this);
        //PECommon.Log("游戏开始。。。");
        ClearUIRoot();
        Init();    
    }
    private void Init()
    {

        //服务模块初始化
        //NetService netService = GetComponent<NetService>();
        //netService.InitSvc();
        ResSvc resSvc = GetComponent<ResSvc>();
        resSvc.InitSvc();
        AudioSvc audioSvc = GetComponent<AudioSvc>();
        audioSvc.InitSvc();

        //业务系统初始化
        //LoginSys loginSys = GetComponent<LoginSys>();
        //loginSys.InitSys();
        MainCitySys mainCitySys = GetComponent<MainCitySys>();
        mainCitySys.InitSys();

        //动画系统-老的模型不支持animator

        //进入登陆场景并加载UI
        //loginSys.EnterLogin();

        
    }
    //初始化的时候确保所有的UI除了提示面板都是隐藏的
    private void ClearUIRoot()
    {
        Transform canvasTrans = transform.Find("Canvas");
        for (int i = 0; i < canvasTrans.childCount; i++)
        {
            if (i!=0)
            {
                canvasTrans.GetChild(i).gameObject.SetActive(false);
            }
        }
        
        dynamicWnd.SetWndState();
    }
    public void AddTips(string tips)
    {
        dynamicWnd.AddTips(tips);
    }
    private PlayerData playerData;
    public PlayerData Playerdata
    {
        get { return playerData; }
    }
    //public void SetPlayerData(ResponLogin data)
    //{
    //    playerData = data.playerData;
    //}
    public void SetPlayerName(string name)
    {
        playerData.name = name;
    }
    //public void SetPlayerDataByGuide(RspGuide data)
    //{
    //    playerData.coin = data.coin;
    //    playerData.exp = data.exp;
    //    playerData.lv = data.lv;
    //    //playerData.guideid = data.guideid;
    //}
    //public void SetPlayerDataByStrong(ResStrong data)
    //{
    //    Playerdata.coin = data.coin;
    //    Playerdata.crystal = data.crystal;
    //    Playerdata.attackValue = data.ad;

    //    //Playerdata.strengthArray = data.strongArr;
    //}
}