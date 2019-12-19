/****************************************************
    文件：BaseNPC.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/12/19 10:48:14
	功能：NPC基础功能
*****************************************************/

using UnityEngine;

public class BaseNPC : MonoBehaviour
{
    private bool showWnd = false;
    private bool isOpened = false;

    public bool ShowWnd
    {
        get
        {
            return showWnd;
        }

        set
        {
            showWnd = value;
            //Debug.Log(value);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ShowWnd = true;
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag=="Player")
        {
            if (showWnd)//加这一行代码是因为离开的设置为false没有正确生效
            {
                showWnd = false;
            }
            GameRoot.instance.AddTips("请按F");
        }
    }
    private void OnTriggerStay(Collider collider)
    {
        if (collider.tag=="Player")
        {
            if (ShowWnd)
            {//保证只打开一次，这样用户手动关了以后不会再打开
                if (!isOpened)
                {
                    OpenNPCWindow();
                    isOpened = true;
                }
            }
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag=="Player")
        {
            isOpened = false;
            CloseNPCWindow();
        }   
    }
    public virtual void OpenNPCWindow()
    {

    }
    public virtual void CloseNPCWindow()
    {
    }

    
}