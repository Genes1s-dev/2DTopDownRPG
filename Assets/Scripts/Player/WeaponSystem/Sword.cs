using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : SingleStrikeWeapon
{
    [SerializeField] private WeaponSO weaponSO;
    private bool hitDealt = false;
    private bool attackPerformed = false;
    [SerializeField] private AnimationClip hitAnimation;
 

    private void Awake()
    {
        WeaponType = Type.sword;
        Cooldown = weaponSO.hitCooldown;
    }

    private void Start()
    {

    }

    public override void Hit()
    {
        if (!IsCooldown)
        {
            Player.Instance.GetAnimator().SetTrigger("SwordHit");
            attackPerformed = true;
            IsCooldown = true;
            StartCoroutine(WeaponHitManagement(weaponSO));
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit detected");
        IDamageable hitObject = other.gameObject.GetComponent<IDamageable>();
        if (!hitDealt && attackPerformed)  //раним противника мечом только если это первое попадание по его коллайдеру во время атаки, и только во время анимации удара!!
        {
            hitObject?.TakeDamage(weaponSO.damage, weaponSO.element);
            hitDealt = true;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        IDamageable hitObject = other.gameObject.GetComponent<IDamageable>();
        //если удар был успешен, обнуляем сам факт попадания при выходе коллайдера меча из коллайдера врага
        if (hitObject != null && hitDealt)
        {
            hitDealt = false;
        }
    }

    public override WeaponSO GetWeaponSO()
    {
        return weaponSO;
    }


    public override IEnumerator WeaponHitManagement(WeaponSO weaponSO)
    {
        float clipDuration = hitAnimation.length;
        yield return new WaitForSeconds(clipDuration);
        attackPerformed = false;

        //длительность атаки (и самой анимации) меньше кулдауна, поэтому вычисляем оставшееся время
        yield return new WaitForSeconds(weaponSO.hitCooldown - clipDuration);
        IsCooldown = !IsCooldown;
    }
}
