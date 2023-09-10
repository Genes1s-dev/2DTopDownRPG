using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ItemSO : ScriptableObject
{
    public string id;
    public new string name;

    [TextArea] public string description;

    public Sprite icon;

    [Range(0f, 1f)] public float droprate;
    
    public enum ItemType
    {
        consumableSmall,
        consumableMedium,
        consumableBig,
        weapon,
        artifact,
        key
    }
    public ItemType itemType;
}
