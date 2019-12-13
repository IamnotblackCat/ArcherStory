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
    public SkillManager skillMg = null;

    public bool canControll = true;

    protected Controller controller = null;

    private BattleProps battleProps;
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
    //血量和攻击力在战斗过程中会变
    private int hp;
    public int Hp
    {
        get
        {
            return hp;
        }

       set
        {
            //Debug.Log("血量减少："+hp+"/"+value);
            SetHPVal(hp,value);
            hp = value;
        }
    }
    private int attackValue;
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
    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }


    private string name;
    public void SetController(Controller ctrl)
    {
        if (ctrl!=null)
        {

            controller = ctrl;
        }
    }
    public void SetCtrlActive(bool active = true)
    {
        controller.gameObject.SetActive(active);
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
    public virtual void SetAreaSkillFX(string fxName,float beginTime,float closeTime)
    {
        if (controller!=null)
        {
            controller.SetAreaSkillFX(fxName,beginTime,closeTime);
        }
    }
    public virtual void SkillAttack(int skillID)
    {
        skillMg.SkillAttack(this,skillID);
    }
    //bool参数表示，是否需要摄像机偏转
    public virtual void SetAtkRotation(Vector2 dir,bool offset=false)
    {
        if (controller!=null)
        {
            if (offset)
            {
                controller.SetAtkRotationCam(dir);
            }
            else
            {
                controller.SetAtkRotationLocal(dir);
            }
        }
    }
    public virtual void SetSkillMoveState(bool move,bool isBlink=true,float skillSpeed=0f)
    {
        if (controller!=null)
        {

            controller.SetSkillMoveState(move,isBlink,skillSpeed);
        }
    }
    public virtual void SetCritical(int critical)
    {
        if (controller != null)
        {

            GameRoot.instance.dynamicWnd.SetCritical(Name, critical);
        }
    }
    public virtual void SetHurt(int hurt)
    {
        if (controller != null)
        {

            GameRoot.instance.dynamicWnd.SetHurt(Name, hurt);
        }
    }
    public virtual void SetHPVal(int oldVal, int newVal)
    {
        if (controller != null)
        {

            GameRoot.instance.dynamicWnd.SetHPVal(Name, oldVal, newVal);
        }
    }
    public virtual Vector3 GetPos()
    {
        if (controller!=null)
        {
        return controller.transform.position;
        }
        Debug.Log("无法获取位置");
        return Vector3.zero;
    }
    public virtual Transform GetTrans()
    {
        return controller.transform;
    }
    public AnimationClip[] GetAniClips()
    {
        if (controller!=null)
        {
            return controller.anim.runtimeAnimatorController.animationClips;
        }
        return null;
    }

    public virtual Vector2 CalculateTargetDir()
    {
        return Vector2.zero;
    }
    public virtual Vector2 AreaSkillTargetDir()
    {
        return Vector2.zero;
    }
}
