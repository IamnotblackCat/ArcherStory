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
     
    public EntityPlayer entitySelfPlayer;

    private MapConfig mapCfg;
    private AudioSource playerAudioSource;

    private Dictionary<string, EntityMonster> monsterDic = new Dictionary<string, EntityMonster>();
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
        mapCfg = resSvc.GetMapCfgData(mapId);
        resSvc.AsyncLoadScene(mapCfg.sceneName,()=>
        {
            //初始化地图
            GameObject map = GameObject.FindWithTag("MapRoot");
            mapMg = map.GetComponent<MapManager>();
            //在地图管理器里面注入战斗管理器
            mapMg.Init(this);

            Camera.main.transform.position = mapCfg.mainCamPos;
            Camera.main.transform.localEulerAngles = mapCfg.mainCamRote;

            LoadPlayer(mapCfg);
            entitySelfPlayer.Idle();
            //激活第一批怪物
            ActiveCurrentBatchMonster();

            audioSvc.PlayBGMusic(Constants.BGFuben);
        });
    }

    public void Update()
    {
        foreach (var item in monsterDic)
        {
            EntityMonster em = item.Value;
            em.TickAILogic();
        }
    }
    private void LoadPlayer(MapConfig mapData)
    {
        GameObject player = resSvc.LoadPrefab(PathDefine.ArcherBattle);

        player.transform.position = mapData.playerBornPos;
        player.transform.eulerAngles = mapData.playerBornRote;
        player.transform.localScale = Vector3.one;

        PlayerData pd = GameRoot.instance.Playerdata;
        BattleProps props = new BattleProps
        {
            hp = pd.hp,
            attackValue=pd.attackValue,
            defend=pd.defend,
            critical=pd.critical,
        };
        /*载入角色以后，把状态管理器、角色控制器注入到逻辑实体里面，通过逻辑实体的状态管理管理状态，然后在状态管理器里面
        又通过逻辑实体里面持有的角色控制器来控制表现*/
        entitySelfPlayer = new EntityPlayer
        {
            battleMg = this,
            stateMg = this.stateMg,
            skillMg = skillMg,
        };
        entitySelfPlayer.SetBattleProps(props);
        entitySelfPlayer.Name = player.name;
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.Init();
        entitySelfPlayer.SetController(playerController);
        playerAudioSource = entitySelfPlayer.GetAudio();
    }

    public void LoadMonsterByWaveID(int wave)
    {
        for (int i = 0; i < mapCfg.monsterList.Count; i++)
        {
            MonsterData md = mapCfg.monsterList[i];
            if (md.mWave==wave)
            {
                GameObject monsterPrefab = resSvc.LoadPrefab(md.mCfg.resPath,true);
                monsterPrefab.transform.localPosition = md.mBornPos;
                monsterPrefab.transform.localEulerAngles = md.mBornRote;
                monsterPrefab.transform.localScale = Vector3.one;

                monsterPrefab.name = "m" + md.mWave + "_" + md.mIndex;

                EntityMonster em = new EntityMonster
                {
                    battleMg = this,
                    stateMg = stateMg,
                    skillMg = skillMg,
                };
                //设置初始属性
                em.md = md;
                //怪物子类重写了这个方法，让怪物在不同地图受等级影响属性
                em.SetBattleProps(md.mCfg.bps);

                MonsterController mc = monsterPrefab.GetComponent<MonsterController>();
                mc.Init();
                em.SetController(mc);
                em.Name = monsterPrefab.name;

                monsterPrefab.SetActive(false);
                monsterDic.Add(monsterPrefab.name,em);
                //字典一定要存储已经变更过的HP，配置文件的hp没更新的
                GameRoot.instance.dynamicWnd.AddHPItemInfo(em.Name,em.Hp,mc.hpRoot);
            }
        }
    }
    //进入场景延迟生成怪物
    public void ActiveCurrentBatchMonster()
    {
        TimeService.instance.AddTimeTask((int tid) =>
        {
            foreach (var item in monsterDic)
            {
                item.Value.SetCtrlActive();
            }
        },0.5f);
    }
    public List<EntityMonster> GetEntityMonsters()
    {
        List<EntityMonster> monsterList = new List<EntityMonster>();
        foreach (var item in monsterDic)
        {
            monsterList.Add(item.Value);
        }
        return monsterList;
    }

    public void RemoveMonster(string key)
    {
        EntityMonster entityMonster ;
        if (monsterDic.TryGetValue(key,out entityMonster))
        {
            monsterDic.Remove(key);
            GameRoot.instance.dynamicWnd.RemoveHPItemInfo(key);
        }
    }
    #region 技能释放与角色控制
    //战斗场景角色控制
    public void SetSelfPlayerMoveDir(Vector2 dir)
    {
        if (!entitySelfPlayer.canControll)
        {
            return;
        }
        if (dir == Vector2.zero)
        {
            entitySelfPlayer.Idle();
        }
        else
        {
            entitySelfPlayer.Move();
            //Debug.Log(dir);
            entitySelfPlayer.SetDir(dir);
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
        audioSvc.PlayCharacterAudio(Constants.skill1,playerAudioSource);
        entitySelfPlayer.Attack(101);
    }
    private void ReleaseSkill2()
    {
        audioSvc.PlayCharacterAudio(Constants.skill2, playerAudioSource);
        entitySelfPlayer.Attack(102);
    }
    private void ReleaseSkill3()
    {//这里使用UI播放音效是因为这个音效会和其他音效共存，避免音效被打断
        audioSvc.PlayUIAudio(Constants.skill3);
        entitySelfPlayer.Attack(103);
    }
    private void ReleaseSkill4()
    {//这里使用UI播放音效是因为这个音效会和其他音效共存，避免音效被打断
        audioSvc.PlayUIAudio(Constants.skill4);
        entitySelfPlayer.Attack(104);
    }
    private void ReleaseSkill5()
    {
        audioSvc.PlayCharacterAudio(Constants.skill5, playerAudioSource);
        entitySelfPlayer.Attack(105);
    }
    private void ReleaseSkill6()
    {
        //这里使用UI播放音效是因为这个音效会和其他音效共存，长时间的火焰音效
        //这行代码有一个神奇的BUG，第一次使用会转两圈。重启数次，并且尝试了一切方法，完全找不到原因
        //通过改名修复的，所以名字就它一个是大写.
        //新发现：这是一个特别神奇的bug，改名字就能好一段时间，然后又出来，改名又好
        audioSvc.PlayUIAudio(Constants.skill6);
        //audioSvc.PlayCharacterAudio(Constants.Skill6,playerAudioSource);
        entitySelfPlayer.Attack(106);
    }
    private void ReleaseSkill7()
    {
        audioSvc.PlayCharacterAudio(Constants.skill7, playerAudioSource);
        entitySelfPlayer.Attack(107);
    }
    private void ReleaseSkill8()
    {
        audioSvc.PlayCharacterAudio(Constants.skill8, playerAudioSource);
        entitySelfPlayer.Attack(108);
    }
} 
#endregion
