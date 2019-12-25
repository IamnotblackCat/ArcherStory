using UnityEngine;
using System.Collections;
/// <summary>
/// 消耗品类
/// </summary>
public class Consumable : Item {

    //public int HP { get; set; }
    //public int MP { get; set; }
    public int EXP { get; set; }

    public Consumable(int id, string name, ItemType type, ItemQuality quality, string des, int capacity, int buyPrice, int sellPrice ,string sprite,int exp)
        :base(id,name,type,quality,des,capacity,buyPrice,sellPrice,sprite)
    {
        this.EXP = exp;
        //this.HP = hp;
        //this.MP = mp;
    }

    public override string GetToolTipText()
    {
        string text = base.GetToolTipText();

        string newText = string.Format("{0}\n\n<color=blue>加经验：{1}\n</color>", text, EXP);

        return newText;
    }

    public override string ToString()
    {
        string s = "";
        s += ID.ToString();
        s += Type;
        s += Quality;
        s += Description;
        s += Capacity; 
        s += BuyPrice;
        s += SellPrice;
        s += Sprite;
        //s += HP;
        //s += MP;
        return s;
    }

}
