using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemSO;

public class Item : MonoBehaviour
{
    protected Player player;
    [SerializeField] protected ItemSO itemData;
    public int stackSize {get; set;}

    private void Awake()
    {
        stackSize = 0;
        player = Player.Instance;
    }

    public virtual void Use(){}

    public void AddToStack()
    {
        stackSize++;
    }

    public void RemoveFromStack()
    {
        stackSize--;
    }

    public ItemSO GetItemSOData()
    {
        return itemData;
    }


}
