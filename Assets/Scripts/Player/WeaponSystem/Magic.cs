using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Magic : ChargableWeapon
{
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private MagicMissile magicMissilePrefab;
    protected float damage;
    private float amplCoef = 1.5f;
    private enum ChargeState
    {
        firstPhase,
        secondPhase,
        thirdPhase,
        finalPhase
    }

    private ChargeState currentChargeState;
    private float chargingTimer = 0.0f;
    private float baseEnergyConsumption = 10f;
    public class EnergyChargedEventArgs : EventArgs  //делаем дженерик класс для наших передаваемых аргументов для большей гибкости и во избежания дублирования кода
    {
        public float currentEnergyConsump;
        public float maxEnergy;
    }  
    public event EventHandler<EnergyChargedEventArgs> OnChargingBegan;  


    private void Awake()
    {
        WeaponType = Type.magic;
        Cooldown = weaponSO.hitCooldown;
    }

    private void Start()
    {
        damage = weaponSO.damage;
    }

    private void Update()
    {
        if (Player.Instance.isCharging && !IsCooldown)
        {
            Aim();
        } 
    }

    public override void Aim()
    {
        if (Player.Instance.GetPlayerStats().Energy > energyConsumption && chargingTimer <= 4.0f)
        {
            damage += Time.deltaTime * 10;
            energyConsumption = damage - weaponSO.damage;
        }

        OnChargingBegan?.Invoke(this, new EnergyChargedEventArgs {currentEnergyConsump = energyConsumption, maxEnergy = Player.Instance.GetPlayerStats().Energy});


        chargingTimer += Time.deltaTime;
        if (chargingTimer <= 1.0f)
        {
            currentChargeState = ChargeState.firstPhase;
            Player.Instance.GetAnimator().SetFloat("ChargeState", 0f);
        } else 
        if (chargingTimer <= 2.0f)
        {
            currentChargeState = ChargeState.secondPhase;
        } else
        if (chargingTimer <= 3.0f)
        {
            currentChargeState = ChargeState.thirdPhase;
            Player.Instance.GetAnimator().SetFloat("ChargeState", 1f);
        } else 
        {
            currentChargeState = ChargeState.finalPhase;
        }
    }

    public override void Hit()
    {
        chargingTimer = 0.0f;

        Vector2 playerLastMoveInput = Player.Instance.GetLastMoveInput();
        MagicMissile magicMissile = Instantiate(magicMissilePrefab, Player.Instance.transform.position, Quaternion.identity);
        magicMissile.transform.Rotate(0, 0, Mathf.Atan2(playerLastMoveInput.y, playerLastMoveInput.x) * Mathf.Rad2Deg);

        magicMissile.GetComponent<Rigidbody2D>().velocity = playerLastMoveInput * weaponSO.projectileFlightSpeed;
        
        magicMissile.SetDamage(damage);

        IsCooldown = true;
        StartCoroutine(WeaponHitManagement(weaponSO));

        Destroy(magicMissile.gameObject, 2f);
        ResetDamageToDefault();
        energyConsumption = 0;
    }


    private float CalculateDamage(ChargeState currentState)
    {
        float dmg = 0;
        switch(currentState)
        {
            case ChargeState.firstPhase:
                ResetDamageToDefault();  //10
                break;
            case ChargeState.secondPhase:
                dmg = Mathf.Floor(damage * amplCoef);  //15
                break;
            case ChargeState.thirdPhase:
                dmg = Mathf.Floor(damage * Mathf.Pow(amplCoef, 2));   //22
                break;
            case ChargeState.finalPhase:
                dmg = Mathf.Floor(damage * Mathf.Pow(amplCoef, 3));  //33
                break;
            default:
                dmg = 1f;
                break;
        }
        return dmg;
    }


    private float CalculateEnergyConsumption(ChargeState currentState)
    {
        float energyCons = 0;
        switch(currentState)
        {
            case ChargeState.firstPhase:
                energyCons = baseEnergyConsumption; //10
                break;
            case ChargeState.secondPhase:
                energyCons = baseEnergyConsumption * amplCoef; //15
                break;
            case ChargeState.thirdPhase:
                energyCons = baseEnergyConsumption * 2; //20
                break;
            case ChargeState.finalPhase:
                energyCons = baseEnergyConsumption * amplCoef * 2; //30
                break;
            default: 
                energyCons = baseEnergyConsumption;
                break;
        }
        return energyCons;
    }


    public override WeaponSO GetWeaponSO()
    {
        return weaponSO;
    }

    private void ResetDamageToDefault()
    {
        this.damage = weaponSO.damage;
    }




}
