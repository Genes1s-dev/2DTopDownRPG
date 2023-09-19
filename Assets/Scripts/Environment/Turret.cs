using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private GameObject arrow;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(Shoot), 3f, 3f);
    }

    public void Shoot()
    {
        animator.SetTrigger("Shoot");
        Instantiate(arrow, transform.position, Quaternion.identity);
    }
}
