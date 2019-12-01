/****************************************************
	文件：SkillManager.cs
	作者：Echo
	邮箱: 350383921@qq.com
	日期：2019/11/27 16:24   	
	功能：技能管理器
*****************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class SkillManager:MonoBehaviour
{
    private TimeService timeSvc;
    private ResSvc resSvc;
    public void Init()
    {
        resSvc = ResSvc.instance;
        timeSvc = TimeService.instance;
    }
    /// <summary>
    /// 技能效果表现
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="skillID"></param>
    public void AttackEffect(EntityBase entity,int skillID)
    {
        SkillCfg skillData = resSvc.GetSkillCfgData(skillID);

        entity.SetAction(skillData.aniAction);
        entity.SetFX(skillData.fx,skillData.skillTime);
        timeSvc.AddTimeTask((int tid)=>
        {
            entity.Idle();
            /*不能直接在这里修改action的值是因为，这个攻击可能会被打断，被打断以后不会进入这个状态
            这样就无法设置action了，所以是在退出攻击状态的时候设置action*/
        },skillData.skillTime);
    }
}
