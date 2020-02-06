/****************************************************
    文件：MainCityWnd.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/10/25 9:17:49
	功能：主城UI界面
*****************************************************/
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainCityWnd : WindowRoot 
{
    #region Public UI Transform
    
    public GameObject goFuben;
    public GameObject exitGame;
    public GameObject skillPanel;
    public GameObject skillDrawPanel;
    public GameObject skillDescriptionPanel;
    public Text txtLV;
    public Text txtExpPrg;

    public Button btnGuide;

    public Transform expProgramTrans;

    #endregion
    
    private AutoGuideCfg currentTaskData;

    //UI自适应不能使用固定距离，要计算得出比率距离
    private float pointDis = Screen.height * 1.0f / Constants.screenStandardHeight * Constants.screenOperationDistant;

    protected override void InitWnd()
    {
        base.InitWnd();
        
        RefreshUI();
    }
    public void RefreshUI()
    {
        PlayerData pd = GameRoot.instance.Playerdata;
        
        SetText(txtLV,pd.lv);

        #region ExpProgress
        int expValPercent = (int)(pd.exp * 1.0f);
        SetText(txtExpPrg, expValPercent + "%");
        int index = expValPercent / 10;
        GridLayoutGroup grid = expProgramTrans.GetComponent<GridLayoutGroup>();
        //得到 标准高度和当前高度的比例，然后乘以当前宽度得到真实宽度，然后减掉间隙计算经验条宽度
        float screenRate = 1.0f * Constants.screenStandardHeight / Screen.height;
        float screenWidth = Screen.width * screenRate;
        float width = (screenWidth - 180) / 10;

        grid.cellSize = new Vector2(width, 7);

        for (int i = 0; i < expProgramTrans.childCount; i++)
        {
            Image expImageValue = expProgramTrans.GetChild(i).GetComponent<Image>();
            if (i < index)
            {
                expImageValue.fillAmount = 1;
            }
            else if (i == index)
            {
                expImageValue.fillAmount = expValPercent * 1.0f % 10 / 10;
            }
            else
            {
                expImageValue.fillAmount = 0;
            }
        }
        #endregion
        //设置自动任务图标
        currentTaskData = resSvc.GetGuideCfgData(pd.guideid);
        if (currentTaskData != null)
        {
            SetGuideBtnIcon(currentTaskData.npcID);
        }
        else
        {//没任务就显示默认图标
            SetGuideBtnIcon(-1);
        }
    }
    //根据任务的不同设置不同的NPC头像
    private void SetGuideBtnIcon(int npcID)
    {
        string spPath = "";
        Image image = btnGuide.GetComponent<Image>();
        switch (npcID)
        {
            case Constants.NPCWiseMan:
                spPath = PathDefine.WiseHead;
                break;
            case Constants.NPCGeneral:
                spPath = PathDefine.GeneralHead;
                break;
            case Constants.NPCArtisan:
                spPath = PathDefine.ArtisanHead;
                break;
            case Constants.NPCTrader:
                spPath = PathDefine.TraderHead;
                break;
            default:
                spPath = PathDefine.TaskHead;
                break;
        }
        SetSprite(image,spPath);
    }
    #region Click Events
    public void ClickGuideBtn()
    {
        audioSvc.PlayUIAudio(Constants.uiClick);
        if (currentTaskData != null)
        {
            MainCitySys.Instance.RunTask(currentTaskData);
        }
        else
        {
            GameRoot.instance.AddTips("更多引导，正在开发中，敬请期待。。。");
        }
    }
    public void ClickHeadBtn()
    {
        audioSvc.PlayUIAudio(Constants.uiOpenPage);
        MainCitySys.Instance.OpenInfoWnd();
    }
    public void ClickFubenBtn()
    {
        audioSvc.PlayUIAudio(Constants.uiOpenPage);
        goFuben.SetActive(true);
    }
    public void ClickKnapsackBtn()
    {
        audioSvc.PlayUIAudio(Constants.uiOpenPage);
        Knapsack.Instance.DisplaySwitch();
    }
    public void ClickSkillBtn()
    {
        audioSvc.PlayUIAudio(Constants.uiOpenPage);
        skillPanel.SetActive(true);
    }
    public void ClickSystemBtn()
    {
        audioSvc.PlayUIAudio(Constants.uiOpenPage);
        exitGame.SetActive(true);
    }
    public void ClickFubenConfirm()
    {
        audioSvc.PlayUIAudio(Constants.uiClick);
        GameRoot.instance.ClearUIRoot();
        GameRoot.instance.dynamicWnd.SetWndState();
        goFuben.SetActive(false);
        BattleSys.Instance.StartBattle(Constants.Duplicate);
    }
    public void ClickExitConfirm()
    {
        audioSvc.PlayUIAudio(Constants.uiClick);
        exitGame.SetActive(false);
        Application.Quit();
    }
    public void ClickExitCancel()
    {
        audioSvc.PlayUIAudio(Constants.uiClick);
        exitGame.SetActive(false);
    }
    public void ClickFubenConcel()
    {
        audioSvc.PlayUIAudio(Constants.uiClick);
        goFuben.SetActive(false);
    }
    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector2 _dir = new Vector2(h, v);
        MainCitySys.Instance.SetMoveDir(_dir);
    }
    public void CloseSkillPanel()
    {
        skillPanel.SetActive(false);
    }
    public void ClickSwitchSkillPanel()
    {
        if (skillDrawPanel.activeSelf)
        {
            skillDescriptionPanel.SetActive(true);
            skillDrawPanel.SetActive(false);
        }
        else
        {
            skillDrawPanel.SetActive(true);
            skillDescriptionPanel.SetActive(false);
        }
    }
    #endregion

}