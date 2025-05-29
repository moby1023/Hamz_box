using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShopZoneTrigger : MonoBehaviour
{
    public GameObject shopPanel;     // UI Panel �ͧ��ҹ���

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ��Ǩ����繼�����
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
