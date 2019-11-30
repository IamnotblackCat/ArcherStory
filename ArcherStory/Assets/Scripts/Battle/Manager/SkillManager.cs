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
    private ResSvc resSvc;
    public void Init()
    {
        resSvc = ResSvc.instance;
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
    }
}
