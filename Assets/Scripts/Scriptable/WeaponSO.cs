using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class WeaponSO : ScriptableObject
{
    public enum Type
    {
        sword,
        bow,
        magic
    }

    public enum Element
    {
        physic,
        fire,
        ice
    }
    public new string name;
    public Sprite icon;
    public float hitCooldown;
    public int damage;
    public Transform prefab;
    public float projectileFlightSpeed;
    public float moveSpeedPenaltyCoef;
    public Type type;
    public Element element;
}
