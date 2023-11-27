using System.Collections;
using System.Collections.Generic;
using TMPro;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Inventory : MonoBehaviour
{
    public static Inventory Instance {get; private set;}
    private List<Item> itemsList = new List<Item>();
    private Player player;
    public Transform itemContent;
    public GameObject inventoryItem;

    private void Awake()
    {
        player = GetComponent<Player>();
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void AddItem(Item item)
    {
        if (!itemsList.Contains(item))
        {
            item.stackSize = 1;
            itemsList.Add(item);
            CreateItemSlot(item);
        } else {
            item.AddToStack();
            RefreshNumberOfStacks(item);
        }
    }

    public void RemoveItem(Item item)
    {
        itemsList.Remove(item);
        ListItems();
    }

    public void ClearList()
    {
        itemsList.Clear();
    }

    public List<Item> GetItemsList()
    {
        return itemsList;
    }

    public void ListItems()//?
    {
        //обновляем набор предметов при открытии инвентаря
        foreach (Transform item in itemContent)
        {
            Destroy(item.gameObject);
        }

        foreach (Item itemToCheck in itemsList)
        {
            /*if (itemContent.Find(itemToCheck.Value.ToString()) != null)
            {
                AddItemStack(itemToCheck.Key, itemToCheck.Value);
            } else {
                CreateItemSlot(itemToCheck.Key);
            }*/
            CreateItemSlot(itemToCheck);
        }
    }

    private void CreateItemSlot(Item item)
    {
        GameObject itemObject = Instantiate(inventoryItem, itemContent);//создание дочернего объекта 
        Image icon = itemObject.transform.Find("Icon").GetComponent<Image>();
        icon.sprite = item.GetItemSOData().icon;

        TextMeshProUGUI textUI = itemObject.transform.Find("QuantityText").GetComponent<TextMeshProUGUI>();
        textUI.text = item.stackSize.ToString();

        ItemSlot itemSlot;
        if (itemObject.TryGetComponent(out itemSlot))
        {
            itemSlot.SetItemData(item);
        }
    }

    public void RefreshNumberOfStacks(Item item)
    {
        int index = 0;

        for (int i = 0; i < itemContent.childCount; i++)
        {
            ItemSlot itemSlot = itemContent.GetChild(i).GetComponent<ItemSlot>();
            if (itemSlot != null)
            {
                if (itemSlot.GetItemData().GetItemSOData().id == item.GetItemSOData().id)
                {
                    index = i;
                    break;
                }
            }
        }

        TextMeshProUGUI textUI = itemContent.GetChild(index).transform.Find("QuantityText").GetComponent<TextMeshProUGUI>();
        textUI.text = item.stackSize.ToString();
    }
}
