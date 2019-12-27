/****************************************************
	文件：MonsterController.cs
	作者：Echo
	邮箱: 350383921@qq.com
	日期：2019/12/04 15:08   	
	功能：怪物表现实体控制器
*****************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MonsterController:Controller
{
    public GameObject unBreakableFX;
    private void Update()
    {
        if (isMove)
        {
            SetDir();
            SetMove();
        }
    }
    private void SetDir()
    {
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1));
        Vector3 eulerAngle = new Vector3(0, angle, 0);
        transform.eulerAngles = eulerAngle;
    }
    private void SetMove()
    {
        ctrl.Move(transform.forward * Time.deltaTime * Constants.monsterMoveSpeed);
        ctrl.Move(Vector3.down * Time.deltaTime * Constants.monsterMoveSpeed);
    }
    //TODO,设置霸体特效的显示
    public override void SetFX(string fxName, float closeTime)
    {
        
    }
}
