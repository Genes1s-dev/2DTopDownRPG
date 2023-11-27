using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Dummy : MonoBehaviour, IDamageable
{
    private float maxHealth = 1000;
    private float currentHealth;
    [SerializeField] Image hpFillAmount;

    private void Start()
    {
        currentHealth = maxHealth;
    }
    

    public void TakeDamage(int damage, WeaponSO.Element element)
    {
        this.currentHealth -= damage;
        hpFillAmount.fillAmount = currentHealth / maxHealth;
        Debug.Log("Current hp: " + currentHealth);
        if (this.currentHealth <= 0)
        {
            DeathSequence();
        }
    }

    public void RestoreHealth(int healthToRestore)
    {
        currentHealth += healthToRestore;
        currentHealth = Mathf.Clamp(currentHealth, currentHealth, maxHealth);
        Debug.Log("Dummy HP received: " + healthToRestore);
    }

    public void DeathSequence()
    {
        Destroy(this.gameObject);
    }

  
}
