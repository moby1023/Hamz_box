using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAiTrack : MonoBehaviour
{
    public GameObject player; // ตั้งค่า Player ที่เราจะให้ Ai มันเดินตาม
    public float speed; // ความเร็ว Ai เดิน
    public float health; // เลือด
    public Animator _animation; // อนิเมชั่น Ai

    [SerializeField] private Transform rayPoint;  // สร้าง RayPoint ใน Ai เพื่อบอกจุดว่าจะให้ RayCast เกิดตรงไหน
    [SerializeField] private float rayDistance; // ปรับระยะ Raycast
    [SerializeField] private LayerMask wallLayer; // ปรับว่า RayCast จะสามารถตรวจจับ layer ไหน อันนี้ตรวจจับ wall

    public float seeDistance; // ระยะที่ Ai จะเห็น
    private float distance;
    private Vector2 velocity;


    void Update()
    {
        velocity = Vector2.zero;

        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = (player.transform.position - transform.position).normalized; //ตรงนี้จะทำให้ Ai ค้นหาตำแหน่ง Player แล้วเดินตรงไปหา

        if (distance < seeDistance)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, transform.right, rayDistance, wallLayer); // สร้าง RayCast มาตรวจจับ layerMask ที่เราตั้งไว้

            if (hitInfo.collider != null) // ถ้าเกิน RayCast มันเจออะไรบางอย่าง มันจะหาทิศทางใหม่เพื่อเดินหลบ
            {
                direction = new Vector2(-direction.y, direction.x);
            }

            velocity = direction * speed;
            transform.position += (Vector3)velocity * Time.deltaTime;

            Debug.DrawRay(rayPoint.position, transform.right * rayDistance, Color.red); // สร้างเส้น RayCast เฉยๆไม่มีไร
        }

        // ตรวจสอบว่า AI เคลื่อนที่อยู่ เพื่อใช้กับ Animation
        if (velocity.magnitude > 0)
        {
            _animation.SetBool("ชื่อ Bool ที่ตั้งไว้ใน Animation", true);
        }
        else
        {
            _animation.SetBool("ชื่อ Bool ที่ตั้งไว้ใน Animation", false);
        }

        // หันไปทางซ้ายหรือขวาตามทิศทางการเคลื่อนที่ Ai เดินไปขวา จะหันหน้าขวา ไปซ้ายหันซ้ายไรงี้
        if (velocity.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (velocity.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void TakeDamage(float damage) // ใช้ร่วมกับสคริปที่ทำดาเมจใส่ เดี๋ยวอธิบาย
    {
        health -= damage;

        if (health <= 0) // เลือดหมด = ตายห่า
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
