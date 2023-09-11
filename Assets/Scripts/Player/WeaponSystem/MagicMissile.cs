using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissile : MonoBehaviour
{
    private float damage;
    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable hitObject = other.gameObject.GetComponent<IDamageable>();
        if (hitObject != null)
        {
            hitObject.TakeDamage((int)damage, WeaponSO.Element.fire);
            Destroy(this.gameObject);

            Debug.Log("Damage dealt: " + damage);
        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
