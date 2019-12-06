/****************************************************
	文件：EntityBase.cs
	作者：Echo
	邮箱: 350383921@qq.com
	日期：2019/11/28 15:40   	
	功能：逻辑实体基类，实现的主体
*****************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class EntityBase
{
    public AniState currentState = AniState.None;

    public BattleManager battleMg = null;
    public StateManager stateMg = null;
    public Controller controller = null;
    public SkillManager skillMg = null;

    public bool canControll = true;

    private BattleProps battleProps;
    //血量和攻击力在战斗过程中会变
    private int hp;
    private int attackValue;
    public BattleProps BattleProps
    {
        get
        {
            return battleProps;
        }

        protected set
        {
            battleProps = value;
        }
    }

    public int Hp
    {
        get
        {
            return hp;
        }

       set
        {
            //Debug.Log("血量减少："+hp+"/"+value);
            hp = value;
        }
    }

    public int AttackValue
    {
        get
        {
            return attackValue;
        }

        set
        {
            attackValue = value;
        }
    }

    public void Idle()
    {
        stateMg.ChangeState(this, AniState.Idle, null);
    }
    public void Move()
    {
        stateMg.ChangeState(this, AniState.Run, null);
    }
    public void Attack(int skillID)
    {
        stateMg.ChangeState(this, AniState.Attack, skillID);
    }
    public void Die()
    {
        stateMg.ChangeState(this,AniState.Die,null);
    }
    public void Wound()
    {
        stateMg.ChangeState(this,AniState.Wound,null);
    }
    //设置战斗过程中的属性
    public virtual void SetBattleProps(BattleProps props)
    {
        Hp = props.hp;
        attackValue = props.attackValue;
        BattleProps = props;
    }
    public virtual void SetBlend(float blend)
    {
        if (controller != null)
        {
            controller.SetBlend(blend);
        }
    }
    public virtual void SetDir(Vector2 dir)
    {
        if (controller != null)
        {
            controller.Dir = dir;
        }
    }
    public virtual void SetAction(int act)
    {
        if (controller != null)
        {
            controller.SetAction(act);
        }
    }
    public virtual void SetFX(string fxName,float closeTime)
    {
        if (controller!=null)
        {
            controller.SetFX(fxName,closeTime);
        }
    }
    public virtual void SkillAttack(int skillID)
    {
        skillMg.SkillAttack(this,skillID);
    }
    public virtual void SetSkillMoveState(bool move,float skillSpeed=0f)
    {
        if (controller!=null)
        {

            controller.SetSkillMoveState(move, skillSpeed);
        }
    }
    public virtual void SetCritical(int critical)
    {
        GameRoot.instance.dynamicWnd.SetCritical(controller.gameObject.name,critical);
    }
    public virtual void SetHurt(int hurt)
    {
        GameRoot.instance.dynamicWnd.SetHurt(controller.gameObject.name,hurt);
    }
    public virtual Vector3 GetPos()
    {
        return controller.transform.position;
    }
    public virtual Transform GetTrans()
    {
        return controller.transform;
    }
}
