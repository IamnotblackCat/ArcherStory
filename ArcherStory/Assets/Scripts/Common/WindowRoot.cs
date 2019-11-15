/****************************************************
    文件：WindowRoot.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/10/13 9:38:22
	功能：UI界面基类
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowRoot : MonoBehaviour
{
    protected ResSvc resSvc = null;
    protected AudioSvc audioSvc = null;
    protected NetService netService = null;

    public void SetWndState(bool isActive = true)
    {
        if (gameObject.activeSelf != isActive)
        {
            SetActive(gameObject, isActive);
        }
        if (isActive)
        {
            InitWnd();
        }
        else
        {
            ClearWnd();
        }
    }
    protected virtual void InitWnd()
    {
        resSvc = ResSvc.instance;
        audioSvc = AudioSvc.instance;
        netService = NetService.instance;
    }
    //窗口关闭的时候要释放引用。 
    protected virtual void ClearWnd()
    {
        resSvc = null;
        audioSvc = null;
        netService = null;
    }
    //UI的一些设置方法的重载，可以快速设置UI的激活和目标
    #region ToolFunction
    protected void SetActive(GameObject go, bool isActive = true)
    {
        go.SetActive(isActive);
    }
    protected void SetActive(Transform trans, bool isActive = true)
    {
        trans.gameObject.SetActive(isActive);
    }
    protected void SetActive(RectTransform rect, bool isActive = true)
    {
        rect.gameObject.SetActive(isActive);
    }
    protected void SetActive(Image img, bool isActive = true)
    {
        img.gameObject.SetActive(isActive);
    }
    protected void SetActive(Text txt, bool isActive = true)
    {
        txt.gameObject.SetActive(isActive);
    }
    protected void SetText(Text text, string context = "")
    {
        text.text = context;
    }
    protected void SetText(Transform trans, string context = "")
    {
        SetText(trans.GetComponent<Text>(), context);
    }
    protected void SetText(Transform trans, int num = 0)
    {
        SetText(trans.GetComponent<Text>(), num.ToString());
    }
    protected void SetText(Text text, int num = 0)
    {
        SetText(text, num.ToString());
    }
    //加载图片
    protected void SetSprite(Image image,string path)
    {
        Sprite sp = resSvc.LoadSprite(path,true);
        image.sprite = sp;
    }
    #endregion
    protected T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T t = go.GetComponent<T>();
        if (t==null)
        {
            t = go.AddComponent<T>();
        }
        return t;
    }
    #region Click Events
    protected void OnClick(GameObject go,Action<object> cb, object args)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClick = cb;
        listener.args = args;
    }

    protected void OnClickDown(GameObject go, Action<PointerEventData> cb)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClickDown = cb;
    }
    protected void OnClickUP(GameObject go, Action<PointerEventData> cb)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClickUp = cb;
    }
    protected void OnDrag(GameObject go, Action<PointerEventData> cb)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onDrag = cb;
    }
    #endregion
    
}