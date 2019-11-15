/****************************************************
    文件：PEListener.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/10/25 17:26:10
	功能：UI点击监听事件
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PEListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler,IPointerClickHandler
{
    public Action<object> onClick;
    public Action<PointerEventData> onClickDown;
    public Action<PointerEventData> onClickUp;
    public Action<PointerEventData> onDrag;

    public object args;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (onClick!=null)
        {
            onClick(args);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (onClickDown!=null)
        {
            onClickDown(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (onClickUp != null)
        {
            onClickUp(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (onDrag != null)
        {
            onDrag(eventData);
        }

    }

}