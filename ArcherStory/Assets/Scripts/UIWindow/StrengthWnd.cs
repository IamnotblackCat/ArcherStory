/****************************************************
    文件：StrengthWnd.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/11/3 11:29:44
	功能：强化装备界面
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StrengthWnd : WindowRoot 
{
    #region UI拖拽
    public Transform[] posBtnTrans;

    public Image imgPos;//强化部位
    public Transform starTransGroup;
    public Transform conditionTrans;

    public Text txtStarLv;
    public Text txtHP1;
    public Text txtHP2;
    public Text txtDamage1;
    public Text txtDamage2;
    public Text txtDef1;
    public Text txtDef2;
    public Image imgArrow1;
    public Image imgArrow2;
    public Image imgArrow3;

    public Text txtNeedLv;
    public Text txtCoin;
    public Text txtCostCoin;
    public Text txtCrystal;
    #endregion
    
    //private Image[] imgs = new Image[6];
    private int currentIndex;
    private PlayerData pd;
    private StrengthCfg nextSc;

    protected override void InitWnd()
    {
        base.InitWnd();
        pd = GameRoot.instance.Playerdata;
        RegClickEvts();
        ClickPosItem(0);
    }
    private void RegClickEvts()
    {
        for (int i = 0; i < posBtnTrans.Length; i++)
        {
            Image img = posBtnTrans[i].GetComponent<Image>();
            //Transform trans = posBtnTrans[i].transform;

            OnClick(img.gameObject, (object obj) =>
             {
                 audioSvc.PlayUIAudio(Constants.uiClick);
                 ClickPosItem((int)obj);
             },i);
        }
    }
    private void ClickPosItem(int index)
    {
        PECommon.Log("Click Item: "+index);
        currentIndex = index;
        for (int i = 0; i < posBtnTrans.Length; i++)
        {
            if (i==currentIndex)
            {
                if (!posBtnTrans[i].parent.GetChild(0).gameObject.activeSelf)
                {
                    //Debug.Log(posBtnTrans[i].parent.GetChild(0).gameObject.name);
                    posBtnTrans[i].parent.GetChild(0).gameObject.SetActive(true);
                }
            }
            else
            {
                if (posBtnTrans[i].parent.GetChild(0).gameObject.activeSelf)
                {
                    posBtnTrans[i].parent.GetChild(0).gameObject.SetActive(false);

                }
            }
        }
        //RefreshItem();
    }
    //private void RefreshItem()
    //{
    //    SetText(txtCoin,pd.coin);

    //    switch (currentIndex)
    //    {
    //        case 0:
    //            SetSprite(imgPos, PathDefine.ItemHead);
    //            break;
    //        case 1:
    //            SetSprite(imgPos, PathDefine.ItemBody);
    //            break;
    //        case 2:
    //            SetSprite(imgPos, PathDefine.ItemWaist);
    //            break;
    //        case 3:
    //            SetSprite(imgPos, PathDefine.ItemHand);
    //            break;
    //        case 4:
    //            SetSprite(imgPos, PathDefine.ItemLeg);
    //            break;
    //        case 5:
    //            SetSprite(imgPos, PathDefine.ItemFoot);
    //            break;
    //        default:
    //            break;
    //    }
    //    int currentStarLv = pd.strengthArray[currentIndex];
    //    int nextStarlv = currentStarLv + 1;

    //    SetText(txtStarLv,currentStarLv+"星级");//设置当前部位星级
    //    for (int i = 0; i < starTransGroup.childCount; i++)
    //    {
    //        Image img = starTransGroup.GetChild(i).GetComponent<Image>();
    //        if (currentStarLv <= i)
    //        {
    //            SetSprite(img,PathDefine.StarIcon1);
    //        }
    //        else
    //        {
    //            SetSprite(img, PathDefine.StarIcon2);
    //        }
    //    }

    //    nextSc = resSvc.GetStrengthCfgData(currentIndex,nextStarlv);
    //    if (nextSc!=null)//存在下一级
    //    {
    //        SetActive(conditionTrans);
    //        SetActive(imgArrow1);
    //        SetActive(imgArrow2);
    //        SetActive(imgArrow3);
    //        SetActive(txtHP2);
    //        SetActive(txtDamage2);
    //        SetActive(txtDef2);

    //        SetText(txtHP2,"强化后  +"+nextSc.addhp);
    //        SetText(txtDamage2, "强化后  +" + nextSc.addhurt);
    //        SetText(txtDef2, "强化后  +" + nextSc.adddef);

    //        SetText(txtNeedLv, "需要等级： " + nextSc.minlv);
    //        SetText(txtCostCoin,nextSc.coin);
    //        SetText(txtCrystal,nextSc.crystal+"/"+pd.crystal);
    //    }
    //    else
    //    {
    //        SetActive(conditionTrans,false);
    //        SetActive(imgArrow1, false);
    //        SetActive(imgArrow2, false);
    //        SetActive(imgArrow3, false);
    //        SetActive(txtHP2, false);
    //        SetActive(txtDamage2, false);
    //        SetActive(txtDef2, false);
    //    }
    //    int sumAddHP = resSvc.GetPropAddValPreLv(currentIndex, nextStarlv, 1);
    //    int sumAddHurt = resSvc.GetPropAddValPreLv(currentIndex, nextStarlv, 2);
    //    int sumAddDef = resSvc.GetPropAddValPreLv(currentIndex, nextStarlv, 3);
    //    SetText(txtHP1, "生命  +" + sumAddHP);
    //    SetText(txtDamage1, "伤害  +" + sumAddHurt);
    //    SetText(txtDef1, "防御  +" + sumAddDef);
    //}
    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.uiClick);
        SetWndState(false);
    }
    //public void ClickStrongBtn()
    //{
    //    audioSvc.PlayUIAudio(Constants.uiClick);
    //    //先在客户端进行简单校验，不要直接发送，别用户狂点直接就服务器数据过大。
    //    if (pd.strengthArray[currentIndex]<10)//当前位置强化星级《10，10就满级了
    //    {
    //        if (pd.lv < nextSc.minlv)
    //        {
    //            GameRoot.instance.AddTips("角色等级不足");
    //            return;
    //        }
    //        if (pd.coin < nextSc.coin)
    //        {
    //            GameRoot.instance.AddTips("角色金币不足");
    //            return;
    //        }
    //        if (pd.crystal < nextSc.crystal)
    //        {
    //            GameRoot.instance.AddTips("角色水晶不足");
    //            return;
    //        }

    //        netService.SendMsg(new GameMsg
    //        {
    //            cmd = (int)CMD.ReqStrong,
    //            reqStrong = new ReqStrong
    //            {
    //                pos = currentIndex,
    //            }
    //        });
    //    }
    //    else
    //    {
    //        GameRoot.instance.AddTips("装备等级已满");
    //    }
    //}
    public void UpdateUI()
    {
        audioSvc.PlayUIAudio(Constants.strengthSuccess);
        ClickPosItem(currentIndex);
    }
}