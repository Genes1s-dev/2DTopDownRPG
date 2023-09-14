using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissile : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponSO;
    private float damage;
    private bool hasHitTarget = false; //для исправления бага, при котором снаряд поражал цель, стоящую за ней
    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable hitObject = other.gameObject.GetComponent<IDamageable>();

        if (!other.isTrigger && hitObject == null)
        {
            Destroy(this.gameObject);
            hasHitTarget = true;
            Debug.Log("Hit wall");
        }

        if (hitObject != null && !hasHitTarget)
        {
            Destroy(this.gameObject);
            hitObject.TakeDamage((int)damage, weaponSO.element);
            Debug.Log("Hit destructable");
        }


    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
