/****************************************************
	文件：MonsterController.cs
	作者：Echo
	邮箱: 350383921@qq.com
	日期：2019/12/04 15:08   	
	功能：怪物表现实体控制器
*****************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MonsterController:Controller
{

    public GameObject unBreakableFX;
    public GameObject boss_Skill2Begin;
    public GameObject boss_Skill3Ground;
    public GameObject boss_Skill4FX;

    private Dictionary<string, GameObject> bossFXDic = new Dictionary<string, GameObject>();
    public override void Init()
    {
        base.Init();
        if (unBreakableFX!=null)
        {
            bossFXDic.Add(unBreakableFX.name,unBreakableFX);
            bossFXDic.Add(boss_Skill2Begin.name,boss_Skill2Begin);
            bossFXDic.Add(boss_Skill3Ground.name,boss_Skill3Ground);
            bossFXDic.Add(boss_Skill4FX.name,boss_Skill4FX);
        }
        
    }
    private void Update()
    {
        if (isMove)
        {
            SetDir();
            SetMove();
        }
        if (skillMove)
        {
            SetSkillMove();
        }
    }
    private void SetDir()
    {
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1));
        Vector3 eulerAngle = new Vector3(0, angle, 0);
        transform.eulerAngles = eulerAngle;
    }
    private void SetSkillMove()
    {
        ctrl.Move(transform.forward * Time.deltaTime * skillMoveSpeed);
    }
    private void SetMove()
    {
        if (!isCrazy)
        {
            ctrl.Move(transform.forward * Time.deltaTime * Constants.monsterMoveSpeed);
        }
        else
        {
            ctrl.Move(transform.forward * Time.deltaTime * Constants.BossCrazyMoveSpeed);
        }
        ctrl.Move(Vector3.down * Time.deltaTime * Constants.monsterMoveSpeed);
    }
    //设置boss技能特效的显示
    public override void SetFX(string fxName, float closeTime,float delayTime)
    {
        if (fxName==null)//没有特效名字的就不执行
        {
            return;
        }
        GameObject go;
        if (bossFXDic.TryGetValue(fxName, out go))
        {
            if (delayTime!=0)
            {
                timeSvc.AddTimeTask((int tid)=>
                {
                    go.SetActive(true);
                },delayTime);
            }
            else
            {
                go.SetActive(true);
            }
            timeSvc.AddTimeTask((int tid) =>
            {
                go.SetActive(false);
            }, closeTime);
        }
    }
    public override void SetLoopFX(string fxName,Vector3 pos)
    {
        if (fxName == null)//没有特效名字的就不执行
        {
            return;
        }
        GameObject go=resSvc.LoadPrefab(PathDefine.boss_SkillLoop);
        go.transform.position = pos;
    }
}
