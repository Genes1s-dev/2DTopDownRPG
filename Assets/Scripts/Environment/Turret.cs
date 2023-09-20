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
    private Transform currentTarget;
    private int index = 0;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(SetTarget), 3f, 3f);
    }

    public void SetTarget()
    {
        currentTarget = targetSpots[index];
        if (index >= targetSpots.Length - 1)
        {
            index = 0;
        } else {
            index++;
        }

        animator.SetTrigger("Shoot");
        StartCoroutine(nameof(Shoot));

    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(1.5f);

        GameObject arrow = Instantiate(arrowPrefab, arrowStartPosition.position, Quaternion.identity);

        Vector3 shootingDir = currentTarget.position - arrowStartPosition.position;

        arrow.transform.Rotate(0, 0, Mathf.Atan2(shootingDir.y, shootingDir.x) * Mathf.Rad2Deg);
        arrow.GetComponent<Rigidbody2D>().velocity = shootingDir * weaponSO.projectileFlightSpeed;
    }
}
