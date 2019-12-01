/****************************************************
    文件：PlayerMoveTest.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/10/29 11:37:58
	功能：主角移动脚本
*****************************************************/

using UnityEngine;

public class PlayerController : Controller 
{
    public GameObject skill2FX;
    public GameObject skill2Emp;
    public GameObject skill4FX;
    public GameObject skill4Emp;
    public CharacterController ctrl;

    #region 相机控制
    private Transform camMainTrans;
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
        if (skill2FX!=null)
        {
            fxDic.Add(skill2FX.name,skill2FX);
            //fxDic.Add(skill4FX.name,skill4FX);
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
    }
    private void SetDir()
    {//第二个参数是角色正面朝向，h=0,v=1,这里因为摄像机偏转了，
        //所以人物的朝向也需要加上摄像机的偏转，不然会出现朝向不一致
        float angle = Vector2.SignedAngle(Dir,new Vector2(0,1))+camMainTrans.eulerAngles.y;
        Vector3 eulerAngle = new Vector3(0,angle,0);
        transform.eulerAngles = eulerAngle;
    }
    private void SetMove()
    {
        ctrl.Move(transform.forward*Time.deltaTime*Constants.playerMoveSpeed);
        if (!ctrl.isGrounded)
        {
            ctrl.Move(-transform.up * Time.deltaTime*10f);
        }
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
        //由运动状态转向idle状态
        else if (currentBlend > targetBlend)
        {
            currentBlend -= Constants.accelerateSpeed * Time.deltaTime;
        }
        //由idle转化为运动
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
                go.transform.DetachChildren();
            }
            //TODO,要判断必须是2技能和4技能，这里要修改
            timeSvc.AddTimeTask((int tid) =>
            {
                if (fxName == skill2FX.name)
                {
                    skill2Emp.transform.SetParent(go.transform);
                    skill2Emp.transform.localPosition = Vector3.zero;
                    skill2Emp.transform.localRotation = Quaternion.identity;
                }
                else if (fxName==skill4FX.name)
                {
                    skill4Emp.transform.SetParent(go.transform);
                    skill4Emp.transform.localPosition = Vector3.zero;
                    skill4Emp.transform.localRotation = Quaternion.identity;
                }
                go.SetActive(false);
            }, closeTime);
        }
    }
}