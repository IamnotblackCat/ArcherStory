/****************************************************
    文件：TriggerData.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/12/27 16:58:58
	功能：地图触发数据类
*****************************************************/

using UnityEngine;

public class TriggerData : MonoBehaviour 
{
    public int triggerWave;
    public bool isBossLevel;
    public BoxCollider boxCollider;
    public MapManager mapMgr;
    public GameObject timeLine;
    private bool isDone = false;//是否已经触发过
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player"&&!isDone)
        {
            if (mapMgr != null)
            {
                mapMgr.TriggerMonsterBorn(boxCollider, triggerWave);
                isDone = true;
                if (isBossLevel)
                {
                    if (!timeLine.activeSelf)
                    {
                        timeLine.SetActive(true);
                    }
                    //暂时禁用黑白无常头顶的血条，2.5秒后恢复显示
                    GameRoot.instance.dynamicWnd.SetWndState(false);
                    TimeService.instance.AddTimeTask((int tid)=>
                    { GameRoot.instance.dynamicWnd.SetWndState();},2.5f);
                    AudioSvc.instance.PlayBGMusic(Constants.BGBoss);
                }
            }
        }
    }
}