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
    void Enter(EntityBase entity,params object[] args);//可变参数，可能有或者没有，不确定是什么参数
    void Exit(EntityBase entity, params object[] args);
    void Process(EntityBase entity, params object[] args);
}
public enum AniState
{
    None,
    Idle,
    Walk,
    Attack,
    Wound,
    Die
}
