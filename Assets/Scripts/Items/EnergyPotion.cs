using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPotion : Item
{
    [SerializeField] private float energyRegenSmall = 15f;
    [SerializeField] private float energyRegenMedium = 30f;
    [SerializeField] private float energyRegenBig = 50f;
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
