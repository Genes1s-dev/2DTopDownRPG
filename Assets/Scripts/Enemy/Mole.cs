using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(PatrolPoints))]
public class Mole : MonoBehaviour, IDamageable
{
    private int maxHealth = 100;
    private int currentHealth;
    private PatrolPoints patrolPoints;
    private Animator animator;
    [SerializeField] Image hpFillAmount;
    [SerializeField] private float moveSpeed = 3f;
    private enum MoleState
    {
        idle,
        patroling,
    }

    private MoleState currentState;
    private void Awake()
    {
        patrolPoints = GetComponent<PatrolPoints>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentState = MoleState.patroling;
        currentHealth = maxHealth;
    }
    private void Update()
    {  
        switch(currentState)
        {
            case MoleState.idle:

                animator.SetBool("idle", true);
                break;

            case MoleState.patroling:

                animator.SetBool("idle", false);

                Vector3 direction = patrolPoints.GetTargetPointDirection();
                SetAnimation(direction);
                if (!patrolPoints.HasRichedPoint())
                {
                    this.transform.position += direction * moveSpeed * Time.deltaTime;
                } else 
                {
                    float minValue = 1.0f;
                    float maxValue = 3.5f;
                    float randomTimer = Helper.GenerateRandomFloat(minValue, maxValue);
                    StartCoroutine(IdleTimerOn(randomTimer));
                    patrolPoints.SetNextTargetPoint();
                }
                break;

            default:
                break; 
        } 
    }

    private IEnumerator IdleTimerOn(float randomTimer)
    {
        currentState = MoleState.idle;
        yield return new WaitForSeconds(randomTimer);
        currentState = MoleState.patroling;
    }

    private void SetAnimation(Vector3 direction)
    {
        animator.SetFloat("horizontal", direction.x);
        animator.SetFloat("vertical", direction.y);
    }

    public void TakeDamage(int damage, WeaponSO.Element element)
    {
        this.currentHealth -= damage;
        hpFillAmount.fillAmount = currentHealth / maxHealth;

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
