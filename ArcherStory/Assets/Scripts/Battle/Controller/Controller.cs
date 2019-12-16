/****************************************************
	文件：Controller.cs
	作者：Echo
	邮箱: 350383921@qq.com
	日期：2019/11/28 16:12   	
	功能：控制器抽象基类
*****************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class Controller:MonoBehaviour
{
    protected ResSvc resSvc = null;

    protected bool isMove;
    protected bool isBlinkSkill;
    protected TimeService timeSvc;
    protected Transform camMainTrans;

    private Vector2 dir = Vector2.zero;

    public Animator anim;
    public CharacterController ctrl;
    public Vector2 Dir
    {
        get
        {
            return dir;
        }
        set
        {
            dir = value;
            if (value == Vector2.zero)
            {
                isMove = false;
            }
            else
            {
                isMove = true;
            }
        }
    }
    public Transform hpRoot;

    protected bool skillMove = false;
    protected float skillMoveSpeed = 0f;

    protected Dictionary<string, GameObject> fxDic = new Dictionary<string, GameObject>();

    public virtual void Init()
    {
        resSvc = ResSvc.instance;
        timeSvc = TimeService.instance;
    }

    public virtual void SetBlend(float blend)
    {
        anim.SetFloat("Blend",blend);
    }
    public virtual void SetAction(int act)
    {
        anim.SetInteger("Action",act);
        //Debug.Log(anim.GetInteger("Action"));
    }
    public virtual void SetFX(string fxName ,float closeTime)
    {
        
    }
    public virtual void SetAreaSkillFX(string fxName,float beginTime,float closeTime)
    {

    }
    public void SetSkillMoveState(bool move,bool isBlink,float skillSpeed=0f)
    {
        skillMove = move;
        isBlinkSkill = isBlink;
        skillMoveSpeed = skillSpeed;
    }
    //不加上摄像机偏转
    public virtual void SetAtkRotationLocal(Vector2 dir)
    {
        float angle = Vector2.SignedAngle(dir, new Vector2(0, 1));
        Vector3 eulerAngle = new Vector3(0, angle, 0);
        transform.eulerAngles = eulerAngle;
    }
    //加上了摄像机偏转
    public virtual void SetAtkRotationCam(Vector2 dir)
    {
        float angle = Vector2.SignedAngle(dir, new Vector2(0, 1)) + camMainTrans.eulerAngles.y;
        Vector3 eulerAngle = new Vector3(0, angle, 0);
        transform.eulerAngles = eulerAngle;
    }

}
