/****************************************************
    文件：PlayerCtrlWnd.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/11/28 14:41:45
	功能：战斗场景界面
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrlWnd : WindowRoot
{
    #region Public UI Transform
    
    public GameObject returnGo;
    public Text txtLV;

    public Button btnGuide;

    public Transform expProgramTrans;

    #endregion

    private bool menuState = true;//true是打开，false收起
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
        
        SetText(txtLV, pd.lv);

        #region ExpProgress
        int expValPercent = (int)(pd.exp * 1.0f / 100);
        
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
    }
    
    #region Click Events
    
    public void ClickHeadBtn()
    {
        audioSvc.PlayUIAudio(Constants.uiOpenPage);
        MainCitySys.Instance.OpenInfoWnd();
    }
    public void ClickReturnBtn()
    {
        returnGo.SetActive(true);
    }
    public void ClickConfirm()
    {
        GameRoot.instance.ClearUIRoot();
        BattleSys.Instance.StartBattle(1003);
    }
    public void ClickConcel()
    {
        returnGo.SetActive(false);
    }

    #endregion
    public void ClickSkill1()
    {
        BattleSys.Instance.ReleaseSkill(1);
    }
    public void ClickSkill2()
    {
        BattleSys.Instance.ReleaseSkill(2);
    }
    public void ClickSkill3()
    {
        BattleSys.Instance.ReleaseSkill(3);
    }
    public void ClickSkill4()
    {
        BattleSys.Instance.ReleaseSkill(4);
    }
    public void ClickSkill5()
    {
        BattleSys.Instance.ReleaseSkill(5);
    }
    public void ClickSkill6()
    {
        BattleSys.Instance.ReleaseSkill(6);
    }
    public void ClickSkill7()
    {
        BattleSys.Instance.ReleaseSkill(7);
    }
    public void ClickSkill8()
    {
        BattleSys.Instance.ReleaseSkill(8);
    }
    private void Update()
    {
        
    }
}