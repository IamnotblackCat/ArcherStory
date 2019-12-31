/****************************************************
	文件：EntityMonster.cs
	作者：Echo
	邮箱: 350383921@qq.com
	日期：2019/12/04 15:09   	
	功能：怪物逻辑实体
*****************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class EntityMonster:EntityBase
{
    public MonsterData md;
    //启动的时候等待2秒检测一下玩家位置
    private float checkTime = 2;
    private float checkTimeCount = 0;
    //2秒攻击一次
    private float atkTime = 2;
    private float atkTimeCount = 0;
    private bool runAI = true;

    public EntityMonster()
    {
        entityType = EntityType.Monster;
    }
    //怪物子类重写了这个方法，让怪物在不同地图或同一地图受等级影响属性（考虑到数值问题，暂时停止）
    public override void SetBattleProps(BattleProps props)
    {
        int level = md.mLevel;
        BattleProps p = new BattleProps
        {
            hp=props.hp*level,
            attackValue=props.attackValue*level,
            defend=props.defend*level,
            critical=props.critical*level,
            
        };
        BattleProps = p;
        Hp = p.hp;
        AttackValue = p.attackValue;
    }
    public override void TickAILogic()
    {
        if (!runAI)
        {
            return;
        }
        //if (currentState==AniState.Idle||currentState==AniState.Run)
        //{
            float delta = Time.deltaTime;
            checkTimeCount += delta;
            if (checkTimeCount < checkTime)
            {
                return;
            }
            else
            {
                //计算目标方向
                Vector2 dir = CalculateTargetDir();
                if (!InAtkRange())
                {
                    //不在范围：设置移动方向，进入移动状态
                    SetDir(dir);
                    Move();
                }
                else
                {
                    //在：停止移动，进行攻击
                    SetDir(Vector2.zero);//这里设置为0，controller就会把状态修改为idle
                                         //判断攻击间隔,移动过程的时间也在累计攻击间隔
                    //atkTimeCount += delta;
                    atkTimeCount += checkTimeCount;
                    if (atkTimeCount > atkTime)
                    {
                        //达到攻击时间，转向并且攻击
                        SetAtkRotation(dir);
                        //TODO多个技能需要用出招表来控制，
                        Attack(md.mCfg.skillID);
                        atkTimeCount = 0;
                    }
                    else
                    {
                        Idle();
                    }
                }
                checkTimeCount = 0;
                //让检测时间在0.1~0.5秒内浮动
                checkTime = PETools.RDInt(1, 5) * 1.0f / 10;
            }

        //}
    }
    public override Vector2 CalculateTargetDir()
    {
        EntityPlayer entityPlayer = battleMg.entitySelfPlayer;
        //玩家死亡就停止AI驱动
        if (entityPlayer==null||entityPlayer.currentState==AniState.Die)
        {
            runAI = false;
            return Vector2.zero;
        }
        else
        {
            Vector3 target = entityPlayer.GetPos();
            Vector3 self = GetPos();
            Vector2 dir = new Vector2(target.x - self.x, target.z - self.z);
            dir = dir.normalized;
            return dir;
        }
    }

    private bool InAtkRange()
    {
        EntityPlayer entityPlayer = battleMg.entitySelfPlayer;
        //玩家死亡就停止AI驱动
        if (entityPlayer == null || entityPlayer.currentState == AniState.Die)
        {
            runAI = false;
            return false;
        }
        else
        {
            Vector3 target = entityPlayer.GetPos();
            Vector3 self = GetPos();
            target.y = 0;
            self.y = 0;
            float dis = Vector3.Distance(target,self);
            if (dis<=md.mCfg.atkDis)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    //boss血量低于90%进入霸体
    public override void GetUnBreakState()
    {
        if (unBreakable)
        {
            return;
        }
        //每次扣血的时候进入
        if (md.mCfg.mType==MonsterType.Boss)
        {
            if (Hp <= BattleProps.hp * 0.9)
            {
                unBreakable = true;
                //TODO，霸体特效
                
            }
        }
    }
    public override void SetHPVal(int oldVal, int newVal)
    {
        if (md.mCfg.mType==MonsterType.Boss)
        {
            BattleSys.Instance.playerCtrlWnd.SetBossHPBarVal(oldVal,newVal,BattleProps.hp);
        }
        else
        {
            base.SetHPVal(oldVal,newVal);
        }
    }
}
