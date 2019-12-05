/****************************************************
	文件：EntityMonster.cs
	作者：Echo
	邮箱: 350383921@qq.com
	日期：2019/12/04 15:09   	
	功能：怪物逻辑实体
*****************************************************/
using System;
using System.Collections.Generic;
using System.Text;


public class EntityMonster:EntityBase
{
    public MonsterData md;
    //怪物子类重写了这个方法，让怪物在不同地图受等级影响属性
    public override void SetBattleProps(BattleProps props)
    {
        int level = md.mLevel;
        BattleProps p = new BattleProps
        {
            hp=props.hp*level,
            attackValue=props.attackValue*level,
            defend=props.defend*level,
            critical=props.critical*level,
            
        };
        props = p;
        Hp = p.hp;
        AttackValue = p.attackValue;
    }
}
