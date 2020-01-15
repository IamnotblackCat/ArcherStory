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
    public GameObject boss_Crazy;
    public GameObject boss_sifangzhan;

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
            bossFXDic.Add(boss_Crazy.name,boss_Crazy);
            bossFXDic.Add(boss_sifangzhan.name,boss_sifangzhan);
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
            if (isBossSkillMove)
            {
                SetBossSpecialSkillMove(Constants.sifangzhanPos);
            }
            else
            {
                SetSkillMove();
            }
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
    //四方斩技能，移动到固定位置
    private void SetBossSpecialSkillMove(Vector3 pos)
    {
        Vector3 dir = (pos - transform.position).normalized;
        ctrl.Move(dir*Time.deltaTime*skillMoveSpeed);
    }
    private void SetMove()
    {
        if (!bossSpeedUp)
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
