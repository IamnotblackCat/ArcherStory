/****************************************************
	文件：IState.cs
	作者：Echo
	邮箱: 350383921@qq.com
	日期：2019/11/28 15:38   	
	功能：状态基类
*****************************************************/
using System;
using System.Collections.Generic;
using System.Text;


public interface IState
{
    void Enter(EntityBase entity);
    void Exit(EntityBase entity);
    void Process(EntityBase entity);
}
public enum AniState
{
    None,
    Idle,
    Run,
    Attack,
}
