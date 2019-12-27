/****************************************************
	文件：MapManager.cs
	作者：Echo
	邮箱: 350383921@qq.com
	日期：2019/11/27 16:25   	
	功能：地图管理器
*****************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class MapManager:MonoBehaviour
{
    private int waveIndex = 1;//默认生成第一批怪物
    private BattleManager battleMg;

    public TriggerData[] triggerArray;
    public void Init(BattleManager battle)
    {
        battleMg = battle;
        //实例化第一批怪物
        battleMg.LoadMonsterByWaveID(waveIndex);
    }
    public void TriggerMonsterBorn(TriggerData trigger,int waveIndex)
    {
        if (battleMg!=null)
        {
            BoxCollider collider = trigger.gameObject.GetComponent<BoxCollider>();
            collider.isTrigger = false;

            battleMg.LoadMonsterByWaveID(waveIndex);
            battleMg.ActiveCurrentBatchMonster();
            battleMg.triggerCheck = true;
        }
    }
    public bool SetNextTriggerOn()
    {
        waveIndex += 1;
        for (int i = 0; i <triggerArray.Length ; i++)
        {
            if (triggerArray[i].triggerWave==waveIndex)
            {
                BoxCollider co = triggerArray[i].GetComponent<BoxCollider>();
                co.isTrigger = true;
                return true;
            }
        }
        return false;
    }
}
