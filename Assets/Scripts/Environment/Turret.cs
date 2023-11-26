using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Turret : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform[] targetSpots;
    [SerializeField] private Transform arrowStartPosition;
    [SerializeField] private float reloadTimer = 3f;
    private Transform currentTarget;
    private int index = 0;
    private bool active;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(nameof(SetTarget));
    }

    private IEnumerator SetTarget()
    {
        while(active)
        {
            currentTarget = targetSpots[index];
            if (index >= targetSpots.Length - 1)
            {
                index = 0;
            } else {
                index++;
            }

            animator.SetTrigger("Shoot");
            yield return new WaitForSeconds(reloadTimer);
        }

    }

    public void Shoot()  //вызывается в animation events в анимации выстрела 
    {
        GameObject arrow = Instantiate(arrowPrefab, arrowStartPosition.position, Quaternion.identity);

        Vector3 shootingDir = currentTarget.position - arrowStartPosition.position;

        arrow.transform.Rotate(0, 0, Mathf.Atan2(shootingDir.y, shootingDir.x) * Mathf.Rad2Deg);
        arrow.GetComponent<Rigidbody2D>().velocity = shootingDir * weaponSO.projectileFlightSpeed;
    }

    public void ChangeState()
    {
        active = !active;
    }

    public void ChangeTargets()
    {
        //
    }
}
