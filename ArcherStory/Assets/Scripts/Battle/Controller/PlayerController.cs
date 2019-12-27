/****************************************************
    文件：PlayerMoveTest.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/10/29 11:37:58
	功能：主角表现实体角色控制器
*****************************************************/

using UnityEngine;

public class PlayerController : Controller
{
    #region 技能特效
    public GameObject skill1FX;
    public GameObject skill2FX;
    public GameObject skill2Emp;
    public GameObject skill3FX;
    public GameObject skill4FX;
    public GameObject skill4Emp;
    public GameObject skill5FX;
    public GameObject skill6FX;
    public GameObject skill7FX;
    public GameObject skill8FX;
    private GameObject skill3Ground;
    private GameObject skill4Ground;

    #endregion
    #region 相机控制
    //private Transform camMainTrans;
    private float camRotSmooth = 3f;
    private bool isRotate;
    private float distance;
    private float disSmooth = 10f;
    #endregion

    private Vector3 cameraOffset = Vector3.zero;
    //平滑动画混合树用到的值
    private float currentBlend = 0;
    private float targetBlend = 0;


    public override void Init()
    {
        base.Init();
        camMainTrans = Camera.main.transform;
        cameraOffset = camMainTrans.transform.position - transform.position;
        InitSkillGroudFX();

        if (skill1FX!=null)
        {
            fxDic.Add(skill1FX.name, skill1FX);
            fxDic.Add(skill2FX.name, skill2FX);
            fxDic.Add(skill3FX.name, skill3FX);
            fxDic.Add(skill4FX.name, skill4FX);
            fxDic.Add(skill5FX.name, skill5FX);
            fxDic.Add(skill6FX.name, skill6FX);
            fxDic.Add(skill7FX.name, skill7FX);
            fxDic.Add(skill8FX.name, skill8FX);
            fxDic.Add(skill3Ground.name, skill3Ground);
            fxDic.Add(skill4Ground.name, skill4Ground);

        }
        

    }
    private void Update()
    {
        #region Input
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");
        //Vector2 _dir = new Vector2(h, v);
        //if (_dir != Vector2.zero)
        //{
        //    Dir = _dir;
        //    //playerAnim.Play("Run");
        //    SetBlend(1);
        //    //anim.SetFloat("Blend", 1);
        //    SetDir();
        //    SetMove();
        //    SetCamera();
        //}
        //else
        //{
        //    Dir = Vector2.zero;
        //    SetBlend(0);
        //    //anim.SetFloat("Blend", 0);
        //}
        #endregion
        CameraControl();
        ScrollView();
        if (currentBlend != targetBlend)
        {
            UpdateMixBlend();
            //Debug.Log(currentBlend + "TargetBlend:" + targetBlend);
        }
        if (isMove)
        {
            //设置主角朝向
            SetDir();
            //设置移动
            SetMove();
            //相机跟随
            SetCamera();
        }
        //技能移动位置
        if (skillMove)
        {
            if (isBlinkSkill)
            {
                SetSkillMove();
            }
            else
            {
                SetSkillBigMove();
            }
            SetCamera();
        }
    }
    private void SetDir()
    {//第二个参数是角色正面朝向，h=0,v=1,这里因为摄像机偏转了，
        //所以人物的朝向也需要加上摄像机的偏转，不然会出现朝向不一致
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1)) + camMainTrans.eulerAngles.y;
        Vector3 eulerAngle = new Vector3(0, angle, 0);
        transform.eulerAngles = eulerAngle;
    }
    private void SetMove()
    {
        ctrl.Move(transform.forward * Time.deltaTime * Constants.playerMoveSpeed);
        //解决不落地的问题，之前的检测到没落地才给方向的解决方案如果不运动的话还是有可能穿帮
        ctrl.Move(Vector3.down*Time.deltaTime*Constants.playerMoveSpeed);
    }
    //TODO，如果指定了目标位置，则朝向指定位置方向移动，否则后退
    private void SetSkillMove()
    {
        ctrl.Move(-transform.forward * Time.deltaTime * skillMoveSpeed);
    }
    //大位移技能，瞬移
    private void SetSkillBigMove()
    {
        //Vector3 dir = BattleSys.Instance.playerCtrlWnd.pos-transform.position;

        ctrl.Move(transform.forward*Time.deltaTime*skillMoveSpeed);
    }
    //重写父类的设置blend，因为玩家角色动画融合，使用了updateBlend进行细腻表现，怪物控制就不用了。
    private void UpdateMixBlend()
    {
        //如果当前值比目标值大就减小，比目标值小就增大，绝对值相差范围内设置相等
        //接近每帧插值范围，直接设置相等
        if (Mathf.Abs(currentBlend - targetBlend) < Constants.accelerateSpeed * Time.deltaTime)
        {
            currentBlend = targetBlend;
        }
        else if (currentBlend > targetBlend)
        {
            currentBlend -= Constants.accelerateSpeed * Time.deltaTime;
        }
        else
        {
            currentBlend += Constants.accelerateSpeed * Time.deltaTime;
        }
        anim.SetFloat("Blend", currentBlend);
    } 
    public override void SetBlend(float blend)
    {
        targetBlend = blend;
    }
    #region 摄像机控制
    public void SetCamera()
    {
        camMainTrans.transform.position = transform.position + cameraOffset;
    }
    private void CameraControl()
    {
        //Debug.Log(camMainTrans.position);
        Vector3 originalCamTrans = camMainTrans.position;
        Quaternion originalCamQua = camMainTrans.rotation;
        if (Input.GetMouseButtonDown(1))
        {
            isRotate = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isRotate = false;
        }
        if (isRotate)
        {
            camMainTrans.RotateAround(transform.position, transform.up, Input.GetAxis("Mouse X") * camRotSmooth);

            camMainTrans.RotateAround(transform.position, camMainTrans.right, -Input.GetAxis("Mouse Y") * camRotSmooth);
            float x = camMainTrans.eulerAngles.x;
            if (x > 80 || x < 10)
            {
                camMainTrans.position = originalCamTrans;
                camMainTrans.rotation = originalCamQua;
            }
        }
        cameraOffset = camMainTrans.position - transform.position;
    }
    private void ScrollView()
    {
        distance = cameraOffset.magnitude;
        distance -= Input.GetAxis("Mouse ScrollWheel") * disSmooth;
        distance = Mathf.Clamp(distance, 4, 15);
        cameraOffset = cameraOffset.normalized * distance;
        SetCamera();
        //Debug.Log(distance + "CameraOffset: " + cameraOffset);
    }
    #endregion
    public override void SetFX(string fxName, float closeTime)
    {
        GameObject go;
        if (fxDic.TryGetValue(fxName,out go))
        {
            go.SetActive(true);
            /*这里因为技能2和技能4是在出现以后位置就固定不变的
             但是出现的时候位置要跟人物相关，所以出现以后就解除了父子关系，消失的时候再添加回来*/
            if (fxName==skill2FX.name||fxName==skill4FX.name)
            {
                go.transform.GetChild(0).gameObject.SetActive(true);
                go.transform.DetachChildren();
            }
            //要判断必须是2技能和4技能，这里要修改
            timeSvc.AddTimeTask((int tid) =>
            {
                if (fxName == skill2FX.name)
                {
                    skill2Emp.SetActive(false);
                    skill2Emp.transform.SetParent(go.transform);
                    skill2Emp.transform.localPosition = Vector3.zero;
                    skill2Emp.transform.localRotation = Quaternion.identity;
                }
                else if (fxName==skill4FX.name)
                {
                    skill4Emp.SetActive(false);
                    skill4Emp.transform.SetParent(go.transform);
                    skill4Emp.transform.localPosition = Vector3.zero;
                    skill4Emp.transform.localRotation = Quaternion.identity;
                }
                go.SetActive(false);
            }, closeTime);
        }
    }
    public override void SetAreaSkillFX(EntityBase entity, string fxName, float beginTime, float closeTime)
    {
        GameObject go;
        if (fxDic.TryGetValue(fxName,out go))
        {
           int effectID= timeSvc.AddTimeTask((int tid) =>
            {
                go.SetActive(true);
                go.transform.position = BattleSys.Instance.playerCtrlWnd.pos;
                entity.RemoveEffectCallBake(tid);
            },beginTime);
            entity.skillEffectCallBackList.Add(effectID);
            int stopEffectID= timeSvc.AddTimeTask((int tid) =>
            {
                go.SetActive(false);
                entity.RemoveEffectCallBake(tid);
            },closeTime+beginTime);
            entity.skillEffectCallBackList.Add(effectID);
        }
    }
    private void InitSkillGroudFX()
    {
        skill3Ground = resSvc.LoadPrefab(PathDefine.skill3Path);
        skill3Ground.name = Constants.skill3Name;
        skill3Ground.SetActive(false);
        skill4Ground = resSvc.LoadPrefab(PathDefine.skill4Path);
        skill4Ground.name = Constants.skill4Name;
        skill4Ground.SetActive(false);
    }
}