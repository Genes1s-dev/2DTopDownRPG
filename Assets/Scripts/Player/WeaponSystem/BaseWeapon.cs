using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseWeapon : MonoBehaviour
{   
    public bool IsCooldown {get; set;}
    public float Cooldown {get; protected set;} //устанавливаем значение в классах-наследниках
    public bool EarlyShotDone {get; protected set;}
    public enum Type 
    {
        bow,
        sword,
        magic
    }
    public Type WeaponType {get; set;}
    protected float energyConsumption;

    public abstract void Hit(); //реализуем в каждом классе-наследнике
    public abstract WeaponSO GetWeaponSO();
    public virtual void Aim(){}  //реализуем не в каждом классе-наследнике
    
    public virtual IEnumerator WeaponHitManagement(WeaponSO weaponSO)
    {
        yield return new WaitForSeconds(weaponSO.hitCooldown);
        IsCooldown = !IsCooldown;
    }

    public float GetEnergyConsumption()
    {
        return energyConsumption;
    }


}
