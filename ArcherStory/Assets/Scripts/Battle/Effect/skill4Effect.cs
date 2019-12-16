/****************************************************
    文件：skill4Effect.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/12/13 18:33:52
	功能：4技能特效：旋转+追踪
*****************************************************/

using UnityEngine;

public class skill4Effect : MonoBehaviour 
{
    private float speed = 300f;
    private Vector3 targetPos;
    private bool canMove = false;

    private void OnEnable()
    {
        targetPos = BattleSys.Instance.playerCtrlWnd.pos;
        transform.LookAt(targetPos);
        //1.5秒以后开始向目标地点移动
        TimeService.instance.AddTimeTask((int tid)=>
        {
            canMove = true;
        },1.5f);
        TimeService.instance.AddTimeTask((int tid) =>
        {
            canMove = false;
        }, 2f);
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward*speed*Time.deltaTime);
        if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position,targetPos,speed/4*Time.deltaTime);
        }
    }
}
    
            