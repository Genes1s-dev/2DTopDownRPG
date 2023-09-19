using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Arrow : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponSO;
    private bool hasHitTarget = false; //для исправления бага, при котором стрела поражала цель, стоящую за ней
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
            hitObject.TakeDamage(weaponSO.damage, weaponSO.element);
        }
    }
}
