/****************************************************
    文件：DragonFlyAnima.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/10/13 15:46:38
	功能：Nothing
*****************************************************/

using UnityEngine;

public class DragonFlyAnima : MonoBehaviour 
{
    private Animation anima;
    private void Awake()
    {
        anima =transform.GetComponent<Animation>();
    }
    private void Start()
    {
        if (anima != null)
        {
            InvokeRepeating("DragonAnimatonLoop", 0, 20);
        }
    }
    private void DragonAnimatonLoop()
    {
        if (anima!=null)
        {
            anima.Play();
        }
    }
}