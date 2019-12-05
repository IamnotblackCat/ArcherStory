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
    public void Init(BattleManager battle)
    {
        battleMg = battle;
        //实例化第一批怪物
        battleMg.LoadMonsterByWaveID(waveIndex);
    }
}
