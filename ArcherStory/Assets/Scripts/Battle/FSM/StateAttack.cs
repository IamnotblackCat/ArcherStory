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
        entity.currentState = AniState.Attack;
    }

    public void Exit(EntityBase entity, params object[] args)
    {
    }

    public void Process(EntityBase entity, params object[] args)
    {
        entity.AttackEffect((int)args[0]);
    }
}
