using UnityEngine;
using System.Collections;

/// <summary>
/// 武器
/// </summary>
public class Weapon : Item {

    public int Damage { get; set; }

    //public WeaponType WpType { get; set; }

    public Weapon(int id, string name, ItemType type, ItemQuality quality, string des, int capacity, int buyPrice, int sellPrice,string sprite,
     int damage  )
        : base(id, name, type, quality, des, capacity, buyPrice, sellPrice,sprite)
    {
        Damage = damage;
    }

    public enum WeaponType
    {
        None,
        OffHand,
        MainHand
    }


    public override string GetToolTipText()
    {
        string text = base.GetToolTipText();

        string wpTypeText = "弓";
        
        string newText = string.Format("{0}\n\n<color=blue>武器类型：{1}\n攻击力：{2}</color>", text, wpTypeText,Damage);

        return newText;
    }
}
