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
using Random = UnityEngine.Random;

public class EntityMonster:EntityBase
{
    public bool isCrazy = false;
    public MonsterData md;
    //启动的时候等待2秒检测一下玩家位置
    private float checkTime = 2;
    private float checkTimeCount = 0;
    //2秒攻击一次
    private float atkTime = 2;
    private float atkTimeCount = 0;
    private bool runAI = true;

    private int[] bossSkillArray = new int[]{301,302,303,304};
    private int lastSkill=0;//boss释放的上一个技能，确保不要重复
    private int currentBossSkill;
    private bool skillEnd = true;//当前boss技能释放是否完毕
    public EntityMonster()
    {
        entityType = EntityType.Monster;
    }
    //怪物子类重写了这个方法，让怪物在不同地图或同一地图受等级影响属性（考虑到数值问题，暂不实现）
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
        if (this.md.mCfg.mType==MonsterType.Normal)
        {
            NormalMonsterLogic();
        }
        else if (this.md.mCfg.mType==MonsterType.Boss)
        {
            BossLogic();
        }
    }
    private void NormalMonsterLogic()
    {
        //这里待修复为什么怪物出生以后是none状态，没有进入idle状态，暂时的解决办法是判断加入none
        if (currentState == AniState.Idle || currentState == AniState.Walk || currentState == AniState.None)
        {
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
                if (!InAtkRange(false))
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
                    atkTimeCount += delta;
                    atkTimeCount += checkTimeCount;
                    if (atkTimeCount > atkTime)
                    {
                        //达到攻击时间，转向并且攻击
                        SetAtkRotation(dir);
                        
                        Attack(md.mCfg.skillID);
                        atkTimeCount = 0;
                    }
                    else
                    {
                        Idle();
                    }
                    checkTimeCount = 0;
                }
                //让检测时间在0.1~0.5秒内浮动
                checkTime = PETools.RDInt(1, 5) * 1.0f / 10;
            }

        }
    }
    //boss行动逻辑
    private void BossLogic()
    {
        if (currentState == AniState.Idle || currentState == AniState.Walk || currentState == AniState.None)
        {
            float delta = Time.deltaTime;
            checkTimeCount += delta;
            if (checkTimeCount < checkTime)
            {
                return;
            }
            else
            {
                if (skillEnd)
                {
                    BossSkillSelect();
                    skillEnd = false;
                }
                //计算目标方向
                Vector2 dir = CalculateTargetDir();
                if (!InAtkRange(true))
                {
                    //不在范围：设置移动方向，进入移动状态
                    SetDir(dir);
                    Move();
                    //攻击4秒钟还没打出去，直接调用黑气，可以修改地形压迫玩家走位
                    if (checkTimeCount>=6)
                    {
                        SetDir(Vector2.zero);
                        atkTimeCount += delta;
                        atkTimeCount += checkTimeCount;
                        if (atkTimeCount > atkTime)
                        {
                            SetAtkRotation(dir);
                            Attack(bossSkillArray[1]);
                            skillEnd = true;
                            atkTimeCount = 0;
                        }
                        else
                        {//攻击间隔就追踪移动，可能回过于灵敏
                         //贴身的时候要进入idle状态
                            if (DistanceTooClose())
                            {
                                SetDir(dir);
                                Move();
                            }
                            else
                            {
                                Idle();
                            }
                        }
                        checkTimeCount = 0;
                    }
                }
                else
                {
                    //在：停止移动，进行攻击
                    SetDir(Vector2.zero);//这里设置为0，controller就会把状态修改为idle
                                         //判断攻击间隔,移动过程的时间也在累计攻击间隔
                    atkTimeCount += delta;
                    atkTimeCount += checkTimeCount;
                    if (atkTimeCount > atkTime)
                    {
                        //达到攻击时间，转向并且攻击
                        SetAtkRotation(dir);
                        //TODO多个技能需要用出招表来控制，
                        Attack(currentBossSkill);
                        skillEnd = true;
                        atkTimeCount = 0;
                    }
                    else
                    {//攻击间隔就追踪移动，可能回过于灵敏
                        //贴身的时候要进入idle状态
                        if (DistanceTooClose())
                        {
                            Idle();
                        }
                        else
                        {
                            SetDir(dir);
                            Move();
                        }
                    }
                    checkTimeCount = 0;
                }
                //让检测时间在0.1~0.5秒内浮动
                checkTime = PETools.RDInt(1, 5) * 1.0f / 10;
            }

        }
    }
    //boss贴身以后攻击间隔每到，进入idle状态
    private bool DistanceTooClose()
    {
        Vector3 target = battleMg.entitySelfPlayer.GetPos();
        Vector3 self = GetPos();
        target.y = 0;
        self.y = 0;
        float dis = Vector3.Distance(target, self);
        if (dis<=3f)//3米距离就认为是贴身了
        {
            return true;
        }
        return false;
    }
    //boss出招技能
    private void BossSkillSelect()
    {
        int temp;
        while (true)
        {
            temp = Random.Range(0,4);
            if (lastSkill!=temp)
            {
                lastSkill = temp;
                break;
            }
        }
        int skillID = bossSkillArray[temp];

        currentBossSkill = skillID;
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

    private bool InAtkRange(bool isBoss)
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
            if (isBoss)
            {
                currentSkillCfg = ResSvc.instance.GetSkillCfgData(currentBossSkill);
                if (dis <= currentSkillCfg.skillAtkDistance)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (dis <= md.mCfg.atkDis)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
    //boss血量低于90%进入霸体,低于50%狂暴，移速增加100%
    public override void GetUnBreakState()
    {
        if (md.mCfg.mType==MonsterType.Boss)
        {
            if (Hp <= BattleProps.hp * 0.9f)
            {
                if (!unBreakable)
                {
                    unBreakable = true;
                }
                //TODO，霸体特效
                SetFX(Constants.boss_Unbreakable,999f);
                if (Hp <= BattleProps.hp * 0.5f)
                {
                    if (!isCrazy)
                    {
                        isCrazy = true;
                        this.controller.isCrazy = true;
                        this.SetBlend(Constants.blendRun);
                    }
                }
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
