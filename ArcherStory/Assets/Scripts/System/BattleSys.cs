/****************************************************
	文件：BattleSys.cs
	作者：Echo
	邮箱: 350383921@qq.com
	日期：2019/11/27 16:18   	
	功能：战斗系统
*****************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BattleSys:SystemRoot
{
    public static BattleSys Instance = null;
    public PlayerCtrlWnd playerCtrlWnd;
    [HideInInspector]
    public BattleManager battleMg;
    public BattleEndWnd battleEndWnd;

    private double startTime;
    private double endTime;
    public int costTime;//消耗时间的秒钟数
    public override void InitSys()
    {
        base.InitSys();
        Instance = this;
    }
    public void StartBattle(int mapid)
    {
        GameObject go = new GameObject { name = "BattleRoot" };
        go.transform.SetParent(GameRoot.instance.transform);
        battleMg = go.AddComponent<BattleManager>();
        //延时调用委托
        battleMg.Init(mapid,()=>
        {
            startTime = TimeService.instance.GetNowTime();
        });
        //Invoke("SetPlayerCtrlWndState", 0.5f);
        SetPlayerCtrlWndState();
    }
    public void EndBatlle(bool isWin,int restHP)
    {
        SetPlayerCtrlWndState(false);
        GameRoot.instance.dynamicWnd.RemoveAllHPItemInfo();
        if (isWin)
        {
            endTime = TimeService.instance.GetNowTime();
            //这里得到的是秒钟数
            costTime = (int)(endTime-startTime)/1000;
            battleEndWnd.SetBattleEndData(costTime,restHP);
            SetBattleEndWndState(FubenEndType.Win);
        }
        else
        {
            SetBattleEndWndState(FubenEndType.Lose);
        }
    }
    public void DestroyBattle()
    {
        battleMg.isPaused = false;
        //battleMG随后就被销毁了，没有运行update的修改timescale代码,手动修改
        Time.timeScale = 1f;
        SetPlayerCtrlWndState(false);
        SetBattleEndWndState(FubenEndType.None,false);
        GameRoot.instance.dynamicWnd.RemoveAllHPItemInfo();
        Destroy(battleMg.gameObject);
    }
    public void SetPlayerCtrlWndState(bool isActive = true)
    {
        playerCtrlWnd.SetWndState(isActive);
    }
    public void SetBattleEndWndState(FubenEndType endType, bool isActive=true)
    {
        battleEndWnd.SetWndType(endType);
        battleEndWnd.SetWndState(isActive);
    }
    public void ReleaseSkill(int index)
    {
        battleMg.ReleaseSkill(index);
    }
   
}
