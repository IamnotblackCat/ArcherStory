/****************************************************
    文件：LoginSys.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/10/12 14:58:57
	功能：登陆注册业务系统
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginSys : SystemRoot 
{
    public static LoginSys instance = null;

    public LoginWnd loginWnd;
    public CreateWnd createWnd;
    public override void InitSys()
    {
        base.InitSys();
        instance = this;
        PECommon.Log("初始化登陆系统完成");
    }
    //进入登陆场景
    public void EnterLogin()
    {
        //异步加载登陆场景——委托如果读条完成，显示登陆界面
        //显示加载进度
        resSvc.AsyncLoadScene(Constants.SceneLogin,()=>
        {
            //加载完成以后打开登陆界面
            loginWnd.SetWndState();
            audioSvc.PlayBGMusic(Constants.BGLogin);
            //loginWnd.gameObject.SetActive(true);
            //loginWnd.in();
        });
    }
    public void RspLogin(GameMsg msg)
    {
        GameRoot.instance.AddTips("登陆成功");
        GameRoot.instance.SetPlayerData(msg.respLogin);
        if (msg.respLogin.playerData.name=="")
        {//打开角色创建面板
            createWnd.SetWndState();
        }
        else
        {
            MainCitySys.Instance.EnterMainCity();
        }
        loginWnd.SetWndState(false);
    }
    public void RspRename(GameMsg msg)
    {
        GameRoot.instance.SetPlayerName(msg.respondRename.name);

        //跳转场景进入主城，打开主城界面
        MainCitySys.Instance.EnterMainCity();
        //关闭创建面板
        createWnd.SetWndState(false);
    }
}