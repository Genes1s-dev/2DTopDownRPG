using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Bow : ChargableWeapon
{
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject bow;
    [SerializeField] private GameObject arrow;
  
    private Vector3 aimPosition;


    public event EventHandler<OnHitDetectedEventArgs> OnHitDetected;
    public class OnHitDetectedEventArgs : EventArgs
    {
        public int damage;
    }

    private void Awake()
    {
        WeaponType = Type.bow;
        Cooldown = weaponSO.hitCooldown;
    }

    private void Update()
    {
        if (Player.Instance.isAiming && !IsCooldown)
        {
            Aim();
        } 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            OnHitDetected?.Invoke(this, new OnHitDetectedEventArgs {damage = weaponSO.damage});
        }
    }


    public override void Hit()
    {
        Vector2 shootDirection = crosshair.transform.localPosition;
       
        shootDirection.Normalize();
      

        GameObject arrow = Instantiate(weaponSO.prefab.gameObject, transform.position, Quaternion.identity);
        arrow.gameObject.GetComponent<Rigidbody2D>().velocity = shootDirection * weaponSO.projectileFlightSpeed;
        

        arrow.transform.Rotate(0, 0, Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg);

        IsCooldown = true;

        StartCoroutine(WeaponHitManagement(weaponSO));
        

        Destroy(arrow.gameObject, 2f);

        crosshair.gameObject.SetActive(false);
        bow.GetComponent<SpriteRenderer>().enabled = false;
        this.arrow.GetComponent<SpriteRenderer>().enabled = false;
    }

    public override void Aim()
    {
        crosshair.gameObject.SetActive(true);
        bow.GetComponent<SpriteRenderer>().enabled = true;
        this.arrow.GetComponent<SpriteRenderer>().enabled = true;

        Vector3 playerPos = Player.Instance.gameObject.transform.position;
        Vector3 mousePosition = Input.mousePosition;
        aimPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        

        float distance = Vector2.Distance(aimPosition, playerPos);

        if (distance > 0.5f)
        {
            Vector2 direction = aimPosition - playerPos;
            direction.Normalize();
            Vector2 cursorPosition = (Vector2)playerPos + (direction * 0.5f);

            crosshair.transform.position = ClampAiming(Player.Instance.GetLastMoveInput(), cursorPosition);
        }
        else
        { 
            crosshair.transform.position = ClampAiming(Player.Instance.GetLastMoveInput(), aimPosition);
        }
    }

    private Vector2 ClampAiming(Vector2 facingDirection, Vector2 targetPosition)
    {
        if (facingDirection == Vector2.left)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, targetPosition.x, transform.position.x);
        } else 
        if (facingDirection == Vector2.right)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, transform.position.x, targetPosition.x);
        } else 
        if (facingDirection == Vector2.up)
        {
            targetPosition.y = Mathf.Clamp(targetPosition.y, transform.position.y, targetPosition.y);
        } else 
        if (facingDirection == Vector2.down)
        {
            targetPosition.y = Mathf.Clamp(targetPosition.y, targetPosition.y, transform.position.y);
        } else

        //Диагональные вектора делаем нормированными для корректного сравнивания, т.к. их длина не равна 1 (~0.71)
        if (facingDirection == new Vector2(-1f, 1f).normalized)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, targetPosition.x, transform.position.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, transform.position.y, targetPosition.y);
        } else
        if (facingDirection == new Vector2(1f, 1f).normalized)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, transform.position.x, targetPosition.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, transform.position.y, targetPosition.y);
        } else
        if (facingDirection == new Vector2(-1f, -1f).normalized)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, targetPosition.x, transform.position.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, targetPosition.y, transform.position.y);
        } else
        if (facingDirection == new Vector2(1f, -1f).normalized)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, transform.position.x, targetPosition.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, targetPosition.y, transform.position.y);
        } 
        return targetPosition;
    }

    public override WeaponSO GetWeaponSO()
    {
        return weaponSO;
    }
}
