/****************************************************
	文件：BattleManager.cs
	作者：Echo
	邮箱: 350383921@qq.com
	日期：2019/11/27 16:22   	
	功能：战斗管理
*****************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BattleManager:MonoBehaviour
{
    private ResSvc resSvc;
    private AudioSvc audioSvc;

    private StateManager stateMg;
    private SkillManager skillMg;
    private MapManager mapMg;

    public void Init(int mapId)
    {
        resSvc = ResSvc.instance;
        audioSvc = AudioSvc.instance;
        //初始化管理器
        stateMg = gameObject.AddComponent<StateManager>();
        stateMg.Init();
        skillMg = gameObject.AddComponent<SkillManager>();
        skillMg.Init();

        //加载地图
        MapConfig mapData = resSvc.GetMapCfgData(mapId);
        resSvc.AsyncLoadScene(mapData.sceneName,()=>
        {
            //初始化地图
            GameObject map = GameObject.FindWithTag("MapRoot");
            mapMg = map.GetComponent<MapManager>();
            mapMg.Init();

            Camera.main.transform.position = mapData.mainCamPos;
            Camera.main.transform.localEulerAngles = mapData.mainCamRote;

            LoadPlayer(mapData);

            audioSvc.PlayBGMusic(Constants.BGFuben);
        });
    }

    private void LoadPlayer(MapConfig mapData)
    {

    }
}
