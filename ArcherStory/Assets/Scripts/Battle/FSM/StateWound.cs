/****************************************************
	文件：StateWound.cs
	作者：Echo
	邮箱: 350383921@qq.com
	日期：2019/12/06 11:26   	
	功能：受伤状态
*****************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class StateWound : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentState = AniState.Wound;
    }

    public void Exit(EntityBase entity, params object[] args)
    {
        
    }

    public void Process(EntityBase entity, params object[] args)
    {
        //背击僵直
        entity.SetDir(Vector2.zero);
        entity.SetAction(Constants.ActionWound);

        //需要考虑到每一个模型的被攻击动画是不一样长度的
        TimeService.instance.AddTimeTask((int tid)=> {
            entity.SetAction(Constants.ActionDefault);
            entity.Idle();
        },GetWoundAnimationLength(entity));
    }
    private float GetWoundAnimationLength(EntityBase entity)
    {
        AnimationClip[] clips = entity.GetAniClips();
        //查找动画名称包含wound的动画长度，如果没找到设置一个默认保护值
        for (int i = 0; i < clips.Length; i++)
        {
            string clipName = clips[i].name;
            if (clipName.Contains("wound")||
                clipName.Contains("Wound") ||
                clipName.Contains("WOUND") )
            {
                return clips[i].length;
            }
        }
        //保护之
        return 1;
    }
}
