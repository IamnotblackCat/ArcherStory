/****************************************************
	文件：StateDie.cs
	作者：Echo
	邮箱: 350383921@qq.com
	日期：2019/12/06 9:48   	
	功能：死亡状态
*****************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class StateDie : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.canControll = false;
        entity.SetDir(Vector2.zero);
        entity.currentState = AniState.Die;
        entity.RemoveSkillCallBack();
    }

    public void Exit(EntityBase entity, params object[] args)
    {
        
    }

    public void Process(EntityBase entity, params object[] args)
    {
        entity.SetAction(Constants.ActionDie);
        TimeService.instance.AddTimeTask((int tid)=>
        {
            entity.SetCtrlActive(false);
        },Constants.DieAniLength);
    }
}
