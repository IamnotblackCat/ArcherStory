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
    

    public void Init()
    {
        camMainTrans = Camera.main.transform;
        cameraOffset = camMainTrans.transform.position - transform.position;
        //playerAnim = GetComponent<Animation>();
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
    public void SetCamera()
    {
        camMainTrans.transform.position = transform.position + cameraOffset;
    }
    //重写父类的设置blend，因为玩家角色动画融合，使用了updateBlend进行细腻表现，怪物控制就不用了。
    public override void SetBlend(float blend)
    {
        targetBlend = blend;
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
            camMainTrans.RotateAround(transform.position, transform.up,Input.GetAxis("Mouse X")*camRotSmooth);

            camMainTrans.RotateAround(transform.position,camMainTrans.right,-Input.GetAxis("Mouse Y")*camRotSmooth);
            float x = camMainTrans.eulerAngles.x;
            if (x>80||x<10)
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
        distance = Mathf.Clamp(distance,4,15);
        cameraOffset = cameraOffset.normalized * distance;
        SetCamera();
        //Debug.Log(distance + "CameraOffset: " + cameraOffset);
    }
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
}