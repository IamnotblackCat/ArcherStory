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
    private EntityPlayer entityPlayer;

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
        GameObject player = resSvc.LoadPrefab(PathDefine.ArcherBattle);

        player.transform.position = mapData.playerBornPos;
        player.transform.eulerAngles = mapData.playerBornRote;
        player.transform.localScale = Vector3.one;
        //载入角色以后，把状态管理器、角色控制器注入到逻辑实体里面，通过逻辑实体的状态管理管理状态，然后在状态管理器里面调整角色控制器来表现
        entityPlayer = new EntityPlayer
        {
            stateMg = this.stateMg
        };
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.Init();
        entityPlayer.controller = playerController;
    }

    //战斗场景角色控制
    public void SetSelfPlayerMoveDir(Vector2 dir)
    {
        if (dir==Vector2.zero)
        {
            entityPlayer.Idle();
        }
        else
        {
            entityPlayer.Move();
        }
    }
    public void ReleaseSkill(int index)
    {
        switch (index)
        {
            case 1:
                ReleaseNormalATK();
                break;
            case 2:
                ReleaseSkill2();
                break;
            case 3:
                ReleaseSkill3();
                break;
            case 4:
                ReleaseSkill4();
                break;
            case 5:
                ReleaseSkill5();
                break;
            case 6:
                ReleaseSkill6();
                break;
            case 7:
                ReleaseSkill7();
                break;
            case 8:
                ReleaseSkill8();
                break;
            default:
                break;
        }
    }
    private void ReleaseNormalATK()
    {

    }
    private void ReleaseSkill2()
    {

    }
    private void ReleaseSkill3()
    {

    }
    private void ReleaseSkill4()
    {

    }
    private void ReleaseSkill5()
    {

    }
    private void ReleaseSkill6()
    {

    }
    private void ReleaseSkill7()
    {

    }
    private void ReleaseSkill8()
    {

    }
}
