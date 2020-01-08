/****************************************************
    文件：InfoWnd.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/10/30 11:45:54
	功能：角色面板信息
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoWnd : WindowRoot/*, IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler, IEndDragHandler*/
{
    public RawImage imgCharactor;
    
    public Text txtExp;
    public Image imgExp;
    public Text txtJob;
    public Text txtHP;
    public Text txtDamage;
    public Text txtDefend;
    

    #region DetailProperty
    public Transform transDetail;
    public Button detailBtn;
    public Button CloseDetailBtn;
    public Text hpValue;
    public Text adValue;
    public Text addValue;
    public Text pierceValue;

    #endregion
    public Text criticalValue;

    private Vector2 startPos;
    private RectTransform rect;
    private Vector3 uiOffset = Vector3.zero;
    protected override void InitWnd()
    {
        base.InitWnd();
        RegistTouchEvts();
        RefreshUI();
        rect = GetComponent<RectTransform>();
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
    public void RefreshUI()
    {
        PlayerData pd = GameRoot.instance.Playerdata;
        SetText(txtExp,pd.exp+"/"+100);
        imgExp.fillAmount = (pd.exp*1.0f/100);
        SetText(txtJob, " 职业  弓箭手");
        
        //detail
        SetText(hpValue,pd.hp);
        SetText(adValue,pd.attackValue);
        SetText(addValue,pd.defend);
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