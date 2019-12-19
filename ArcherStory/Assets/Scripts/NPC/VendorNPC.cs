/****************************************************
    文件：VendorNPC.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/12/19 10:52:41
	功能：商店
*****************************************************/

using UnityEngine;

public class VendorNPC : BaseNPC
{
    public override void OpenNPCWindow()
    {
        Vendor.Instance.Show();
    }
    public override void CloseNPCWindow()
    {
        Vendor.Instance.Hide();
    }
    
}