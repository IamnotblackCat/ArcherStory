/****************************************************
    文件：SkillExplainWnd.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2020/1/2 16:30:16
	功能：技能说明
*****************************************************/

using UnityEngine;
using UnityEngine.EventSystems;

public class SkillExplainWnd : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject go;
    private void Start()
    {
        go = transform.GetChild(1).gameObject;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        go.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        go.SetActive(false);
    }
}