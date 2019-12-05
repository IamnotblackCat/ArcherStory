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
    public void SkillAttack(EntityBase entity,int skillID)
    {
        AttackDamage(entity,skillID);
        AttackEffect(entity,skillID);
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

        SkillMoveCfg skillMoveCfg = resSvc.GetSkillMoveCfgData(skillData.skillMove);
        float speed = skillMoveCfg.moveDis / (skillMoveCfg.moveTime / 1000f);//单位是毫秒
        entity.SetSkillMoveState(true,speed);
        timeSvc.AddTimeTask((int tid) =>
        {//技能移动时间到就设置为不能移动
            entity.SetSkillMoveState(false);
        }, skillMoveCfg.moveTime/1000f);
        timeSvc.AddTimeTask((int tid)=>
        {
            entity.Idle();
            //Debug.Log("转换状态");
            /*不能直接在这里修改action的值是因为，这个攻击可能会被打断，被打断以后不会进入这个状态
            这样就无法设置action了，所以是在退出攻击状态的时候设置action*/
        },skillData.skillTime);
    }
    public void AttackDamage(EntityBase entity,int skillID)
    {
        SkillCfg skillData = resSvc.GetSkillCfgData(skillID);
        List<int> actionList = skillData.skillActionList;

        int sum = 0;
        for (int i = 0; i < actionList.Count; i++)
        {
            SkillActionCfg skillAction = resSvc.GetSkillActionData(actionList[i]);
            sum += skillAction.delayTime;
            if (sum>0)//延迟伤害，比如火圈范围持续伤害
            {
                timeSvc.AddTimeTask((int tid) =>
                {
                    SkillAction(entity,skillAction.ID);
                },sum);
            }
            else//瞬间伤害
            {
                SkillAction(entity, skillAction.ID);
            }
        }
    }
    public void SkillAction(EntityBase entity,int actionID)
    {

    }
}
