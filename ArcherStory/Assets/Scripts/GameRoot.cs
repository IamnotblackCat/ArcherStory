/****************************************************
	文件：GameRoot.cs
	作者：Echo
	邮箱: 350383921@qq.com
	日期：2019/11/20 10:18   	
	功能：游戏启动入口
*****************************************************/

using UnityEngine;

public class GameRoot : MonoBehaviour 
{
    public static GameRoot instance = null;
    public LoadingWnd loadingWnd;
    public DynamicWnd dynamicWnd;
    public GameObject bossBeginWnd;

    public MainCityWnd mainCityWnd;
    //再主城界面读取是否更换了武器，在战斗管理加载战斗角色的时候根据这个来更换
    public bool isNewBow;
    private void Start()
    {
        instance = this;
        DontDestroyOnLoad(this);
        ClearUIRoot();
        Init();    
    }
    private void Init()
    {

        //服务模块初始化
        ResSvc resSvc = GetComponent<ResSvc>();
        resSvc.InitSvc();

        AudioSvc audioSvc = GetComponent<AudioSvc>();
        audioSvc.InitSvc();

        TimeService timeSvc = GetComponent<TimeService>();
        timeSvc.InitSvc();

        MainCitySys mainCitySys = GetComponent<MainCitySys>();
        mainCitySys.InitSys();
        BattleSys battleSys = GetComponent<BattleSys>();
        battleSys.InitSys();

        
        dynamicWnd.SetWndState();

    }
    //初始化的时候确保所有的UI除了提示面板都是隐藏的
    public void ClearUIRoot()
    {
        Transform canvasTrans = transform.Find("Canvas");
        for (int i = 0; i < canvasTrans.childCount; i++)
        {
            if (i!=0)
            {
                canvasTrans.GetChild(i).gameObject.SetActive(false);
            }
        }
        
    }
    public void AddTips(string tips)
    {
        dynamicWnd.AddTips(tips);
    }
    private PlayerData playerData=new PlayerData();
    public PlayerData Playerdata
    {
        get { return playerData; }
    }
    //从json中更新读取数据
    public void ReadPlayerData()
    {
        playerData.ReadJson();
    }
    public void GetExp(int exp)
    {
        playerData.CalculateExp(exp);
        mainCityWnd.RefreshUI();
    }
    public void SetPlayerDataByEquipment(int attackValue,int hpValue)
    {
        //先修改一下playerData为基础数值
        playerData.ReadJson();
        playerData.attackValue += attackValue;
        playerData.hp += hpValue;
        //数值发生变化的时候就刷新一下详细信息
        MainCitySys.Instance.infoWnd.RefreshUI();
    }
    
}