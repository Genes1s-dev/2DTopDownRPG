using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPotion : Item
{
    [SerializeField] private int healingSmall = 20;
    [SerializeField] private int healingMedium = 50;
    [SerializeField] private int healingBig = 100;
    public override void Use()
    {
        switch (itemData.itemType)
        {
            case ItemSO.ItemType.consumableSmall:
                Player.Instance.RestoreHealth(healingSmall);
                break;
            case ItemSO.ItemType.consumableMedium:
                Player.Instance.RestoreHealth(healingMedium);
                break;
            case ItemSO.ItemType.consumableBig:
                Player.Instance.RestoreHealth(healingBig);
                break;
            default:
                break;
        }
        RemoveFromStack();
    }
    
}
