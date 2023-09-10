using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO itemSO;
    public void Interact()
    {
        Destroy(this.gameObject);
        //Inventory.Instance.AddItem(itemSO);
    }
}
