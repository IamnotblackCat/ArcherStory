/****************************************************
    文件：NetService.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/10/17 17:6:3
	功能：网络服务
*****************************************************/

using PENet;
using PEProtocol;
using System.Collections.Generic;
using UnityEngine;

public class NetService : MonoBehaviour 
{
    public static NetService instance = null;

    private static string obj = "lock";
    PESocket<ClientSession, GameMsg> client = null;
    private Queue<GameMsg> msgQue = new Queue<GameMsg>();

    public void InitSvc()
    {
        instance = this;
        client = new PESocket<ClientSession, GameMsg>();
        //根据回调的不同等级
        client.SetLog(true, (string msg, int lv) =>
        {
            switch (lv)
            {
                case 0:
                    msg = "Log" + msg;
                    Debug.Log(msg);
                    break;
                case 1:
                    msg = "LogWarning" + msg;
                    Debug.LogWarning(msg);
                    break;
                case 2:
                    msg = "LogError" + msg;
                    Debug.LogError(msg);
                    break;
                case 3:
                    msg = "LogInfo" + msg;
                    Debug.Log(msg);
                    break;
                default:
                    break;
            }
        });
        client.StartAsClient(ServiceConfig.srvIP, ServiceConfig.srvPort);
        PECommon.Log("Init NetServece");
    }
    public void SendMsg(GameMsg msg)
    {
        if (client.session!=null)
        {
            client.session.SendMsg(msg);
        }
        else
        {
            GameRoot.instance.AddTips("服务器未连接");
            //重新初始化，尝试再连接
            InitSvc();
        }
    }
    public void AddNetPack(GameMsg msg)
    {
        lock (obj)
        {
            msgQue.Enqueue(msg);
        }
    }
    private void Update()
    {
        if (msgQue.Count>0)
        {
            lock (obj)
            {
                GameMsg msg = msgQue.Dequeue();
                ProcessMsg(msg);
            }
        }
    }
    private void ProcessMsg(GameMsg msg)
    {
        if (msg.err!=(int)ErroroCode.None)
        {
            switch ((ErroroCode)msg.err)
            {
                case ErroroCode.ServerDataError:
                    PECommon.Log("服务端数据异常", LogType.Error);
                    GameRoot.instance.AddTips("客户端数据");
                    break;
                case ErroroCode.UpdateDBError:
                    PECommon.Log("数据库更新异常",LogType.Error);
                    GameRoot.instance.AddTips("网络不稳定");
                    break;
                case ErroroCode.AcctIsOnline:
                    GameRoot.instance.AddTips("当前账号已在线");
                    break;
                case ErroroCode.WrongPass:
                    GameRoot.instance.AddTips("密码错误");
                    break;
                case ErroroCode.lackLv:
                    GameRoot.instance.AddTips("等级不足");
                    break;
                case ErroroCode.lackCoin:
                    GameRoot.instance.AddTips("金币不足");
                    break;
                case ErroroCode.lackCrystal:
                    GameRoot.instance.AddTips("水晶不足");
                    break;
                default:
                    break;
            }
            return;
        }
        switch ((CMD)msg.cmd)
        {
            case CMD.ResponLogin://去登陆系统处理服务器回应的消息
                LoginSys.instance.RspLogin(msg);
                break;
            case CMD.RespondRename:
                LoginSys.instance.RspRename(msg);
                break;
            case CMD.RspGuide:
                MainCitySys.Instance.RspondGuide(msg);
                break;
            case CMD.ResStrong:
                MainCitySys.Instance.RspStrong(msg);
                break;
            default:
                break;
        }
    }
}