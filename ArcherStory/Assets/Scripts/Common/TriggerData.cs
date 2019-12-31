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
    public MapManager mapMgr;

    public void OnTriggerExit(Collider other)
    {
        if (other.tag=="Player")
        {
            if (mapMgr!=null)
            {
                mapMgr.TriggerMonsterBorn(this,triggerWave);
                if (isBossLevel)
                {
                    AudioSvc.instance.PlayBGMusic(Constants.BGBoss);
                }
            }
        }
    }
}