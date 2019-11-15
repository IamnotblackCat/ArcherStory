/****************************************************
	文件：PETools.cs
	作者：Echo
	邮箱: 350383921@qq.com
	日期：2019/10/15 18:46   	
	功能：工具类，包含随机取值方法
*****************************************************/
using System;
using System.Collections.Generic;
using System.Text;


public class PETools
{
    //使用System是因为还有一个UnityEngine.Random
    public static int RDInt(int min,int max,System.Random rd=null)
    {//提高代码复用性设计？
        if (rd==null)
        {
            rd = new System.Random();
        }
        int val = rd.Next(min,max+1);
        return val;
    }
}
