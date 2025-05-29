using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoItem : MonoBehaviour
{
    [SerializeField] private int ammoAmount = 20;
    [SerializeField] private AudioClip ammoPickupSFX;
    [SerializeField] private GameObject pickupEffect;

    [Header("Allowed Weapon Tags")]
    [SerializeField] private List<string> allowedTags; // ✅ เพิ่ม List สำหรับหลาย Tag

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null && player.weapon != null)
            {
                if (allowedTags.Contains(player.weapon.tag))
                {
                    // เช็คว่ากระสุนยังไม่เต็ม
                    if (player.weapon.CurrentAmmo < player.weapon.MaxAmmo)
                    {
                        player.weapon.RefillAmmo(ammoAmount);

                        if (ammoPickupSFX != null)
                        {
                            AudioSource.PlayClipAtPoint(ammoPickupSFX, transform.position);
                        }

                        if (pickupEffect != null)
                        {
                            Instantiate(pickupEffect, transform.position, Quaternion.identity);
                        }

                        Destroy(gameObject);
                    }
                    else
                    {
                        // ถ้าอยากให้มี debug log
                        Debug.Log("Ammo is full! Cannot pick up.");
                    }
                }
            }
        }
    }

}
