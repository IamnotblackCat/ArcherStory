/****************************************************
    文件：TimeService.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/11/30 15:15:41
	功能：计时服务
*****************************************************/

using System;
using UnityEngine;

public class TimeService : SystemRoot 
{
    public static TimeService instance = null;
    private PETimer pt;

    public void InitSvc()
    {
        instance = this;
        pt = new PETimer();
        //日志输出
        pt.SetLog((string info)=>
        {
            Debug.Log(info);
        });
    }
    private void Update()
    {
        pt.Update();
    }
    public int AddTimeTask(Action<int> callback, double delay, PETimeUnit timeUnit = PETimeUnit.Second, int count = 1)
    {
        return pt.AddTimeTask(callback,delay,timeUnit,count);
    }
    public void DeleteTask(int tid)
    {
        pt.DeleteTimeTask(tid);
    }
    public double GetNowTime()
    {
        return pt.GetMillisecondsTime();
    }
}