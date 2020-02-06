/****************************************************
    文件：GuideWnd.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/11/1 16:21:17
	功能：Nothing
*****************************************************/

//using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class GuideWnd : WindowRoot
{
    public Text txtName;
    public Text txtDialog;
    public Image imgIcon;
    public Button btnNext;

    private PlayerData pd;
    private AutoGuideCfg currentTaskData;
    private string[] dialogArr;
    private int index;

    protected override void InitWnd()
    {
        base.InitWnd();
        currentTaskData = MainCitySys.Instance.GetGuideData();
        pd = GameRoot.instance.Playerdata;
        dialogArr = currentTaskData.dilogArr.Split('#');
        index = 1;

        SetTalk();
    }

    public void SetTalk()
    {
        string[] taskTalk = dialogArr[index].Split('|');
        if (taskTalk[0] == "0")
        {
            //自己
            //SetSprite(imgIcon, PathDefine.SelfIcon);
            if (imgIcon.enabled)
            {
                imgIcon.enabled = false;
            }
            SetText(txtName, "");
        }
        else
        {
            //npc
            if (!imgIcon.enabled)
            {
                imgIcon.enabled = true;
            }
            switch (currentTaskData.npcID)
            {
                case 0:
                    SetSprite(imgIcon, PathDefine.WiseIcon);
                    SetText(txtName, "智者");
                    break;
                case 1:
                    SetSprite(imgIcon, PathDefine.GeneralIcon);
                    SetText(txtName, "将军");
                    break;
                case 2:
                    SetSprite(imgIcon, PathDefine.ArtisanIcon);
                    SetText(txtName, "掌柜");
                    break;
                case 3:
                    SetSprite(imgIcon, PathDefine.TraderIcon);
                    SetText(txtName, "锻造商人");
                    break;
                default:
                    SetSprite(imgIcon, PathDefine.GuideIcon);
                    SetText(txtName, "小芸");
                    break;
            }
        }
        //图片大小不一致
        imgIcon.SetNativeSize();
        //把对话中提到的玩家名字显示出对应的玩家名字
        SetText(txtDialog, taskTalk[1]);
    }
    public void ClickNextBtn()
    {
        audioSvc.PlayUIAudio(Constants.uiClick);
        index++;
        if (index == dialogArr.Length)
        {
            //GameMsg msg = new GameMsg
            //{
            //    cmd = (int)CMD.ReqGuide,
            //    reqGuide = new ReqGuide
            //    {
            //        guidid = currentTaskData.ID
            //    }
            //};
            //netService.SendMsg(msg);
            MainCitySys.Instance.RspondGuide();
            SetWndState(false);
        }
        else
        {
            SetTalk();

        }
    }
}