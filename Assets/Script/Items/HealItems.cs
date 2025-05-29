using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItems : MonoBehaviour
{
    [SerializeField] private float healAmount = 20f;
    [SerializeField] private AudioClip healSFX;
    [SerializeField] private GameObject healEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null && player.currentHealth < player.maxHealth) // เช็คว่าเลือดยังไม่เต็ม
            {
                player.Heal(healAmount);

                // เล่นเสียง
                if (healSFX != null)
                {
                    AudioSource.PlayClipAtPoint(healSFX, transform.position);
                }

                // แสดง effect
                if (healEffect != null)
                {
                    Instantiate(healEffect, transform.position, Quaternion.identity);
                }

                Destroy(gameObject);
            }
        }
    }
}
