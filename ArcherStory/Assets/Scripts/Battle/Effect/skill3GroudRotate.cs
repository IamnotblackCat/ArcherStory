/****************************************************
    文件：skill3GroudRotate.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2020/1/8 14:34:33
	功能：Nothing
*****************************************************/

using UnityEngine;

public class skill3GroudRotate : MonoBehaviour 
{
    private float rotateSpeed = 500f;
    private void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
}