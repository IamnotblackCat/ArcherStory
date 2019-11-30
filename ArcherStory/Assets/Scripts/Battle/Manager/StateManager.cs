/****************************************************
	文件：BattleManager.cs
	作者：Echo
	邮箱: 350383921@qq.com
	日期：2019/11/27 16:22   	
	功能：状态管理
*****************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    private Dictionary<AniState, IState> fsm = new Dictionary<AniState, IState>();
    public void Init()
    {
        fsm.Add(AniState.Idle,new StateIdle());
        fsm.Add(AniState.Run,new StateRun());
        fsm.Add(AniState.Attack,new StateAttack());
    }
    public void ChangeState(EntityBase entity,AniState targetState, params object[] args)
    {
        if (entity.currentState==targetState)
        {
            return;
        }
        if (fsm.ContainsKey(targetState))
        {
            if (entity.currentState!=AniState.None)
            {
                fsm[entity.currentState].Exit(entity,args);
            }
            fsm[targetState].Enter(entity,args);
            fsm[targetState].Process(entity,args);
        }
    }
}
