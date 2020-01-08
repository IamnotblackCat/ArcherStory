/****************************************************
    文件：lvUpEffect.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2020/1/8 11:49:58
	功能：Nothing
*****************************************************/

using UnityEngine;

public class lvUpEffect : MonoBehaviour 
{
    private void OnEnable()
    {
        TimeService.instance.AddTimeTask((int tid) => 
        {
            gameObject.SetActive(false);
        }, 1.0f);
    }
}