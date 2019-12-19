using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// 物品槽
/// </summary>
public class Slot : MonoBehaviour ,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler{

    public GameObject itemPrefab;
    /// <summary>
    /// 把item放在自身下面
    /// 如果自身下面已经有item了，amount++
    /// 如果没有 根据itemPrefab去实例化一个item，放在下面
    /// </summary>
    /// <param name="item"></param>
    public void StoreItem(Item item)
    {
        if (transform.childCount == 0)
        {
            GameObject itemGameObject = Instantiate(itemPrefab) as GameObject;
            itemGameObject.transform.SetParent(this.transform);
            itemGameObject.transform.localScale = Vector3.one;
            itemGameObject.transform.localPosition = Vector3.zero;
            itemGameObject.GetComponent<ItemUI>().SetItem(item);
        }
        else
        {
            transform.GetChild(0).GetComponent<ItemUI>().AddAmount();
        }
    }


    /// <summary>
    /// 得到当前物品槽存储的物品类型
    /// </summary>
    /// <returns></returns>
    public Item.ItemType GetItemType()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().Item.Type;
    }

    /// <summary>
    /// 得到物品的id
    /// </summary>
    /// <returns></returns>
    public int GetItemId()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().Item.ID;
    }

    public bool IsFilled()
    {
        ItemUI itemUI = transform.GetChild(0).GetComponent<ItemUI>();
        return itemUI.Amount >= itemUI.Item.Capacity;//当前的数量大于等于容量
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(transform.childCount>0)
            InventoryManager.Instance.HideToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.childCount > 0)
        {
            string toolTipText = transform.GetChild(0).GetComponent<ItemUI>().Item.GetToolTipText();
            InventoryManager.Instance.ShowToolTip(toolTipText);
        }
        
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (InventoryManager.Instance.IsPickedItem==false&& transform.childCount > 0)
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                if (currentItemUI.Item is Equipment || currentItemUI.Item is Weapon)
                {
                    currentItemUI.ReduceAmount(1);
                    Item currentItem = currentItemUI.Item;
                    if (currentItemUI.Amount <= 0)
                    {
                        DestroyImmediate(currentItemUI.gameObject);
                        InventoryManager.Instance.HideToolTip();
                    }
                    CharacterPanel.Instance.PutOn(currentItem);   
                }
            }
        }

        if (eventData.button != PointerEventData.InputButton.Left) return;
        // 自身是空 1,IsPickedItem ==true  pickedItem放在这个位置
                            // 按下ctrl      放置当前鼠标上的物品的一个
                            // 没有按下ctrl   放置当前鼠标上的物品的所有
                 //2,IsPickedItem==false  不做任何处理
        // 自身不是空 
                 //1,IsPickedItem==true
                        //自身的id==pickedItem.id  
                                    // 按下ctrl      放置当前鼠标上的物品的一个
                                    // 没有按下ctrl   放置当前鼠标上的物品的所有
                                                    //可以完全放下
                                                    //只能放下其中一部分
                        //自身的id!=pickedItem.id   pickedItem跟当前物品交换          
                 //2,IsPickedItem==false
                        //ctrl按下 取得当前物品槽中物品的一半
                        //ctrl没有按下 把当前物品槽里面的物品放到鼠标上
        if (transform.childCount > 0)
        {
            ItemUI currentItem = transform.GetChild(0).GetComponent<ItemUI>();

            if (InventoryManager.Instance.IsPickedItem == false)//当前没有选中任何物品( 当前手上没有任何物品)当前鼠标上没有任何物品
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    int amountPicked = (currentItem.Amount + 1) / 2;
                    InventoryManager.Instance.PickupItem(currentItem.Item, amountPicked);
                    int amountRemained = currentItem.Amount - amountPicked;
                    if (amountRemained <= 0)
                    {
                        Destroy(currentItem.gameObject);//销毁当前物品
                    }
                    else
                    {
                        currentItem.SetAmount(amountRemained);
                    }
                }
                else
                {
                    InventoryManager.Instance.PickupItem(currentItem.Item,currentItem.Amount);
                    Destroy(currentItem.gameObject);//销毁当前物品
                }
            }else
            {
                //1,IsPickedItem==true
                    //自身的id==pickedItem.id  
                        // 按下ctrl      放置当前鼠标上的物品的一个
                        // 没有按下ctrl   放置当前鼠标上的物品的所有
                            //可以完全放下
                            //只能放下其中一部分
                    //自身的id!=pickedItem.id   pickedItem跟当前物品交换          
                if (currentItem.Item.ID == InventoryManager.Instance.PickedItem.Item.ID)
                {
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        if (currentItem.Item.Capacity > currentItem.Amount)//当前物品槽还有容量
                        {
                            currentItem.AddAmount();
                            InventoryManager.Instance.RemoveItem();
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (currentItem.Item.Capacity > currentItem.Amount)
                        {
                            int amountRemain = currentItem.Item.Capacity - currentItem.Amount;//当前物品槽剩余的空间
                            if (amountRemain >= InventoryManager.Instance.PickedItem.Amount)
                            {
                                currentItem.SetAmount(currentItem.Amount + InventoryManager.Instance.PickedItem.Amount);
                                InventoryManager.Instance.RemoveItem(InventoryManager.Instance.PickedItem.Amount);
                            }
                            else
                            {
                                currentItem.SetAmount(currentItem.Amount + amountRemain);
                                InventoryManager.Instance.RemoveItem(amountRemain);
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else
                {
                    Item item = currentItem.Item;
                    int amount = currentItem.Amount;
                    currentItem.SetItem(InventoryManager.Instance.PickedItem.Item, InventoryManager.Instance.PickedItem.Amount);
                    InventoryManager.Instance.PickedItem.SetItem(item, amount);
                }

            }
        }
        else
        {
            // 自身是空  
                        //1,IsPickedItem ==true  pickedItem放在这个位置
                            // 按下ctrl      放置当前鼠标上的物品的一个
                            // 没有按下ctrl   放置当前鼠标上的物品所有数量
                        //2,IsPickedItem==false  不做任何处理
            if (InventoryManager.Instance.IsPickedItem == true)
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    this.StoreItem(InventoryManager.Instance.PickedItem.Item);
                    InventoryManager.Instance.RemoveItem();
                }
                else
                {
                    for (int i = 0; i < InventoryManager.Instance.PickedItem.Amount; i++)
                    {
                        this.StoreItem(InventoryManager.Instance.PickedItem.Item);
                    }
                    InventoryManager.Instance.RemoveItem(InventoryManager.Instance.PickedItem.Amount);
                }
            }
            else
            {
                return;
            }

        }
    }
}
