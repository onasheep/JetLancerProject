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
        base.Init();

        Type = TYPE.BASIC;
        hp = 5;
        damage = 1;
        speed = 5f;
        maxSpeed = 10f;
      
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
        //this.fireFunc = () =>
        //{
        //    // TODO : Ư�� ���� ����� �����Ѵٸ�, ���⿡�ٰ� �߰�
        //};

        this.fireFunc = default;

        base.Fire();
    }

    protected override void SetTarget()
    {
        if (target.IsValid() == false)
        {
            // TODO : Player ��ũ��Ʈ�� ã�ƿ����� �����ϰ� �Ǹ� �ٸ� �÷��̾��� ��ũ��Ʈ�� ã�ƿ��� �ɰ�
             target = FindObjectOfType<PlayerController>().gameObject;
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
        GameManager.Instance.poolManager.SpawnFromPool(RDefine.ENEMY_EXPLOSION, this.transform.position, Quaternion.identity);
        // TODO : �ı� ��Ÿ ���� �߰� 
    }



}
