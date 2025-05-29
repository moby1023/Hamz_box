using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiChase : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float health;
    public Animator _animation;
    public GameObject bloodSplatter;

    public System.Action OnDeath;


    [SerializeField] private Transform rayPoint;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float attackDamage = 10f;

    [Header("Item Drop Settings")]
    [SerializeField] private GameObject[] dropItems; // ใส่ Prefab ไอเท็ม
    [SerializeField] private float dropChance = 0.5f;

    private float lastAttackTime;


    [Header("Reward Settings")]
    [SerializeField] private int rewardMoney = 10;


    public float seeDistance;
    private float distance;
    private Vector2 velocity;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // ถ้า player ยังไม่ถูกเซ็ต ให้หาจาก tag "Player"
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindWithTag("Player");
            if (foundPlayer != null)
            {
                player = foundPlayer;
            }
            else
            {
                Debug.LogWarning("AI cannot find player with tag 'Player'.");
            }
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {


        velocity = Vector2.zero;

        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = (player.transform.position - transform.position).normalized; // ทำให้เป็นหน่วยเดียว

        if (distance < seeDistance) // ตรวจสบระยะการมองเห็น
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, transform.right, rayDistance, wallLayer); // ยิง raycast เพื่อตรวจหาสิ่งกีดขวาง

            if (hitInfo.collider != null) // ถ้าเจอสิ่งกีดขวางให้สุ่มทางเดินไปซ้าย / ขวา
            {
                direction = new Vector2(-direction.y, direction.x);
            }

            velocity = direction * speed;
            transform.position += (Vector3)velocity * Time.deltaTime;

            Debug.DrawRay(rayPoint.position, transform.right * rayDistance, Color.red);
        }

        
        if (velocity.magnitude > 0) // เล่น animation เมื่อเดิน
        {
            _animation.SetBool("isWalking", true);
        }
        else
        {
            _animation.SetBool("isWalking", false);
        }

        
        if (velocity.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (velocity.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (distance < attackRange)
        {
            TryAttackPlayer();
        }

    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red ;
            Invoke("ResetColor", 0.1f); 
        }

        if (health <= 0)
        {
            Die();
        }
    }

    void ResetColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }
    }

    private void TryAttackPlayer()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(attackDamage);
                Debug.Log("AI attacked the player!");
            }

            lastAttackTime = Time.time;
        }
    }


    void Die()
    {
        // สร้างเลือด
        if (bloodSplatter != null)
        {
            GameObject effect = Instantiate(bloodSplatter, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
        }

        TryDropItem();

        // ✅ เพิ่มเงินให้ผู้เล่น
        MoneyManager.Instance.AddMoney(rewardMoney);

        OnDeath?.Invoke();
        Destroy(gameObject);
    }


    private void TryDropItem()
    {
        if (dropItems.Length == 0) return;

        if (Random.value <= dropChance) // เช็คโอกาสดรอป
        {
            int index = Random.Range(0, dropItems.Length);
            Instantiate(dropItems[index], transform.position, Quaternion.identity);
        }
    }
}