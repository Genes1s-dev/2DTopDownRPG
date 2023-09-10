using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneCube : MonoBehaviour
{
    private new Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))  //только игрок может взаимодействовать с каменными кубами!!
        {
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
            rigidbody.velocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            if (collision.gameObject.GetComponent<Rigidbody2D>().velocity == Vector2.zero && !collision.gameObject.isStatic)
            {
                rigidbody.velocity = Vector2.zero;
            }            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rigidbody.bodyType = RigidbodyType2D.Static;
        }
    }
}
