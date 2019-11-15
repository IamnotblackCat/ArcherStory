/****************************************************
    文件：DynamicWnd.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/10/15 9:56:39
	功能：提示信息面板
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicWnd : WindowRoot 
{
    public Animation tipsAni;
    public Text tipsTxt;

    private bool isTipsShow = false;
    private Queue<string> tipsQueue = new Queue<string>();
    private void Update()
    {
        if (tipsQueue.Count>0&&isTipsShow==false)
        {
            lock (tipsQueue)
            {
                SetTips(tipsQueue.Dequeue());
                isTipsShow = true;
            }
        }
    }
    public void AddTips(string tips)
    {
        tipsQueue.Enqueue(tips);
    }
    protected override void InitWnd()
    {
        base.InitWnd();

        SetActive(tipsTxt,false);
    }

    private void SetTips(string tips)
    {
        SetActive(tipsTxt);
        SetText(tipsTxt,tips);
        //动画播放完成以后隐藏
        //这里分开两部写是为了以后自己能看懂，原理是为了取得动画的时间长度
        AnimationClip aniClip = tipsAni.GetClip("texTips");
        tipsAni.Play();

        StartCoroutine(AniPlayDone(aniClip.length,()=>
        {
            SetActive(tipsTxt,false);
            isTipsShow = false;
        }));

    }
    //协程执行，参数用委托原因是增加代码可复用性，将执行代码延迟到调用部分
    private IEnumerator AniPlayDone(float sec,Action cb)
    {
        yield return new WaitForSeconds(sec);
        if (cb!=null)
        {
            cb();
        }
    }
}