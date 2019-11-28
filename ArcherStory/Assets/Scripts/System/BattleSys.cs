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
    public BattleManager battleMg;

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

        battleMg.Init(mapid);
        //Invoke("SetPlayerCtrlWndState", 0.5f);
        SetPlayerCtrlWndState();
    }
    public void SetPlayerCtrlWndState(bool isActive = true)
    {
        playerCtrlWnd.SetWndState(isActive);
    }

    public void ReleaseSkill(int index)
    {
        battleMg.ReleaseSkill(index);
    }
   
}
