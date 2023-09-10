using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPotion : Item
{
    [SerializeField] private int energyRegenSmall = 15;
    [SerializeField] private int energyRegenMedium = 30;
    [SerializeField] private int energyRegenBig = 50;
    public override void Use()
    {
        switch (itemData.itemType)
        {
            case ItemSO.ItemType.consumableSmall:
                Player.Instance.RestoreEnergy(energyRegenSmall);
                break;
            case ItemSO.ItemType.consumableMedium:
                Player.Instance.RestoreEnergy(energyRegenMedium);
                break;
            case ItemSO.ItemType.consumableBig:
                Player.Instance.RestoreEnergy(energyRegenBig);
                break;
            default:
                break;
        }
        RemoveFromStack();
    }
    
}
