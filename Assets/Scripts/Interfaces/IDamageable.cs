using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{
    public void TakeDamage(int damage, WeaponSO.Element element);
    public void RestoreHealth(int damage);
    public void DeathSequence();
}
