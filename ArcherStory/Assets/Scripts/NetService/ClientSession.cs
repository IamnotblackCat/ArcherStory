/****************************************************
    文件：ClientSession.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/10/17 17:11:59
	功能：客户端会话需要用到的部分
*****************************************************/

using PENet;
using PEProtocol;
using UnityEngine;

public class ClientSession : PESession<GameMsg>
{
    protected override void OnConnected()
    {
        GameRoot.instance.AddTips("服务器连接成功");
        PECommon.Log("Server is connected");
    }
    protected override void OnReciveMsg(GameMsg msg)
    {
        PECommon.Log("Recive CMD: "+ ((CMD)msg.cmd).ToString());
        NetService.instance.AddNetPack(msg);
    }
    protected override void OnDisConnected()
    {
        GameRoot.instance.AddTips("服务器已经断开连接");
        PECommon.Log("Server is disConnected");
    }
}