/****************************************************
	文件：StateRun.cs
	作者：Echo
	邮箱: 350383921@qq.com
	日期：2019/11/28 15:41   	
	功能：移动状态
*****************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class StateWalk : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentState = AniState.Walk;
    }

    public void Exit(EntityBase entity, params object[] args)
    {
    }

    public void Process(EntityBase entity, params object[] args)
    {
        //判断是boss以后播放的是walk动画，因为血量降低到50%以后才播放run动画提速
        //这里的判断并不是真正的判断，是为了防止在boss提速以后第二次进入这个状态修改回walk
        if (entity is EntityMonster)
        {
            EntityMonster monster = (EntityMonster)entity;
            if (monster.md.mCfg.mType==MonsterType.Boss)
            {
                if (monster.speedUp)
                {
                    entity.SetBlend(Constants.blendRun);
                }
                else
                {
                    entity.SetBlend(Constants.blendWalk);
                }
            }
            else
            {
                entity.SetBlend(Constants.blendRun);
            }
        }
        else
        {
            entity.SetBlend(Constants.blendRun);

        }
    }
}
