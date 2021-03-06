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
    public Transform hpItemRoot;

    private bool isTipsShow = false;
    private Queue<string> tipsQueue = new Queue<string>();
    private Dictionary<string, ItemEntityHP> itemDic = new Dictionary<string, ItemEntityHP>();
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
    public void AddHPItemInfo(string mName,int hp,Transform trans,bool isBoss=false)
    {
        ItemEntityHP item = null;
        if (itemDic.TryGetValue(mName,out item))
        {
            return;
        }
        else
        {
            GameObject go = resSvc.LoadPrefab(PathDefine.HPDynamic);
            go.transform.SetParent(hpItemRoot);
            go.transform.localPosition = new Vector3(-1000,0,0);
            ItemEntityHP itemCom = go.GetComponent<ItemEntityHP>();
            itemCom.InitItemInfo(trans, hp,isBoss);//boss血条不显示，但还要出伤害
            itemDic.Add(mName,itemCom);
        }
    }
    public void RemoveHPItemInfo(string mName)
    {
        ItemEntityHP item = null;
        if (itemDic.TryGetValue(mName,out item))
        {
            //在面板中摧毁血条UI
            Destroy(item.gameObject);
            //字典移除血条
            itemDic.Remove(mName);
        }
    }
    public void RemoveAllHPItemInfo()
    {
        foreach (var item in itemDic)
        {
            Destroy(item.Value.gameObject);
        }
        itemDic.Clear();
    }
    public void SetCritical(string key, int critical)
    {
        ItemEntityHP item = null;
        if (itemDic.TryGetValue(key, out item))
        {
            item.SetCritical(critical);
        }
    }
    public void SetHurt(string key, int hurt)
    {
        ItemEntityHP item = null;
        if (itemDic.TryGetValue(key, out item))
        {
            item.SetHPDownTxt(hurt);
        }
    }
    public void SetHPVal(string key,int oldVal,int newVal)
    {
        ItemEntityHP item = null;
        if (itemDic.TryGetValue(key,out item))
        {
            item.SetHPVal(oldVal,newVal);
        }
    }
}