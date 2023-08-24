using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ThreeBulletEnemy : EnemyBase, IDamageable
{
   

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        base.SetTarget();
    }

    private void Update()
    {
        base.CheckTarget();
        Move();
        Fire();
    }

    // TODO : ���� ������ �Ⱓ�� �����ϴٸ� CSV ���Ϸ� ������ �о�ͼ� �־��ֱ�
    // �ش� �ʱ�ȭ ���� �ӽð� 
    protected override void Init()
    {
        base.Init();

        Type = TYPE.BIGBULLET;
        hp = 4;
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
        this.fireFunc = default;

        this.fireFunc += () => DefaultFire(0.0f);
        this.fireFunc += () => DefaultFire(30.0f);
        this.fireFunc += () => DefaultFire(-30.0f);

        base.Fire();
        
    }

    protected override void Die()
    {
        GameManager.Instance.poolManager.SpawnFromPool(RDefine.ENEMY_EXPLOSION, this.transform.position, Quaternion.identity);
        // TODO : �ı� ��Ÿ ���� �߰� 
    }




}
