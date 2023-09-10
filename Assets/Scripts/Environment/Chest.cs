using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TMPro;

public class Chest : MonoBehaviour, IInteractable
{
    private Inventory _playerInventory;
    private List<Item> chestItems = new List<Item>();
    [SerializeField] private List<Item> possibleItems;
    [SerializeField] private GameObject openedChest;
    [SerializeField] private GameObject closedChest;
    [SerializeField] private TextMeshProUGUI interactionText;
    private bool isOpened = false;

    /*[Inject]
    public void InjectDependencies(Inventory inventory)
    {
        _playerInventory = inventory;
    }*/

    public void Interact()
    {
        if (!isOpened)
        {
            OpenChest();
        }
        isOpened = true;
        interactionText.gameObject.SetActive(false);
        closedChest.SetActive(false);
        openedChest.SetActive(true);
    }

    private void OpenChest()
    {
        // Генерация и добавление предметов в инвентарь
        GenerateRandomItems();
        foreach(Item item in chestItems)
        {
            Debug.Log(item.GetItemSOData().name);
            Inventory.Instance.AddItem(item);
        }

        chestItems.Clear();
        
    }

    private void GenerateRandomItems()
    {
        foreach (Item possibleItem in possibleItems)
        {
            float randomValue = Random.value;
            if (randomValue < possibleItem.GetItemSOData().droprate)
            {
                chestItems.Add(possibleItem);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isOpened)
        {
            interactionText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interactionText.gameObject.SetActive(false);
        }
    }
}
