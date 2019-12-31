/****************************************************
    文件：BattleEndWnd.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/12/31 16:15:25
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class BattleEndWnd : WindowRoot 
{
    public Transform rewardTrans;
    public Button btnClose;
    public Button btnExit;
    public Button btnSure;
    public Text txtTime;
    public Text txtRestHP;
    public Text txtReward;

    private FubenEndType endType = FubenEndType.None;
    protected override void InitWnd()
    {
        base.InitWnd();

        RefreshUI();
    }
    private void RefreshUI()
    {
        switch (endType)
        {
            case FubenEndType.Pause:
                SetActive(rewardTrans,false);
                SetActive(btnExit.gameObject);
                SetActive(btnClose.gameObject);
                break;
            case FubenEndType.Win:
                break;
            case FubenEndType.Lose:
                break;
        }
    }
    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.uiClick);
        SetWndState(false);
    }
    public void ClickExitBtn()
    {
        audioSvc.PlayUIAudio(Constants.uiClick);
        //进入主城，销毁战斗
    }
    public void ClickSureBtn()
    {
        audioSvc.PlayUIAudio(Constants.uiClick);
        //进入主城，销毁战斗
    }
    public void SetWndType(FubenEndType endType)
    {
        this.endType = endType;  
    }
}

public enum FubenEndType
{
    None,
    Pause,
    Win,
    Lose
}