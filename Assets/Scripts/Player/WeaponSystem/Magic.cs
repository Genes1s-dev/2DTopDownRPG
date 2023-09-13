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
            Player.Instance.GetAnimator().SetFloat("ChargeState", 0f);
        } else 
        if (chargingTimer >= 2.0f && chargingTimer <= 3.0f)
        {
            Player.Instance.GetAnimator().SetFloat("ChargeState", 1f);
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

    public override WeaponSO GetWeaponSO()
    {
        return weaponSO;
    }

    private void ResetDamageToDefault()
    {
        this.damage = weaponSO.damage;
    }




}
