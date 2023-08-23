using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Events;

public class GoldEnemy : EnemyBase, IDamageable
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

        Type = TYPE.GOLD;
        hp = 1; //5;
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
        this.fireFunc = () =>
        {
            // TODO : Ư�� ���� ����� �����Ѵٸ�, ���⿡�ٰ� �߰�
        };

        this.fireFunc = default;

        base.Fire();

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
        // { Ÿ��, �Ÿ�, ���� ������, �߻翡 �ʿ��� ������ ��� �����Ƿ�, ��ġ�� ���� �� �� ����
        Transform targetPos = target.transform;
        distToTarget = (targetPos.position - this.transform.position).magnitude;
        dirToTarget = (targetPos.position - this.transform.position).normalized;
        targetAngle = Mathf.Atan(dirToTarget.y / dirToTarget.x) * Mathf.Rad2Deg;
        //  Ÿ��, �Ÿ�, ���� ������, �߻翡 �ʿ��� ������ ��� �����Ƿ�, ��ġ�� ���� �� �� ���� }
    }       // CheckTarget()


    protected override void Die()
    {
        SpawnDieBullet();
        GameManager.Instance.poolManager.SpawnFromPool(RDefine.ENEMY_EXPLOSION, this.transform.position, Quaternion.identity);
        // TODO : �ı� ��Ÿ ���� �߰� 
    }

    // ���� �� 8�������� źȯ�� �Ѹ��� �Լ�
    private void SpawnDieBullet()
    {
        // źȯ �Ѱ��� ȸ���� ���� 
        float angle = 45f;

        // źȯ�� 8�� �̹Ƿ� 8�� ���ư��� for�� 
        for(int i = 0; i < 8; i++)
        {
            Quaternion quaternion = Quaternion.AngleAxis(angle * i, this.transform.forward);
            Vector3 dirBullet = quaternion * this.transform.right;
            GameObject bullet = GameManager.Instance.poolManager.
                SpawnFromPool(RDefine.ENEMY_BULLET, this.transform.position, Quaternion.identity);
            bullet.transform.right = dirBullet;
            bullet.GetComponent<Rigidbody2D>().
                AddForce(200f * Time.deltaTime * dirBullet, ForceMode2D.Impulse);
        }   // loop : 8���� źȯ�� ������ ��, ������ �Ѿ� ������ �������ص� ����

    }       // SpawnDieBullet()



}
