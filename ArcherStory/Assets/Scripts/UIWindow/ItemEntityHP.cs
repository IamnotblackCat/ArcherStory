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

        UpdateMixBlend();
    }
    public void InitItemInfo(Transform targetTrans, int hp,bool isBoss=false)
    {
        rect = transform.GetComponent<RectTransform>();
        rootTrans = targetTrans;
        hpVal = hp;
        ImgHPGreen.fillAmount = 1;
        ImgHPRed.fillAmount = 1;
        if (isBoss)
        {
            ImgHPGreen.enabled = false;
            ImgHPRed.enabled = false;
            Image imgBG = GetComponent<Image>();
            imgBG.enabled = false;
        }
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
    private float currentHPProgram=1;
    private float targetHPProgram=1;
    public void SetHPVal(int oldVal,int newVal)
    {
        currentHPProgram = 1.0f * oldVal / hpVal;
        targetHPProgram = 1.0f * newVal / hpVal;
        //当前实际血量绿色显示，红色渐变消失
        ImgHPGreen.fillAmount = targetHPProgram;
    }
    private void UpdateMixBlend()
    {
        if (Mathf.Abs(currentHPProgram - targetHPProgram) < Constants.accelerateHPSpeed * Time.deltaTime)
        {
            currentHPProgram = targetHPProgram;
        }
        else if (currentHPProgram > targetHPProgram)
        {
            currentHPProgram -= Constants.accelerateHPSpeed * Time.deltaTime;
        }
        else
        {
            currentHPProgram += Constants.accelerateHPSpeed * Time.deltaTime;
        }
        ImgHPRed.fillAmount = currentHPProgram;
    }
}