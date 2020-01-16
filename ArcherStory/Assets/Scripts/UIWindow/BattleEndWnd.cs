/****************************************************
    文件：BattleEndWnd.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/12/31 16:15:25
	功能：战斗结束页面
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
    private int costTime;
    private int restHP;
    private TimeService timeSvc;
    protected override void InitWnd()
    {
        base.InitWnd();
        timeSvc = TimeService.instance;
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
                SetActive(rewardTrans,false);
                SetActive(btnExit.gameObject,false);
                SetActive(btnClose.gameObject,false);

                int min = costTime / 60;
                int second = costTime % 60;
                SetText(txtTime,"通关时间 ："+min+":"+second);
                SetText(txtRestHP,"剩余血量 ："+restHP);
                //嵌套播放结算界面的音效
                timeSvc.AddTimeTask((int tid) =>
                {
                    SetActive(rewardTrans);
                    audioSvc.PlayUIAudio(Constants.FBItemEnter);
                    timeSvc.AddTimeTask((int tid1) =>
                    {
                        audioSvc.PlayUIAudio(Constants.FBItemEnter);
                        timeSvc.AddTimeTask((int tid2) =>
                        {
                            audioSvc.PlayUIAudio(Constants.FBItemEnter);
                            timeSvc.AddTimeTask((int tid3) =>
                            {
                                audioSvc.PlayUIAudio(Constants.FBItemEnter);
                                timeSvc.AddTimeTask((int tid4) =>
                                {
                                    audioSvc.PlayUIAudio(Constants.FBWin);
                                }, 0.1f);
                            }, 0.27f);
                        }, 0.27f);
                    }, 0.325f);
                }, 1f);
                break;
            case FubenEndType.Lose:
                SetActive(rewardTrans, false);
                SetActive(btnExit.gameObject);
                SetActive(btnClose.gameObject,false);
                audioSvc.PlayUIAudio(Constants.FBLose);
                
                break;
        }
    }
    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.uiClick);
        BattleSys.Instance.battleMg.isPaused = false;
        SetWndState(false);
    }
    public void ClickExitBtn()
    {
        audioSvc.PlayUIAudio(Constants.uiClick);
        //进入主城，销毁战斗
        MainCitySys.Instance.EnterMainCity();
        BattleSys.Instance.DestroyBattle();
    }
    public void ClickSureBtn()
    {
        audioSvc.PlayUIAudio(Constants.uiClick);
        //进入主城，销毁战斗
        MainCitySys.Instance.EnterMainCity();
        BattleSys.Instance.DestroyBattle();
    }
    public void SetWndType(FubenEndType endType)
    {
        this.endType = endType;  
    }
    public void SetBattleEndData(int costTime,int restHP)
    {
        this.costTime = costTime;
        this.restHP = restHP;
    }
}

public enum FubenEndType
{
    None,
    Pause,
    Win,
    Lose
}