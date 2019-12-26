/****************************************************
	文件：StateAttack.cs
	作者：Echo
	邮箱: 350383921@qq.com
	日期：2019/11/29 16:34   	
	功能：攻击状态
*****************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class StateAttack : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        //Debug.Log("技能2");
        entity.canControll = false;
        entity.SetDir(Vector2.zero);
        entity.currentState = AniState.Attack;
        //得到当前技能数据
        entity.currentSkillCfg = ResSvc.instance.GetSkillCfgData((int)args[0]);
    }

    public void Exit(EntityBase entity, params object[] args)
    {
        //Debug.Log("回调");
        entity.canControll = true;
        entity.SetAction(Constants.ActionDefault);
        entity.ExitSkill();
    }

    public void Process(EntityBase entity, params object[] args)
    {
        //技能伤害与特效表现
        entity.SkillAttack((int)args[0]);
    }
}
