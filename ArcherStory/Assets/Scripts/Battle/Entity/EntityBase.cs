/****************************************************
	文件：EntityBase.cs
	作者：Echo
	邮箱: 350383921@qq.com
	日期：2019/11/28 15:40   	
	功能：逻辑实体基类，实现的主体
*****************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class EntityBase
{
    public AniState currentState = AniState.None;
    public StateManager stateMg = null;
    public Controller controller=null;

    public void Idle()
    {
        stateMg.ChangeState(this, AniState.Idle);
    }
    public void Move()
    {
        stateMg.ChangeState(this, AniState.Run);
    }
    public void Attack()
    {
        stateMg.ChangeState(this,AniState.Attack);
    }
    public virtual void SetBlend(float blend)
    {
        if (controller!=null)
        {
            controller.SetBlend(blend);
        }
    }
    public virtual void SetDir(Vector2 dir)
    {
        if (controller!=null)
        {
            controller.Dir = dir;
        }
    }
}
