using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler //реализуем интерфейс для обработки кликов 
{
    private Item itemData;
    public void SetItemData(Item itemData)
    {
        this.itemData = itemData;
    }

    public Item GetItemData()
    {
        return itemData;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            itemData.Use();
            Inventory.Instance.RefreshNumberOfStacks(itemData);
            
            if (itemData.stackSize <= 0)
            {
                Inventory.Instance.RemoveItem(itemData);
            }
        }
    }
}
