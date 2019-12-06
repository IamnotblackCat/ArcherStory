/****************************************************
    文件：ItemEntityHP.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/12/6 16:9:4
	功能：血条及暴击/飘字动画脚本
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class ItemEntityHP : MonoBehaviour 
{
    public Image ImgHPGreen;
    public Image ImgHPRed;

    public Text txtCritical;
    public Animation animCritical;

    public Text txtHPDown;
    public Animation animHPDown;

    private int hpVal;
    private RectTransform rect;
    private Transform rootTrans;
    private float scaleRate = 1.0F * Constants.screenStandardHeight / Screen.height;
    private void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(rootTrans.position);
        rect.anchoredPosition = screenPos * scaleRate;
        //rect.transform.position = screenPos;
    }
    public void InitItemInfo(Transform targetTrans, int hp)
    {
        rect = transform.GetComponent<RectTransform>();
        rootTrans = targetTrans;
        hpVal = hp;
        ImgHPGreen.fillAmount = 1;
        ImgHPRed.fillAmount = 1;
    }
    //多个数字飘出来的时候，要覆盖掉前面的
    public void SetCritical(int criticalNum)
    {
        animCritical.Stop();
        txtCritical.text = "暴击 " + criticalNum;
        animCritical.Play();
    }
    public void SetHPDownTxt(int HP)
    {
        animHPDown.Stop();
        txtHPDown.text = "-"+HP;
        animHPDown.Play();
    }
}