using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BasicEnemy : EnemyBase, IDamageable
{

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        FindTarget();
    }

    private void Update()
    {
        Move();
        FireBullet();
    }

    protected override void Init()
    {
        Type = TYPE.BASIC;
        base.hp = 20;
        base.damage = 1;
        base.speed = 5f;
        base.maxSpeed = 10f;
        base.rigid = GetComponent<Rigidbody2D>();

    }
    protected override void Move()
    {
        // ���⼭ Ÿ�Ժ��� moveFunc�� �ٸ��� �ش�.
        this.moveFunc = () =>
        {
            // TODO : ���� ������ ���� �Ѵٸ�, ���� ��ü�� �߰����� ������ �߰� 
        };

        // {�� TODO �κ��� null�� �ƴϰ� default�� �ƴϱ� ������ default �ʱ�ȭ ����
        this.moveFunc = default;
        // }�� TODO �κ��� null�� �ƴϰ� default�� �ƴϱ� ������ default �ʱ�ȭ ����

        base.Move();
    }       // Move()

    protected override void FireBullet()
    {
        // ���� ���� üũ�� Debug Line
        Debug.DrawLine(this.transform.position, this.transform.position + new Vector3(6f, 1f, 0));
        Debug.DrawLine(this.transform.position, this.transform.position + new Vector3(6f, -1f, 0));

        

    }

    protected override void FindTarget()
    {
        if (base.target.IsValid() == false)
        {
            base.target = FindObjectOfType<Player>().gameObject;
        }       // if : Ÿ���� null �̰ų� default�� ��� Ÿ���� ������
        else
        {
            Debug.LogWarning("Target is null.");
        }       // else : Ÿ���� ������ ��� ���
    }       // FindTarget()

    protected override void Die()
    {
        // TODO : �ı� ��Ÿ ���� �߰� 
    }

    public void OnDamage(int damage)
    {
        if(hp > 0)
        {
            base.hp -= damage;
        }       // if : 0���� Ŭ���� ����
        else
        {
            Die();
        }       // else : 0���� ������ Die() �Լ� ȣ��
    }

}
