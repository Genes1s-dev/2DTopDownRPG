using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IDamageable
{
    private int maxHealth = 3;
    private int currentHealth;
    [SerializeField] private Sprite[] damagedSprites;
    private int index = 0;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private new Rigidbody2D rigidbody;
    [SerializeField] private GameObject fire;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void DeathSequence()
    {
        Destroy(this.gameObject);
        //Sequence
        //ItemDrops
    }

    public void RestoreHealth(int damage)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth++;
        }
    }

    public void TakeDamage(int damage, WeaponSO.Element element)
    {
        switch (element)
        {
            case WeaponSO.Element.physic:
                ReceivePhysicalDanmage();
                break;
            case WeaponSO.Element.fire:
                ReceiveFireDamage();
                break;
            default:
                break;
        }
    }

    private void ReceivePhysicalDanmage()
    {
        currentHealth--;

        if (currentHealth <= 0)
        {
            DeathSequence();
        } else {
            index++;
            spriteRenderer.sprite = damagedSprites[index];
        } 
    }

    private void ReceiveFireDamage()
    {
        currentHealth = 0;

        fire.SetActive(true);
        animator.SetTrigger("BurnIt");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rigidbody.velocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<Rigidbody2D>().velocity == Vector2.zero)
            {
                rigidbody.velocity = Vector2.zero;
            }
        }
    }
 
}
