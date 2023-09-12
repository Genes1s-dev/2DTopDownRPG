using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class PlayerBars : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Magic magicWeapon;
    [SerializeField] private float hpBarUpdateDuration = 2f;
    [SerializeField] Image hpBarFront;
    [SerializeField] Image hpBarBack;
    [SerializeField] Image energyMainBar;
    [SerializeField] Image energySecondaryBar;
    [SerializeField] Image expBar;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI expText;
 

    private void Start()
    {
        playerStats.OnHPChanged += Player_OnHPChanged;
        playerStats.OnEnergyChanged += Player_OnEnergyChanged;
        playerStats.OnExpGained += Player_OnExpGained;
        magicWeapon.OnChargingBegan += MagicWeapon_OnChargingBegan;
    }

    private void Player_OnHPChanged(object sender, PlayerStats.StatChangedEventArgs<int> e)
    {
        StopCoroutine(nameof(HPBarBackUpdateUp));
        StopCoroutine(nameof(HPBarBackUpdateDown));  //*предварительно останавливаем корутины 
        //для корректного отображания изменения ХП-бара 
        //(обновление UI ХП-бара должно происходить после последнего изменения, 
        //в случае если их произошло несколько за короткий промежуток времени (<2сек - длительности обновления бара)
        float playerHealth = e.currentValue;
        float playerMaxHP = e.maxValue;
        float hpPercentage = playerHealth / playerMaxHP;
        

        if (hpPercentage < hpBarBack.fillAmount)
        {
            hpBarBack.color = Color.red;
            hpBarFront.fillAmount = hpPercentage;
            StartCoroutine(nameof(HPBarBackUpdateDown));
        } else if (hpPercentage > hpBarBack.fillAmount)
        {
            hpBarBack.color = Color.green;
            hpBarBack.fillAmount = hpPercentage;
            StartCoroutine(nameof(HPBarBackUpdateUp)); 
        }
    }

    private IEnumerator HPBarBackUpdateDown()
    {
        float timer = 0.0f;
        float timePercentage = 0.0f;

        while (timer < hpBarUpdateDuration)
        {
            timePercentage = timer / hpBarUpdateDuration;
            hpBarBack.fillAmount = Mathf.Lerp(hpBarBack.fillAmount, hpBarFront.fillAmount, timePercentage);
            timer += Time.deltaTime;
            yield return null;
        } 
    }

    private IEnumerator HPBarBackUpdateUp()
    {
        float timer = 0.0f;
        float timePercentage = 0.0f;

        while (timer < hpBarUpdateDuration)
        {
            timePercentage = timer / hpBarUpdateDuration;
            hpBarFront.fillAmount = Mathf.Lerp(hpBarFront.fillAmount, hpBarBack.fillAmount, timePercentage);
            timer += Time.deltaTime;
            yield return null;
        } 
    }


    private void Player_OnEnergyChanged(object sender, PlayerStats.StatChangedEventArgs<float> e)
    {
        float playerEnergy = e.currentValue;
        float playerMaxEnergy = e.maxValue;
        float energyPercentage = playerEnergy / playerMaxEnergy; 

        energyMainBar.fillAmount = energyPercentage;
        energySecondaryBar.fillAmount = 1 - energyMainBar.fillAmount;
    }

    private void MagicWeapon_OnChargingBegan(object sender, Magic.EnergyChargedEventArgs e)
    {
        float energyCharged = e.currentEnergyConsump;
        float maxEnergy = e.maxEnergy;
        
        float energyChargedPercentage = energyCharged / maxEnergy;

        energySecondaryBar.fillAmount = (1 - energyMainBar.fillAmount) + energyChargedPercentage;
    }

    private void Player_OnExpGained(object sender, PlayerStats.StatChangedEventArgs<int> e)
    {
        float playerExp = e.currentValue;
        float playerMaxExp = e.maxValue;
        int playerLevel = e.level;

        float expPercentage = playerExp / playerMaxExp; 

        expBar.fillAmount = expPercentage;
        levelText.text = playerLevel.ToString();
        expText.text = playerExp.ToString() + "/" + playerMaxExp.ToString();
    }


}
