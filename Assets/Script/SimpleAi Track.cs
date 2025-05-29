using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAiTrack : MonoBehaviour
{
    public GameObject player; // ��駤�� Player �����Ҩ���� Ai �ѹ�Թ���
    public float speed; // �������� Ai �Թ
    public float health; // ���ʹ
    public Animator _animation; // ͹������ Ai

    [SerializeField] private Transform rayPoint;  // ���ҧ RayPoint � Ai ���ͺ͡�ش��Ҩ���� RayCast �Դ�ç�˹
    [SerializeField] private float rayDistance; // ��Ѻ���� Raycast
    [SerializeField] private LayerMask wallLayer; // ��Ѻ��� RayCast ������ö��Ǩ�Ѻ layer �˹ �ѹ����Ǩ�Ѻ wall

    public float seeDistance; // ���з�� Ai �����
    private float distance;
    private Vector2 velocity;


    void Update()
    {
        velocity = Vector2.zero;

        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = (player.transform.position - transform.position).normalized; //�ç���з���� Ai ���ҵ��˹� Player �����Թ�ç���

        if (distance < seeDistance)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, transform.right, rayDistance, wallLayer); // ���ҧ RayCast �ҵ�Ǩ�Ѻ layerMask �����ҵ�����

            if (hitInfo.collider != null) // ����Թ RayCast �ѹ�����úҧ���ҧ �ѹ���ҷ�ȷҧ���������Թ�ź
            {
                direction = new Vector2(-direction.y, direction.x);
            }

            velocity = direction * speed;
            transform.position += (Vector3)velocity * Time.deltaTime;

            Debug.DrawRay(rayPoint.position, transform.right * rayDistance, Color.red); // ���ҧ��� RayCast ����������
        }

        // ��Ǩ�ͺ��� AI ����͹������� ������Ѻ Animation
        if (velocity.magnitude > 0)
        {
            _animation.SetBool("���� Bool ��������� Animation", true);
        }
        else
        {
            _animation.SetBool("���� Bool ��������� Animation", false);
        }

        // �ѹ价ҧ�������͢�ҵ����ȷҧ�������͹��� Ai �Թ仢�� ���ѹ˹�Ң�� 仫����ѹ�����ç��
        if (velocity.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (velocity.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void TakeDamage(float damage) // �������Ѻʤ�Ի���Ӵ������� �����͸Ժ��
    {
        health -= damage;

        if (health <= 0) // ���ʹ��� = ������
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
