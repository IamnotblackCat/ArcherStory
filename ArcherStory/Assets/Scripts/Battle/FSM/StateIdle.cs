/****************************************************
	文件：StateIdle.cs
	作者：Echo
	邮箱: 350383921@qq.com
	日期：2019/11/28 15:39   	
	功能：Idle状态
*****************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class StateIdle : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentState = AniState.Idle;
        //进来就设置方向向量为0，停止移动
        entity.SetDir(Vector2.zero);
    }

    public void Exit(EntityBase entity, params object[] args)
    {
    }

    public void Process(EntityBase entity, params object[] args)
    {
        entity.SetBlend(Constants.blendIdle);
    }
}
