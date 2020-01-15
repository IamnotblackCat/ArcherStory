/****************************************************
    文件：PlayerCtrlWnd.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/11/28 14:41:45
	功能：战斗场景界面
*****************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrlWnd : WindowRoot
{
    #region Public UI Transform

    public GameObject bossSkillGo;
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

    public Text txtSelfHP;
    public Image imgSelfHP;

    public Transform bossHPTrans;
    public Image hpRed;
    public Image hpYellow;
    #endregion
    private int HPSum;
    #region 技能冷却相关变量

    private float sk2FillCount;//记录imgCD图数据，用来计算显示fillAmount属性
    private float sk2NumCount;//记录冷却时间的文本，用来计算显示剩余冷却时间
    private bool isSk2CD = false;
    private float sk2CDTime;

    private float sk3FillCount;//记录imgCD图数据，用来计算显示fillAmount属性
    private float sk3NumCount;//记录冷却时间的文本，用来计算显示剩余冷却时间
    private bool isSk3CD = false;
    private float sk3CDTime;

    private float sk4FillCount;//记录imgCD图数据，用来计算显示fillAmount属性
    private float sk4NumCount;//记录冷却时间的文本，用来计算显示剩余冷却时间
    private bool isSk4CD = false;
    private float sk4CDTime;

    private float sk5FillCount;//记录imgCD图数据，用来计算显示fillAmount属性
    private float sk5NumCount;//记录冷却时间的文本，用来计算显示剩余冷却时间
    private bool isSk5CD = false;
    private float sk5CDTime;

    private float sk6FillCount;//记录imgCD图数据，用来计算显示fillAmount属性
    private float sk6NumCount;//记录冷却时间的文本，用来计算显示剩余冷却时间
    private bool isSk6CD = false;
    private float sk6CDTime;

    private float sk7FillCount;//记录imgCD图数据，用来计算显示fillAmount属性
    private float sk7NumCount;//记录冷却时间的文本，用来计算显示剩余冷却时间
    private bool isSk7CD = false;
    private float sk7CDTime;

    private float sk8FillCount;//记录imgCD图数据，用来计算显示fillAmount属性
    private float sk8NumCount;//记录冷却时间的文本，用来计算显示剩余冷却时间
    private bool isSk8CD = false;
    private float sk8CDTime;
    #endregion

    #region 范围技能区域显示相关
    private RaycastHit hit;
    private Ray ray;
    private bool areaSkill3Icon = false;//3技能射线开关
    private bool areaSkill4Icon = false;//4技能射线检测开关
    private bool areaSkill7Icon = false;//7技能射线检测开关
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

        HPSum = GameRoot.instance.Playerdata.hp;
        SetText(txtSelfHP,HPSum+"/"+HPSum);
        imgSelfHP.fillAmount = 1;

        RefreshUI();
        sk2CDTime = resSvc.GetSkillCfgData(102).skillCDTime;
        sk3CDTime = resSvc.GetSkillCfgData(103).skillCDTime;
        sk4CDTime = resSvc.GetSkillCfgData(104).skillCDTime;
        sk5CDTime = resSvc.GetSkillCfgData(105).skillCDTime;
        sk6CDTime = resSvc.GetSkillCfgData(106).skillCDTime;
        sk7CDTime = resSvc.GetSkillCfgData(107).skillCDTime;
        sk8CDTime = resSvc.GetSkillCfgData(108).skillCDTime;
        InitSkillAreaIcon();
        if (bossHPTrans.gameObject.activeSelf)
        {
            bossHPTrans.gameObject.SetActive(false);
        }
    }
    public void RefreshUI()
    {
        PlayerData pd = GameRoot.instance.Playerdata;

        SetText(txtLV, pd.lv);

        #region ExpProgress
        int expValPercent = (int)(pd.exp * 1.0f);

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

    public void ClickCharacterBtn()
    {
        audioSvc.PlayUIAudio(Constants.uiOpenPage);
        MainCitySys.Instance.OpenInfoWnd();
    }
    
    #endregion
    public void ClickSkill1()
    {
        BattleSys.Instance.ReleaseSkill(1);
    }
    public void ClickSkill2()
    {
        if (!isSk2CD)
        {
            BattleSys.Instance.ReleaseSkill(2);
            isSk2CD = true;
            SetActive(imgSk2CD);
            imgSk2CD.fillAmount = 1;
            SetText(txtSk2CD,sk2CDTime.ToString());
        }

    }
    public void ClickSkill3()
    {
        if (!isSk3CD)
        {
            BattleSys.Instance.ReleaseSkill(3);
            isSk3CD = true;
            SetActive(imgSk3CD);
            imgSk3CD.fillAmount = 1;
            SetText(txtSk3CD,sk3CDTime.ToString());
        }
    }
    public void ClickSkill4()
    {
        if (!isSk4CD)
        {
            BattleSys.Instance.ReleaseSkill(4);
            isSk4CD = true;
            SetActive(imgSk4CD);
            imgSk4CD.fillAmount = 1;
            SetText(txtSk4CD, sk4CDTime.ToString());
        }
    }
    public void ClickSkill5()
    {
        if (!isSk5CD)
        {
            BattleSys.Instance.ReleaseSkill(5);
            isSk5CD = true;
            SetActive(imgSk5CD);
            imgSk5CD.fillAmount = 1;
            SetText(txtSk5CD, sk5CDTime.ToString());
        }
    }
    public void ClickSkill6()
    {
        if (!isSk6CD)
        {
            BattleSys.Instance.ReleaseSkill(6);
            isSk6CD = true;
            SetActive(imgSk6CD);
            imgSk6CD.fillAmount = 1;
            SetText(txtSk6CD, sk6CDTime.ToString());
        }
    }
    public void ClickSkill7()
    {
        if (!isSk7CD)
        {
            BattleSys.Instance.ReleaseSkill(7);
            isSk7CD = true;
            SetActive(imgSk7CD);
            imgSk7CD.fillAmount = 1;
            SetText(txtSk7CD, sk7CDTime.ToString());
        }
    }
    public void ClickSkill8()
    {
        if (!isSk8CD)
        {
            BattleSys.Instance.ReleaseSkill(8);
            isSk8CD = true;
            SetActive(imgSk8CD);
            imgSk8CD.fillAmount = 1;
            SetText(txtSk8CD, sk8CDTime.ToString());
        }
    }
    private void Update()
    {
        #region 技能冷却显示

        float delta = Time.deltaTime;
        if (isSk2CD)
        {
            sk2FillCount += delta;
            if (sk2FillCount >= sk2CDTime)
            {
                isSk2CD = false;
                sk2FillCount = 0;
                SetActive(imgSk2CD, false);
            }
            else
            {
                imgSk2CD.fillAmount = 1 - sk2FillCount / sk2CDTime;
            }
            sk2NumCount += delta;
            if (sk2NumCount >= sk2CDTime)
            {
                sk2NumCount = 0;
            }
            float val = sk2CDTime - sk2NumCount;
            SetText(txtSk2CD, val.ToString("0.0"));
        }
        if (isSk3CD)
        {
            sk3FillCount += delta;
            if (sk3FillCount >= sk3CDTime)
            {
                //Debug.Log("sk3FillCount:"+sk3FillCount+"--sk3CDTime:"+sk3CDTime);
                isSk3CD = false;
                sk3FillCount = 0;
                SetActive(imgSk3CD, false);
            }
            else
            {
                imgSk3CD.fillAmount = 1 - sk3FillCount / sk3CDTime;
            }
            sk3NumCount += delta;
            if (sk3NumCount >= sk3CDTime)
            {
                sk3NumCount = 0;
            }
            float val = sk3CDTime - sk3NumCount;
            SetText(txtSk3CD, val.ToString("0.0"));
        }
        if (isSk4CD)
        {
            sk4FillCount += delta;
            if (sk4FillCount >= sk4CDTime)
            {
                isSk4CD = false;
                sk4FillCount = 0;
                SetActive(imgSk4CD, false);
            }
            else
            {
                imgSk4CD.fillAmount = 1 - sk4FillCount / sk4CDTime;
            }
            sk4NumCount += delta;
            if (sk4NumCount >= sk4CDTime)
            {
                sk4NumCount = 0;
            }
            float val = sk4CDTime - sk4NumCount;
            SetText(txtSk4CD, val.ToString("0.0"));
        }
        if (isSk5CD)
        {
            sk5FillCount += delta;
            if (sk5FillCount >= sk5CDTime)
            {
                isSk5CD = false;
                sk5FillCount = 0;
                SetActive(imgSk5CD, false);
            }
            else
            {
                imgSk5CD.fillAmount = 1 - sk5FillCount / sk5CDTime;
            }
            sk5NumCount += delta;
            if (sk5NumCount >= sk5CDTime)
            {
                sk5NumCount = 0;
            }
            float val = sk5CDTime - sk5NumCount;
            SetText(txtSk5CD, val.ToString("0.0"));
        }
        if (isSk6CD)
        {
            sk6FillCount += delta;
            if (sk6FillCount >= sk6CDTime)
            {
                isSk6CD = false;
                sk6FillCount = 0;
                SetActive(imgSk6CD, false);
            }
            else
            {
                imgSk6CD.fillAmount = 1 - sk6FillCount / sk6CDTime;
            }
            sk6NumCount += delta;
            if (sk6NumCount >= sk6CDTime)
            {
                sk6NumCount = 0;
            }
            float val = sk6CDTime - sk6NumCount;
            SetText(txtSk6CD, val.ToString("0.0"));
        }
        if (isSk7CD)
        {
            sk7FillCount += delta;
            if (sk7FillCount >= sk7CDTime)
            {
                isSk7CD = false;
                sk7FillCount = 0;
                SetActive(imgSk7CD, false);
            }
            else
            {
                imgSk7CD.fillAmount = 1 - sk7FillCount / sk7CDTime;
            }
            sk7NumCount += delta;
            if (sk7NumCount >= sk7CDTime)
            {
                sk7NumCount = 0;
            }
            float val = sk7CDTime - sk7NumCount;
            SetText(txtSk7CD, val.ToString("0.0"));
        }
        if (isSk8CD)
        {
            sk8FillCount += delta;
            if (sk8FillCount >= sk8CDTime)
            {
                isSk8CD = false;
                sk8FillCount = 0;
                SetActive(imgSk8CD, false);
            }
            else
            {
                imgSk8CD.fillAmount = 1 - sk8FillCount / sk8CDTime;
            }
            sk8NumCount += delta;
            if (sk8NumCount >= sk8CDTime)
            {
                sk8NumCount = 0;
            }
            float val = sk8CDTime - sk8NumCount;
            SetText(txtSk8CD, val.ToString("0.0"));
        }
        #endregion
        #region 移动和技能按键检测
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector2 _dir = new Vector2(h, v);
        //boss血条激活了就启动血条渐变
        if (bossHPTrans.gameObject.activeSelf)
        {
            BlendBossHP();
        }
        if (BattleSys.Instance.battleMg.entitySelfPlayer != null)
        {//暂停时无法控制角色
            if (BattleSys.Instance.battleMg.isPaused == true)
            {
                return;
            }
            BattleSys.Instance.battleMg.SetSelfPlayerMoveDir(_dir);
            if (!BattleSys.Instance.battleMg.entitySelfPlayer.canControll)
            {//被打断取消范围显示
                if (skillArea.activeSelf)
                {
                    skillArea.SetActive(false);
                    areaSkill3Icon = false;
                    areaSkill4Icon = false;
                    areaSkill7Icon = false;
                }
                return;
            }
           
        }
       
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ClickSkill1();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ClickSkill2();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            ClickSkill5();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            ClickSkill6();
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            ClickSkill8();
        }
        #region 范围技能
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            areaSkill3Icon = true;
        }
        if (areaSkill3Icon)
        {
            UpdateAreaIcon(KeyCode.Alpha3,Constants.skill3MaxArea,Constants.skill3Scale);
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            if (areaSkill3Icon==false)
            {
                return;
            }
            skillArea.SetActive(false);
            areaSkill3Icon = false;
            if (canRealseSkill)//超出范围无法释放
            {
                ClickSkill3();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            areaSkill4Icon = true;
        }
        if (areaSkill4Icon)
        {
            UpdateAreaIcon(KeyCode.Alpha4, Constants.skill4MaxArea,Constants.skill4Scale);
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {//可能已经被中断
            if (areaSkill4Icon==false)
            {
                return;
            }
            skillArea.SetActive(false);
            areaSkill4Icon = false;
            if (canRealseSkill)//超出范围无法释放
            {
                ClickSkill4();
            }
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            areaSkill7Icon = true;
        }
        if (areaSkill7Icon)
        {
            UpdateAreaIcon(KeyCode.F, Constants.skill7MaxArea,Constants.skill7Scale);
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            if (areaSkill7Icon == false)
            {
                return;
            }
            skillArea.SetActive(false);
            areaSkill7Icon = false;
            if (canRealseSkill)//超出范围无法释放
            {
                ClickSkill7();
            }
        }
        #endregion
        #endregion
    }
   
    public void ClickHeadBtn()
    {
        audioSvc.PlayUIAudio(Constants.uiOpenPage);
        BattleSys.Instance.battleMg.isPaused = true;
        BattleSys.Instance.SetBattleEndWndState(FubenEndType.Pause);
    }
    public void ClickBossSkillBtn()
    {
        bossSkillGo.SetActive(true);
    }
    public void ClickCloseBossSkillBtn()
    {
        bossSkillGo.SetActive(false);
    }
    //实时显示技能范围图标
    //传入一个距离，判断如果角色自身与射线检测目标地点超出距离，超出变换图片，且无法释放技能
    public void UpdateAreaIcon(KeyCode key,float distance,Vector3 skillScale)
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
                skillArea.transform.localScale = skillScale;
            }
        }
    }
    private void InitSkillAreaIcon()
    {
        skillArea = resSvc.LoadPrefab(PathDefine.skillAreaIcon);
        skillArea.transform.SetParent(GameRoot.instance.transform);
        skillArea.SetActive(false);
    }

    //玩家血球显示 TODO，修改为渐变
    public void SetSelfHPBarVal(int val)
    {
        SetText(txtSelfHP,val+"/"+HPSum);
        imgSelfHP.fillAmount = val * 1.0f / HPSum;
    }
    
    public void SetBossHPTransform(bool state,float prg=1)
    {
        SetActive(bossHPTrans,state);
        hpRed.fillAmount = prg;
        hpYellow.fillAmount = prg;
    }

    //boss血条渐变
    private float currentHPProgram = 1f;
    private float targetHPProgram = 1f;
    public void SetBossHPBarVal(int oldVal,int newVal,int sumVal)
    {
        currentHPProgram = oldVal * 1.0f / sumVal;
        targetHPProgram = newVal * 1.0f / sumVal;
        hpRed.fillAmount = targetHPProgram;
    }
    private void BlendBossHP()
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
        hpYellow.fillAmount = currentHPProgram;
    }
}