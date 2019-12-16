/****************************************************
    文件：skill1ATK.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/12/16 16:39:45
	功能：普通攻击跟踪目标
*****************************************************/

using UnityEngine;

public class skill1ATK : MonoBehaviour 
{
    private float speed = 100f;
    private bool canFly = false;
    private bool isHaveTarget = true;
    private Vector3 monsterPos;
    private Vector3 beginPos;
    private Quaternion beginRot;

    private void Awake()
    {//射完以后要返回初始位置
        beginPos = transform.localPosition;
        beginRot = transform.localRotation;
    }
    private void OnEnable()
    {
        EntityMonster monster = BattleSys.Instance.battleMg.entitySelfPlayer.FindClosedMonster();
        //拿到当前攻击目标的位置，位置稍微上调一点，不然是射脚的
        //没有目标的时候，箭就向前飞
        Vector3 pos;
        if (monster==null)
        {
            isHaveTarget = false;
            pos = Vector3.zero;
        }
        else
        {
            isHaveTarget = true;
            pos = monster.GetPos();
        }
        monsterPos = new Vector3(pos.x,pos.y+2f,pos.z);

        TimeService.instance.AddTimeTask((int tid)=>
        {
            canFly = true;
        },0.6f);
        TimeService.instance.AddTimeTask((int tid)=>
        {
            canFly = false;
            transform.localPosition = beginPos;
            transform.localRotation = beginRot;
        },1.2f);
    }
    private void Update()
    {
        if (canFly)
        {
            if (isHaveTarget)
            {
                //弧线飞行是因为父物体弓的位置发生了变化
                transform.position = Vector3.MoveTowards(transform.position,monsterPos,speed*Time.deltaTime);

            }
            else
            {
                transform.Translate(Vector3.forward*speed*Time.deltaTime);
            }
        }
    }
}