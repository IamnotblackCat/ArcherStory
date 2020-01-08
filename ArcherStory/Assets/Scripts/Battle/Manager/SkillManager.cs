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
        entity.skillActionCallBackList.Clear();
        entity.skillEffectCallBackList.Clear();
        AttackEffect(entity,skillID);
        AttackDamage(entity,skillID);
    }
    /// <summary>
    /// 技能效果表现
    /// </summary>
    /// <param name="entity">。。。</param>
    /// <param name="skillID"></param>
    public void AttackEffect(EntityBase entity,int skillID)
    {
        SkillCfg skillData = resSvc.GetSkillCfgData(skillID);
        //分技能类型调整角色行为
        if (entity.entityType==EntityType.Player)
        {
            /*如果是指向技能，自动转向最近目标，如果是范围技能，朝向区域方向，
             *如果是辅助技能，方向不变,闪避技能被设定为指向技能，因为永远朝向
             *目标并且背向移动*/
            if (skillData.dmgType == DamageType.TargetSkill)
            {
                Vector2 dir = entity.CalculateTargetDir();
                if (dir != Vector2.zero)
                {
                    entity.SetAtkRotation(dir);
                }
            }
            else if (skillData.dmgType == DamageType.AreaSkill)
            {
                Vector2 dir = entity.AreaSkillTargetDir();
                if (dir != Vector2.zero)
                {
                    entity.SetAtkRotation(dir);
                }
                //如果是范围技能，且不是瞬移，在动画播放快结束的时候播放对应延迟特效,持续时间后消失。
                if (skillData.ID != 107)
                {
                    entity.SetAreaSkillFX(skillData.targetFX, skillData.delayFXTime, skillData.delayCloseFXTime);

                }
            }
            else//辅助技能
            {
                if (skillData.ID==105)//治疗技能
                {
                    entity.Hp += GameRoot.instance.Playerdata.hp / 3;
                    if (entity.Hp>GameRoot.instance.Playerdata.hp)
                    {
                        entity.Hp = GameRoot.instance.Playerdata.hp;
                    }
                }
                else if (skillData.ID==106)//状态技能，攻击力+50%，进入霸体状态
                {
                    entity.AttackValue += GameRoot.instance.Playerdata.attackValue / 2;
                    entity.unBreakable = true;
                    TimeService.instance.AddTimeTask((int tid)=>
                    {
                        entity.AttackValue -= GameRoot.instance.Playerdata.attackValue/2;
                        entity.unBreakable = false;
                    },15f);
                }
            }
        }
        entity.SetAction(skillData.aniAction);
        entity.SetFX(skillData.fx,skillData.skillFXTime);

        if (skillData.cantStop)
        {
            entity.cantStop = true;
        }
        SkillMoveCfg skillMoveCfg = resSvc.GetSkillMoveCfgData(skillData.skillMove);
        float speed = 0;
        if (skillData.ID==107)//是瞬移技能，不是闪避
        {
            float dis = Vector3.Distance(entity.GetPos(),BattleSys.Instance.playerCtrlWnd.pos);
            speed = dis / (skillMoveCfg.moveTime/1000f);
            entity.SetSkillMoveState(true,false,speed);
        }
        else if(skillData.ID==108)//闪避技能
        {
            speed = skillMoveCfg.moveDis / (skillMoveCfg.moveTime / 1000f);//单位是毫秒
            entity.SetSkillMoveState(true,true,speed);
        }
        timeSvc.AddTimeTask((int tid) =>
        {//技能移动时间到就设置为不能移动
            entity.SetSkillMoveState(false);
        }, skillMoveCfg.moveTime/1000f);
        timeSvc.AddTimeTask((int tid)=>
        {
            entity.Idle();
            
            /*不能直接在这里修改action的值是因为，这个攻击可能会被打断，被打断以后不会进入这个状态
            这样就无法设置action了，所以是在退出攻击状态的时候设置action*/
        },skillData.animationTime);
    }
    //技能伤害
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

                int attackid = timeSvc.AddTimeTask((int tid) =>
                 {
                     if (entity != null)//多个攻击同时打过来，已经死亡就会报空
                     {
                         SkillAction(entity, skillData, index);
                         entity.RemoveActionCallBake(tid);
                     }
                         //LoopSkill(entity,skillData,index,sum);
                     }, sum * 1.0f / 1000);
                entity.skillActionCallBackList.Add(attackid);
            }
            else//瞬间伤害
            {
                SkillAction(entity,skillData,index);
            }
        }
    }
    //永久存在的技能
    private void LoopSkill(EntityBase entity,SkillCfg skillData,int index,int sum)
    {
        timeSvc.AddTimeTask((int temp) =>
        {
            SkillAction(entity, skillData, index);
            LoopSkill(entity,skillData,index,sum);
        }, sum * 1.0f / 1000);
    }
    public void SkillAction(EntityBase caster,SkillCfg skillCfg,int index)
    {
        SkillActionCfg skillActionCfg = resSvc.GetSkillActionData(skillCfg.skillActionList[index]);

        int damage = skillCfg.skillDamageList[index];
        if (caster.entityType==EntityType.Player)
        {
            //拿到场景中的怪物实体，遍历运算
            List<EntityMonster> monsterList = caster.battleMg.GetEntityMonsters();

            for (int i = 0; i < monsterList.Count; i++)
            {
                EntityMonster target = monsterList[i];
                if (target==null)
                {
                    return;
                }
                //判断距离，角度
                //如果是范围技能，施法者位置变更为鼠标指定区域位置，角度固定为360度
                if (skillCfg.dmgType == DamageType.AreaSkill)
                {
                    if (InRange(BattleSys.Instance.playerCtrlWnd.pos, target.GetPos(), skillActionCfg.radius))
                    {
                        CalcDamage(caster, target, skillCfg, damage);
                    }
                }
                else
                {
                    if (InRange(caster.GetPos(), target.GetPos(), skillActionCfg.radius) && InAngle(caster.GetTrans(), target.GetPos(), skillActionCfg.angle))
                    {
                        //计算伤害
                        CalcDamage(caster, target, skillCfg, damage);
                    }
                    else if(!InRange(caster.GetPos(), target.GetPos(), skillActionCfg.radius))
                    {
                        GameRoot.instance.AddTips("超出攻击范围");
                    }
                }
            }

        }
        else if (caster.entityType==EntityType.Monster)//如果是怪物只能攻击玩家
        {
            EntityPlayer target = caster.battleMg.entitySelfPlayer;
            //判断距离角度
            if (InRange(caster.GetPos(),target.GetPos(),skillActionCfg.radius)&&InAngle(caster.GetTrans(), target.GetPos(), skillActionCfg.angle))
            {
                CalcDamage(caster, target, skillCfg, damage);
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
        dmgSum += caster.AttackValue;
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
            if (target.entityType==EntityType.Monster)
            {
                target.battleMg.RemoveMonster(target.Name);

            }
            else if (target.entityType==EntityType.Player)
            {
                target.battleMg.EndBattle(false,0);
                target.battleMg.entitySelfPlayer = null;
            }
        }
        else
        {
            target.Hp -= dmgSum;
            //判断是否符合获取霸体条件
            target.GetUnBreakState();
            //TODO,是否处于剑刃风暴状态，是则反弹伤害
            if (!target.unBreakable&&!target.cantStop)
            {
                target.Wound();
            }
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
