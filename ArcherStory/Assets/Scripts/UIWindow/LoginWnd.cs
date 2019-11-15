/****************************************************
    文件：LoginWnd.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/10/12 19:2:2
	功能：登陆界面功能
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class LoginWnd : WindowRoot 
{
    public Button btnNotice;
    public Button btnEnterGame;
    public InputField iptAccount;
    public InputField iptPassword;

    protected override void InitWnd()
    {
        base.InitWnd();
        //本地存储的账号密码
        if (PlayerPrefs.HasKey("acct") &&PlayerPrefs.HasKey("pass"))
        {
            iptAccount.text = PlayerPrefs.GetString("acct");
            iptPassword.text = PlayerPrefs.GetString("pass");
        }
        else
        {
            iptAccount.text = "";
            iptPassword.text = "";
        }
    }
    //登陆按钮
    public void ClickEnterGame()
    {
        audioSvc.PlayUIAudio(Constants.uiLogin);
        string _acct= iptAccount.text;
        string _pass = iptPassword.text;
        if (_acct!=""&&_pass!="")
        {
            //存储账号密码
            PlayerPrefs.SetString("acct",_acct);
            PlayerPrefs.SetString("pass",_pass);

            //发送网络消息，请求登陆
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.RequestLogin,
                reqLogin = new RequestLogin
                {
                    acct = _acct,
                    pass=_pass
                }
            };
            netService.SendMsg(msg);
            
        }
        else
        {
            GameRoot.instance.AddTips("帐号或者密码为空");
        }
    }
    public void ClickNoticeBtn()
    {
        GameRoot.instance.AddTips("功能正在开发中。。。");
        audioSvc.PlayUIAudio(Constants.uiClick);
    }
}