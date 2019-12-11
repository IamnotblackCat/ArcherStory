/****************************************************
    文件：PlayerCtrlWnd.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/11/28 14:41:45
	功能：战斗场景界面
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrlWnd : WindowRoot
{
    #region Public UI Transform

    public GameObject returnGo;
    public Button btnGuide;
    public Transform expProgramTrans;
    public Text txtLV;
    public Text txtSk2CD;
    public Text txtSk3CD;
    public Text txtSk4CD;
    public Text txtSk5CD;
    public Text txtSk6CD;
    public Text txtSk7CD;
    public Text txtSk8CD;

    public Image imgSk2CD;
    public Image imgSk3CD;
    public Image imgSk4CD;
    public Image imgSk5CD;
    public Image imgSk6CD;
    public Image imgSk7CD;
    public Image imgSk8CD;
    #endregion

    #region 技能冷却相关变量

    private float sk2FillCount;//记录imgCD图数据，用来计算显示fillAmount属性
    private float sk2NumCount;//记录冷却时间的文本，用来计算显示剩余冷却时间
    private bool isSkl2CD = false;
    private float skl2CDTime;

    #endregion

    #region 范围技能区域显示相关
    private RaycastHit hit;
    private Ray ray;
    private bool areaSkillIcon = false;
    public GameObject sprite2D;
    public Vector3 pos;
    //范围技能区域显示
    private GameObject skillArea;
    public Sprite sprite1;//范围内显示图标
    public Sprite sprite2;//范围外显示图标
    private bool canRealseSkill = true;
    #endregion

    private bool menuState = true;//true是打开，false收起
    private AutoGuideCfg currentTaskData;

    //UI自适应不能使用固定距离，要计算得出比率距离
    private float pointDis = Screen.height * 1.0f / Constants.screenStandardHeight * Constants.screenOperationDistant;

    protected override void InitWnd()
    {
        base.InitWnd();

        RefreshUI();
        skl2CDTime = resSvc.GetSkillCfgData(102).skillCDTime;
        InitSkillAreaIcon();
    }
    public void RefreshUI()
    {
        PlayerData pd = GameRoot.instance.Playerdata;

        SetText(txtLV, pd.lv);

        #region ExpProgress
        int expValPercent = (int)(pd.exp * 1.0f / 100);

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
    }

    #region Click Events

    public void ClickHeadBtn()
    {
        audioSvc.PlayUIAudio(Constants.uiOpenPage);
        MainCitySys.Instance.OpenInfoWnd();
    }
    public void ClickReturnBtn()
    {
        returnGo.SetActive(true);
    }
    public void ClickConfirm()
    {
        GameRoot.instance.ClearUIRoot();
        returnGo.SetActive(false);
        MainCitySys.Instance.EnterMainCity();
    }
    public void ClickConcel()
    {
        returnGo.SetActive(false);
    }

    #endregion
    public void ClickSkill1()
    {
        BattleSys.Instance.ReleaseSkill(1);
    }
    public void ClickSkill2()
    {
        if (!isSkl2CD)
        {
            BattleSys.Instance.ReleaseSkill(2);
            isSkl2CD = true;
            SetActive(imgSk2CD);
            imgSk2CD.fillAmount = 1;
            SetText(txtSk2CD,skl2CDTime.ToString());
        }

    }
    public void ClickSkill3()
    {
        BattleSys.Instance.ReleaseSkill(3);
    }
    public void ClickSkill4()
    {
        BattleSys.Instance.ReleaseSkill(4);
    }
    public void ClickSkill5()
    {
        BattleSys.Instance.ReleaseSkill(5);
    }
    public void ClickSkill6()
    {
        BattleSys.Instance.ReleaseSkill(6);
    }
    public void ClickSkill7()
    {
        BattleSys.Instance.ReleaseSkill(7);
    }
    public void ClickSkill8()
    {
        BattleSys.Instance.ReleaseSkill(8);
    }
    private void Update()
    {
        #region 移动和技能按键检测
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector2 _dir = new Vector2(h, v);
        if (BattleSys.Instance.battleMg.entitySelfPlayer != null)
        {
            BattleSys.Instance.battleMg.SetSelfPlayerMoveDir(_dir);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ClickSkill2();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            areaSkillIcon = true;
        }
        if (areaSkillIcon)
        {
            UpdateAreaIcon(KeyCode.Alpha3,5f);
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            skillArea.SetActive(false);
            areaSkillIcon = false;
            if (canRealseSkill)//超出范围无法释放
            {
                ClickSkill3();
            }
        }
        #endregion
        float delta = Time.deltaTime;
        if (isSkl2CD)
        {
            sk2FillCount += delta;
            if (sk2FillCount>=skl2CDTime)
            {
                isSkl2CD = false;
                sk2FillCount = 0;
                SetActive(imgSk2CD,false);
            }
            else
            {
                imgSk2CD.fillAmount = 1 - sk2FillCount / skl2CDTime;
            }
            sk2NumCount+=delta;
            if (sk2NumCount>=skl2CDTime)
            {
                sk2NumCount = 0;
            }
            float val = skl2CDTime - sk2NumCount;
            SetText(txtSk2CD,val.ToString("0.0"));
        }
    }
    public void ClickInitBtn()
    {
        resSvc.InitCfgData();
    }
    //实时显示技能范围图标
    //传入一个距离，判断如果角色自身与射线检测目标地点超出距离，超出变换图片，且无法释放技能
    public void UpdateAreaIcon(KeyCode key,float distance)
    {
        Sprite changeSprite = null;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null&&hit.collider.tag=="Road")
            {
                //var coll = new Vector3(hit.point.x,hit.point.y,hit.point.z);
                skillArea.SetActive(true);
                pos= new Vector3(hit.point.x, hit.point.y, hit.point.z);
                Vector3 playerPos = BattleSys.Instance.battleMg.entitySelfPlayer.GetPos();
                float dis = Vector3.Distance(playerPos, pos);
                if (dis>=distance)
                {
                    changeSprite = sprite2;
                    canRealseSkill = false;
                }
                else
                {
                    changeSprite = sprite1;
                    canRealseSkill = true;
                }
                skillArea.GetComponent<SpriteRenderer>().sprite = changeSprite;
                skillArea.transform.position = pos;
            }
        }
    }
    private void InitSkillAreaIcon()
    {
        skillArea = resSvc.LoadPrefab(PathDefine.skillAreaIcon);
        //skillArea.transform.Rotate(new Vector3(90, 0, 0));
        skillArea.transform.SetParent(GameRoot.instance.transform);
        skillArea.SetActive(false);
        //Debug.Log(skillArea);
    }
}