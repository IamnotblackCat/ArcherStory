/****************************************************
    文件：PlayerMoveTest.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/10/29 11:37:58
	功能：主角移动脚本
*****************************************************/

using UnityEngine;

public class PlayerController : MonoBehaviour 
{

    public Animator anim;
    public CharacterController ctrl;

    #region 相机控制
    private Transform camMainTrans;
    private float camRotSmooth = 3f;
    private bool isRotate;
    private float distance;
    private float disSmooth = 10f;
    #endregion
    private Animation playerAnim;
    private bool isMove = false;

    private Vector2 dir=Vector2.zero;
    private Vector3 cameraOffset = Vector3.zero;
    //平滑动画混合树用到的值
    private float currentBlend = 0;
    private float targetBlend = 0;
    public Vector2 Dir
    {
        get
        {
            return dir;
        }
        set
        {
            dir = value;
            if (value==Vector2.zero)
            {
                isMove = false;
            }
            else
            {
                isMove = true;
            }
        }
    }

    public void Init()
    {
        camMainTrans = Camera.main.transform;
        cameraOffset = camMainTrans.transform.position - transform.position;
        playerAnim = GetComponent<Animation>();
    }
    private void Update()
    {
        #region Input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector2 _dir = new Vector2(h, v);
        if (_dir != Vector2.zero)
        {
            Dir = _dir;
            playerAnim.Play("Run");
            SetDir();
            SetMove();
            SetCamera();
            //SetBlend(1);
        }
        else
        {
            Dir = Vector2.zero;
                int value = Random.Range(1, 5);
            if (Input.GetMouseButtonDown(0))
            {
                playerAnim.Play("Attack" + value);
            }
            else
            {
                if (playerAnim.IsPlaying("Run"))
                {
                    playerAnim.Play("Idle");
                }
                playerAnim.PlayQueued("Idle");
            }
            //SetBlend(0);
        }
        CameraControl();
        ScrollView();
        #endregion
        //if (currentBlend!=targetBlend)
        //{
        //    UpdateMixBlend();
        //}
        //if (isMove)
        //{
        //    //设置主角朝向
        //    SetDir();
        //    //设置移动
        //    SetMove();

        //    //相机跟随
        //    SetCamera();

        //}
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
    public void SetBlend(float blend)
    {
        targetBlend = blend;
    }
    private void CameraControl()
    {
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
}