using UnityEngine;
using System.Collections;

public class Vendor : Inventory {

    #region 单例模式
    private static Vendor _instance;
    public static Vendor Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("VendorPanel").GetComponent<Vendor>();
            }
            return _instance;
        }
    }
    #endregion

    public int[] itemIdArray;

    private Player player;

    public override void Start()
    {
        base.Start();
        InitShop();
        player = GameObject.Find("MainCityWnd").GetComponent<Player>();
        Hide();
    }

    private void InitShop()
    {
        foreach (int itemId in itemIdArray)
        {
            StoreItem(itemId);
        }
    }
    /// <summary>
    /// 主角购买
    /// </summary>
    /// <param name="item"></param>
    public void BuyItem(Item item)
    {
        bool isSuccess = player.ConsumeCoin(item.BuyPrice);
        if (isSuccess)
        {
            Knapsack.Instance.StoreItem(item);
        }
    }
    /// <summary>
    /// 主角出售物品
    /// </summary>
    public void SellItem()
    {
        int sellAmount = 1;
        if (Input.GetKey(KeyCode.LeftControl))
        {
            sellAmount = 1;
        }
        else
        {
            sellAmount = InventoryManager.Instance.PickedItem.Amount;
        }

        int coinAmount = InventoryManager.Instance.PickedItem.Item.SellPrice * sellAmount;
        player.EarnCoin(coinAmount);
        InventoryManager.Instance.RemoveItem(sellAmount);
    }
}
