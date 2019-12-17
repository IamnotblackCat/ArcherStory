/****************************************************
	文件：EntityPlayer.cs
	作者：Echo
	邮箱: 350383921@qq.com
	日期：2019/11/28 16:00   	
	功能：玩家逻辑实体类
*****************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class EntityPlayer:EntityBase
{
    public EntityPlayer()
    {
        entityType = EntityType.Player;
    }
    public override Vector2 CalculateTargetDir()
    {
        EntityMonster monster = FindClosedMonster();
        if (monster==null)
        {
            return Vector2.zero;
        }
        Vector3 self = GetPos();
        Vector3 target = monster.GetPos();
        Vector2 dir = new Vector2(target.x-self.x,target.z-self.z);
        dir = dir.normalized;
        return dir;
    }
    public EntityMonster FindClosedMonster()
    {
        List<EntityMonster> list = battleMg.GetEntityMonsters();
        if (list==null||list.Count==0)
        {
            return null;
        }
        Vector3 self = GetPos();
        EntityMonster targetMonster = null;
        float dis = 0;
        for (int i = 0; i < list.Count; i++)
        {
            Vector3 target = list[i].GetPos();
            if (i==0)
            {
                dis = Vector3.Distance(self,target);
                targetMonster = list[0];
            }
            else
            {
                float tempPos = Vector3.Distance(self,target);
                if (tempPos<dis)
                {
                    dis = tempPos;
                    targetMonster = list[i];
                }
            }
        }
        return targetMonster;
    }
    public override Vector2 AreaSkillTargetDir()
    {
        Vector3 self = GetPos();
        Vector3 target= BattleSys.Instance.playerCtrlWnd.pos;
        Vector2 dir = new Vector2(target.x-self.x,target.z-self.z);
        return dir;
    }
    public override void SetHPVal(int oldVal, int newVal)
    {
        BattleSys.Instance.playerCtrlWnd.SetSelfHPBarVal(newVal);
    }
}
