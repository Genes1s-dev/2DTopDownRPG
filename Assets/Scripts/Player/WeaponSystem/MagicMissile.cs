using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissile : MonoBehaviour
{
    private float damage;
    private bool hasHitTarget = false; //для исправления бага, при котором снаряд поражал цель, стоящую за ним
    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable hitObject = other.gameObject.GetComponent<IDamageable>();

        if (!other.isTrigger && hitObject == null)
        {
            Destroy(this.gameObject);
            hasHitTarget = true;
        }

        if (hitObject != null && !hasHitTarget)
        {
            Destroy(this.gameObject);
            hitObject.TakeDamage((int)damage, WeaponSO.Element.fire);
            Debug.Log("Damage dealt: " + damage);
        }


    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
