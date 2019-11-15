/****************************************************
    文件：SystemRoot.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/10/13 15:40:6
	功能：业务系统基类
*****************************************************/

using UnityEngine;

public class SystemRoot : MonoBehaviour 
{
    protected ResSvc resSvc;
    protected AudioSvc audioSvc;
    protected NetService netService;

    public virtual void InitSys()
    {
        resSvc = ResSvc.instance;
        audioSvc = AudioSvc.instance;
        netService = NetService.instance;
    }
}