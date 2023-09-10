using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Arrow : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponSO;
    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable hitObject = other.gameObject.GetComponent<IDamageable>();
        if (hitObject != null)
        {
            //collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-collision.relativeVelocity, ForceMode2D.Impulse);
            hitObject.TakeDamage(weaponSO.damage, weaponSO.element);
            Destroy(this.gameObject);
        }

    }
}
