using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterPanel : Inventory
{
    #region 单例模式
    private static CharacterPanel _instance;
    public static CharacterPanel Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    private Text propertyText;

    private Player player;

    private ChangeSkinSys skinChange;
    public override void Start()
    {
        base.Start();
        _instance = transform.GetComponent<CharacterPanel>();
        player = GameObject.Find("MainCityWnd").GetComponent<Player>();
    }


    public void PutOn(Item item)
    {
        Item exitItem = null;
        foreach (Slot slot in slotList)
        {
            EquipmentSlot equipmentSlot = (EquipmentSlot)slot;
            if (equipmentSlot.IsRightItem(item))
            {
                if (equipmentSlot.transform.childCount > 0)
                {
                    ItemUI currentItemUI= equipmentSlot.transform.GetChild(0).GetComponent<ItemUI>();
                    exitItem = currentItemUI.Item;
                    currentItemUI.SetItem(item, 1);
                }
                else
                {
                    equipmentSlot.StoreItem(item);
                }
                break;
            }
        }
        if(exitItem!=null)
            Knapsack.Instance.StoreItem(exitItem);
            UpdateSkinned();
    }
    
    public void PutOff(Item item)
    {
        Knapsack.Instance.StoreItem(item);
        UpdateSkinned();
    }

    private void UpdateSkinned()
    {
        foreach (EquipmentSlot slot in slotList)
        {
            if (slot.transform.childCount>0)
            {
                Item item = slot.transform.GetChild(0).GetComponent<ItemUI>().Item;
                if (item is Weapon)
                {
                    ChangeSkinSys.Instance.ChangeWeaponSkinToNew();
                    GameRoot.instance.isNewBow = true;
                }
            }
            else
            {
                if (slot.wpType==Weapon.WeaponType.MainHand)
                {
                    ChangeSkinSys.Instance.ChangeWeaponSkinToOld();
                    GameRoot.instance.isNewBow = false;
                }
            }
        }
    }
}
