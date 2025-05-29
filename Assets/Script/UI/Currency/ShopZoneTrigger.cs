using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShopZoneTrigger : MonoBehaviour
{
    public GameObject shopPanel;     // UI Panel ของร้านค้า

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ตรวจว่าเป็นผู้เล่น
        {
            if (shopPanel != null)
            {
                shopPanel.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (shopPanel != null)
            {
                shopPanel.SetActive(false);
            }
        }
    }
}
