/****************************************************
	文件：Controller.cs
	作者：Echo
	邮箱: 350383921@qq.com
	日期：2019/11/28 16:12   	
	功能：控制器抽象基类
*****************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class Controller:MonoBehaviour
{
    public Animator anim;
    protected bool isMove;
    private Vector2 dir = Vector2.zero;
    public Vector2 Dir
    {
        get
        {
            return dir;
        }
        set
        {
            dir = value;
            if (value == Vector2.zero)
            {
                isMove = false;
            }
            else
            {
                isMove = true;
            }
        }
    }
    public virtual void SetBlend(float blend)
    {
        anim.SetFloat("Blend",blend);
    }
}
