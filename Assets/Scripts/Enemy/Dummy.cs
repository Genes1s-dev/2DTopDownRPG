using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Dummy : MonoBehaviour, IDamageable
{
    private int maxHealth = 1000;
    private int currentHealth;
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
    }

    public void DeathSequence()
    {
        //
    }

  
}
