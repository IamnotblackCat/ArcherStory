/****************************************************
    文件：skill3Rotate.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/12/13 18:19:24
	功能：3技能的特效旋转脚本
*****************************************************/

using UnityEngine;

public class skill3Rotate : MonoBehaviour 
{
    private float rotateSpeed = 500f;
    private void Update()
    {
        transform.Rotate(-Vector3.forward*rotateSpeed*Time.deltaTime);
    }
}