using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class HomingEnemy : EnemyBase, IDamageable
{
  
    private float fireMTime = default;
    private float maxfireTime = default;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        SetTarget();
    }

    private void Update()
    {
        CheckTarget();
        Move();

        Fire();
    }

    // TODO : ���� ������ �Ⱓ�� �����ϴٸ� CSV ���Ϸ� ������ �о�ͼ� �־��ֱ�
    // �ش� �ʱ�ȭ ���� �ӽð� 
    protected override void Init()
    {
        Type = TYPE.HOMING;
        hp = 5;
        damage = 1;
        speed = 5f;
        maxSpeed = 10f;

        fireMTime = 0f;
        maxfireTime = 5f;
        rigid = GetComponent<Rigidbody2D>();

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

    protected override void Fire()
    {
      
        if (distToTarget < 10f)
        {
            fireMTime += Time.deltaTime;

            if (fireMTime > maxfireTime)
            {
                fireMTime = 0f;

                Debug.Log("�̻��Ϲ߻�");
                FireMissile();
            }
        }
    }

    public void FireMissile()
    {
        GameManager.Instance.poolManager.
            SpawnFromPool(RDefine.ENEMY_ROCKET, this.transform.position, Quaternion.identity);

    }


    protected override void SetTarget()
    {
        if (target.IsValid() == false)
        {
            target = FindObjectOfType<PlayerController>().gameObject;
            targetPos = target.transform;
        }       // if : Ÿ���� null �̰ų� default�� ��� Ÿ���� ������
        else
        {
            Debug.LogWarning("Target is already exist.");
        }       // else : Ÿ���� ������ �α� ���
    }       // SetTarget()

    // �÷��̾� ������, �÷��̾���� �Ÿ�, ���⺤��, ������ ���
    protected override void CheckTarget()
    {
        // ������ return
        if (target.IsValid() == false) { return; }

        // { Ÿ��, �Ÿ�, ���� ������, �߻翡 �ʿ��� ������ ��� �����Ƿ�, ��ġ�� ���� �� �� ����
        Transform targetPos = target.transform;
        distToTarget = (targetPos.position - this.transform.position).magnitude;
        dirToTarget = (targetPos.position - this.transform.position).normalized;
        targetAngle = Mathf.Atan(dirToTarget.y / dirToTarget.x) * Mathf.Rad2Deg;
        //  Ÿ��, �Ÿ�, ���� ������, �߻翡 �ʿ��� ������ ��� �����Ƿ�, ��ġ�� ���� �� �� ���� }
    }       // CheckTarget()


    protected override void Die()
    {
        // TODO : �ı� ��Ÿ ���� �߰� 
    }



}
