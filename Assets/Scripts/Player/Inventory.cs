using System.Collections;
using System.Collections.Generic;
using TMPro;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Inventory : MonoBehaviour
{
    /*
    public static Inventory Instance {get; private set;}
    private Dictionary<ItemSlot, Transform> itemsDictionary = new Dictionary<ItemSlot, Transform>();
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

    
    public void AddItem(ItemSlot item)
    {
       // Debug.Log("Chest opened");
        if (!itemsDictionary.ContainsKey(item))
        {
            item.stackSize = 1;
            CreateItemSlot(item);
        } else {
            AddItemStack(item, itemsDictionary[item]);//?
        }
        Debug.Log(itemsDictionary.Count);
    }

    public void RemoveItem(ItemSlot item)
    {
        itemsDictionary.Remove(item);
    }

    public void UseItem()
    {
        //item.Use(player);
    }

    public void ClearList()
    {
        itemsDictionary.Clear();
    }

    public Dictionary<ItemSlot, Transform> GetItemsList()
    {
        return itemsDictionary;
    }

    public void ListItems()//?
    {
        //обновляем набор предметов при открытии инвентаря
        foreach (Transform item in itemContent)
        {
            Destroy(item.gameObject);
        }

        foreach (KeyValuePair<ItemSlot, Transform> itemToCheck in itemsDictionary)
        {
            /*if (itemContent.Find(itemToCheck.Value.ToString()) != null)
            {
                AddItemStack(itemToCheck.Key, itemToCheck.Value);
            } else {
                CreateItemSlot(itemToCheck.Key);
            }
            CreateItemSlot(itemToCheck.Key);
        }
    } 

    private void CreateItemSlot(ItemSlot item)
    {
        GameObject itemObject = Instantiate(inventoryItem, itemContent);//создание дочернего объекта 
        Image icon = itemObject.transform.Find("Icon").GetComponent<Image>();
        icon.sprite = item.itemData.icon;

        TextMeshProUGUI textUI = itemObject.transform.Find("QuantityText").GetComponent<TextMeshProUGUI>();
        textUI.text = item.stackSize.ToString();
        
        if (!itemsDictionary.ContainsKey(item))
        {
            itemsDictionary.Add(item, itemObject.transform);
        }
    }

    private void AddItemStack(ItemSlot item, Transform itemObjectSlot)
    {
        if (!itemsDictionary.ContainsKey(item))
        {
            itemsDictionary.Add(item, itemObjectSlot);
        }

        item.AddToStack();

        int index = 0;

        for (int i = 0; i < itemContent.childCount; i++)
        {
            if (itemContent.GetChild(i) == itemObjectSlot)
            {
                index = i;
                break;
            }
        }

        TextMeshProUGUI textUI = itemContent.GetChild(index).transform.Find("QuantityText").GetComponent<TextMeshProUGUI>();
        textUI.text = item.stackSize.ToString();
    }

    */
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
