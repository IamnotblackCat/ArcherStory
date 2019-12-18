/****************************************************
    文件：MainCityWnd.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/10/25 9:17:49
	功能：主城UI界面
*****************************************************/
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainCityWnd : WindowRoot 
{
    #region Public UI Transform
    
    public GameObject goFuben;
    public Text txtLV;
    public Text txtExpPrg;

    public Button btnGuide;

    public Transform expProgramTrans;

    #endregion

    private bool menuState = true;//true是打开，false收起
    private AutoGuideCfg currentTaskData;

    //UI自适应不能使用固定距离，要计算得出比率距离
    private float pointDis = Screen.height * 1.0f / Constants.screenStandardHeight * Constants.screenOperationDistant;

    protected override void InitWnd()
    {
        base.InitWnd();
        
        RefreshUI();
    }
    public void RefreshUI()
    {
        PlayerData pd = GameRoot.instance.Playerdata;
        
        SetText(txtLV,pd.lv);

        #region ExpProgress
        int expValPercent = (int)(pd.exp * 1.0f /100);
        SetText(txtExpPrg, expValPercent + "%");
        int index = expValPercent / 10;
        GridLayoutGroup grid = expProgramTrans.GetComponent<GridLayoutGroup>();
        //得到 标准高度和当前高度的比例，然后乘以当前宽度得到真实宽度，然后减掉间隙计算经验条宽度
        float screenRate = 1.0f * Constants.screenStandardHeight / Screen.height;
        float screenWidth = Screen.width * screenRate;
        float width = (screenWidth - 180) / 10;

        grid.cellSize = new Vector2(width, 7);

        for (int i = 0; i < expProgramTrans.childCount; i++)
        {
            Image expImageValue = expProgramTrans.GetChild(i).GetComponent<Image>();
            if (i < index)
            {
                expImageValue.fillAmount = 1;
            }
            else if (i == index)
            {
                expImageValue.fillAmount = expValPercent * 1.0f % 10 / 10;
            }
            else
            {
                expImageValue.fillAmount = 0;
            }
        }
        #endregion

        //设置自动任务图标
        //currentTaskData = resSvc.GetGuideCfgData(pd.guideid);
        //if (currentTaskData!=null)
        //{
        //    SetGuideBtnIcon(currentTaskData.npcID);
        //}
        //else
        //{//没任务就显示默认图标
        //    SetGuideBtnIcon(-1);
        //}
    }
    //根据任务的不同设置不同的NPC头像
    private void SetGuideBtnIcon(int npcID)
    {
        string spPath = "";
        Image image = btnGuide.GetComponent<Image>();
        switch (npcID)
        {
            case Constants.NPCWiseMan:
                spPath = PathDefine.WiseHead;
                break;
            case Constants.NPCGeneral:
                spPath = PathDefine.GeneralHead;
                break;
            case Constants.NPCArtisan:
                spPath = PathDefine.ArtisanHead;
                break;
            case Constants.NPCTrader:
                spPath = PathDefine.TraderHead;
                break;
            default:
                spPath = PathDefine.TaskHead;
                break;
        }
        SetSprite(image,spPath);
    }
    #region Click Events
    //public void ClickGuideBtn()
    //{
    //    audioSvc.PlayUIAudio(Constants.uiClick);
    //    if (currentTaskData!=null)
    //    {
    //        MainCitySys.Instance.RunTask(currentTaskData);
    //    }
    //    else
    //    {
    //        GameRoot.instance.AddTips("更多引导，正在开发中，敬请期待。。。");
    //    }
    //}
    //public void ClickMenuBtn()
    //{
    //    audioSvc.PlayUIAudio(Constants.uiExtenBtn);
    //    menuState = !menuState;

    //    AnimationClip clip = null;
    //    //取反之后的状态
    //    if (menuState)
    //    {
    //        clip = menuAnim.GetClip("OpenMainCityBtn");
    //    }
    //    else
    //    {
    //        clip = menuAnim.GetClip("CloseMainCityBtn");
    //    }
    //    menuAnim.Play(clip.name);
    //}
    public void ClickHeadBtn()
    {
        audioSvc.PlayUIAudio(Constants.uiOpenPage);
        MainCitySys.Instance.OpenInfoWnd();
    }
    public void ClickStrengthBtn()
    {
        audioSvc.PlayUIAudio(Constants.uiOpenPage);
        MainCitySys.Instance.OpenStrengthWnd();
    }
    public void ClickFubenBtn()
    {
        goFuben.SetActive(true);
    }
    public void ClickKnapsackBtn()
    {
        Knapsack.Instance.DisplaySwitch();
    }
    public void ClickSkillBtn()
    {

    }
    public void ClickSystemBtn()
    {

    }
    public void ClickConfirm()
    {
        GameRoot.instance.ClearUIRoot();
        GameRoot.instance.dynamicWnd.SetWndState();
        goFuben.SetActive(false);
        BattleSys.Instance.StartBattle(Constants.Duplicate);
    }
    public void ClickConcel()
    {
        goFuben.SetActive(false);
    }
    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector2 _dir = new Vector2(h, v);
        MainCitySys.Instance.SetMoveDir(_dir);
    }
    //public void RegistrTouchEvts()
    //{
    //    //添加监听器
    //    OnClickDown(imgTouch.gameObject, (PointerEventData evt) =>
    //     {
    //         clickPos = evt.position;
    //         SetActive(imgDirPoint);
    //         imgDirBg.transform.position = evt.position;
    //     });
    //    OnClickUP(imgTouch.gameObject, (PointerEventData evt) =>
    //    {
    //        imgDirBg.transform.position = defaultPos;
    //        SetActive(imgDirPoint,false);
    //        //小圆点位置设置为中心锚点的中间
    //        imgDirPoint.transform.localPosition = Vector2.zero;
    //        //方向信息传递
    //        //MainCitySys.Instance.SetMoveDir(Vector2.zero);
    //    });
    //    OnDrag(imgTouch.gameObject, (PointerEventData evt) =>
    //    {
    //        Vector2 dir = evt.position - clickPos;//得到拖拽的方向
    //        //要把拖拽向量方向不变，但是距离限制
    //        if (dir.magnitude>pointDis)
    //        {
    //            Vector2 clampDir = Vector2.ClampMagnitude(dir,pointDis);
    //            //背景图位置+自身需要移动的
    //            imgDirPoint.transform.position = clickPos + clampDir;
    //        }
    //        else
    //        {
    //            imgDirPoint.transform.position = evt.position;
    //        }
    //        //方向信息传递
    //        //MainCitySys.Instance.SetMoveDir(dir.normalized);
    //    });
    //}

    #endregion

}