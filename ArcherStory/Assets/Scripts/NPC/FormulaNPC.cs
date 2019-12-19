/****************************************************
    文件：FormulaNPC.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/12/19 11:5:17
	功能：锻造NPC
*****************************************************/

using UnityEngine;

public class FormulaNPC : BaseNPC 
{
    public override void OpenNPCWindow()
    {
        Forge.Instance.Show();
    }
    public override void CloseNPCWindow()
    {
        Forge.Instance.Hide();
    }
}