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
            int index = i;
            if (sum>0)//延迟伤害，比如火圈范围持续伤害
            {
                timeSvc.AddTimeTask((int tid) =>
                {
                    SkillAction(entity,skillData,index);
                },sum/1000);
            }
            else//瞬间伤害
            {
                SkillAction(entity,skillData,index);
            }
        }
    }
    public void SkillAction(EntityBase caster,SkillCfg skillCfg,int index)
    {
        SkillActionCfg skillActionCfg = resSvc.GetSkillActionData(skillCfg.skillActionList[index]);

        int damage = skillCfg.skillDamageList[index];
        //拿到场景中的怪物实体，遍历运算
        List<EntityMonster> monsterList = caster.battleMg.GetEntityMonsters();

        for (int i = 0; i < monsterList.Count; i++)
        {
            EntityMonster target = monsterList[i];
            //判断距离，角度
            if (InRange(caster.GetPos(),target.GetPos(),skillActionCfg.radius) &&InAngle(caster.GetTrans(),target.GetPos(),skillActionCfg.angle))
            {
                //计算伤害
                CalcDamage(caster,target,skillCfg, damage);
            }
        }
    }
    System.Random rd = new System.Random();
    /// <summary>
    /// 技能伤害计算
    /// </summary>
    /// <param name="caster">施法者</param>
    /// <param name="target">目标</param>
    /// <param name="skillCfg">技能数据配置</param>
    /// <param name="damage">技能原本伤害值</param>
    private void CalcDamage(EntityBase caster,EntityBase target,SkillCfg skillCfg, int damage)
    {//伤害除了技能本身伤害，还需要加上自身属性值
        int dmgSum = damage;

        //计算属性加成
        dmgSum += caster.BattleProps.attackValue;
        //暴击
        int criticalNum = PETools.RDInt(1,100,rd);
        if (criticalNum<=caster.BattleProps.critical)
        {
            //暴击伤害比率在110%~200%之间
            float criticalRate = 1 + (PETools.RDInt(10, 100, rd) / 100.0f);
            dmgSum = (int)criticalRate * dmgSum;
            target.SetCritical(dmgSum);
            //Debug.Log("暴击伤害率："+criticalRate+" 暴击率："+caster.BattleProps.critical);
        }
        //计算防御扣减
        dmgSum -= target.BattleProps.defend;
        //最小伤害为0
        if (dmgSum<0)
        {
            dmgSum = 0;
            return;
        }
        target.SetHurt(dmgSum);
        if (target.Hp<=dmgSum)
        {
            target.Hp = 0;
            //死亡
            target.Die();
        }
        else
        {
            target.Hp -= dmgSum;
            target.Wound();
        }
    }
    private bool InRange(Vector3 from,Vector3 to,float range)
    {
        float dis = Vector3.Distance(from,to);
        if (dis<=range)
        {
            return true;
        }
        return false;
    }

    private bool InAngle(Transform trans,Vector3 target,float angle)
    {
        if (angle==360)
        {
            return true;
        }
        else//扇形攻击范围，是否在范围内检测。
        {
            Vector3 start = trans.forward;
            Vector3 dir = (target - trans.position).normalized;

            float ang = Vector3.Angle(start,dir);
            if (ang<=angle/2)
            {
                return true;
            }
            return false;
        }
    }
}
