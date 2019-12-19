/****************************************************
    文件：ChestNPC.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/12/19 11:11:52
	功能：仓库NPC
*****************************************************/

using UnityEngine;

public class ChestNPC : BaseNPC 
{
    public override void OpenNPCWindow()
    {
        Chest.Instance.Show();
    }
    public override void CloseNPCWindow()
    {
        Chest.Instance.Hide();
    }
}