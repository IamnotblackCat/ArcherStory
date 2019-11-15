/****************************************************
    文件：InfoWnd.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/10/30 11:45:54
	功能：Nothing
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoWnd : WindowRoot 
{
    public RawImage imgCharactor;

    public Text txtCharactor;
    public Text txtExp;
    public Image imgExp;
    public Text txtPower;
    public Image imgPower;
    public Text txtJob;
    public Text txtFight;
    public Text txtHP;
    public Text txtDamage;
    public Text txtDefend;
    

    #region DetailProperty
    public Transform transDetail;
    public Button detailBtn;
    public Button CloseDetailBtn;
    public Text hpValue;
    public Text adValue;
    public Text apValue;
    public Text addValue;
    public Text apdValue;
    public Text dodgeValue;
    public Text pierceValue;

    #endregion
    public Text criticalValue;

    private Vector2 startPos;
    protected override void InitWnd()
    {
        base.InitWnd();
        RegistTouchEvts();
        RefreshUI();
        SetActive(transDetail,false);
    }
    private void RegistTouchEvts()
    {
        OnClickDown(imgCharactor.gameObject, (PointerEventData evt) =>
         {
             startPos = evt.position;
             MainCitySys.Instance.SetStartRotate();
         });
        OnDrag(imgCharactor.gameObject,(PointerEventData evt)=>
        {
            float rotate = -(evt.position.x - startPos.x)*0.3f;
            //Debug.Log(rotate);
            MainCitySys.Instance.SetPlayerRotate(rotate);
        });
    }
    private void RefreshUI()
    {
        PlayerData pd = GameRoot.instance.Playerdata;
        SetText(txtCharactor,pd.name+" LV"+pd.lv);
        SetText(txtExp,pd.exp+"/"+PECommon.GetExpUpValByLv(pd.lv));
        imgExp.fillAmount = (pd.exp*1.0f/PECommon.GetExpUpValByLv(pd.lv));
        SetText(txtPower,pd.power);
        imgPower.fillAmount = (pd.power*1.0f/PECommon.GetPowerLimit(pd.lv));
        SetText(txtJob, " 职业  暗夜刺客");
        SetText(txtFight, " 战力  "+PECommon.GetFightByPlayerData(pd));
        SetText(txtHP, " 生命  " + pd.hp);
        SetText(txtDamage, " 伤害  "+(pd.ad+pd.ap));
        SetText(txtDefend, " 防御  " + (pd.addef+pd.apdef));

        //detail
        SetText(hpValue,pd.hp);
        SetText(addValue,pd.ad);
        SetText(apValue,pd.ap);
        SetText(addValue,pd.addef);
        SetText(apdValue,pd.apdef);
        SetText(dodgeValue,pd.dodge);
        SetText(pierceValue,pd.pierce);
        SetText(criticalValue,pd.critical);
    }
    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.uiClick);
        MainCitySys.Instance.CloseInfoWndCamera();
        SetWndState(false);
    }
    public void ClickDetailBtn()
    {
        audioSvc.PlayUIAudio(Constants.uiClick);
        SetActive(transDetail);
    }
    public void ClickCloseDetailBtn()
    {
        audioSvc.PlayUIAudio(Constants.uiClick);
        SetActive(transDetail,false);
    }
}