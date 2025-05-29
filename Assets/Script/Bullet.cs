using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject impactEffect;
    public float damage = 10f; 

    private void Start()
    {
        Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Wall":
                Impact();
                break;

            case "Enemy":
                Debug.Log("Hit");
                AiChase enemy = collision.GetComponent<AiChase>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
                Destroy(gameObject);
                break;

            case "Object":
                break;
        }
    }

    public void Impact()
    {
        if (impactEffect != null)
        {
            GameObject effect = Instantiate(impactEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
        }
        Destroy(gameObject);
    }
}
