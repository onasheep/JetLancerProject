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

        Type = TYPE.HOMING;
        hp = 3;
        damage = 1;
        speed = 5f;
        maxSpeed = 10f;

        fireMTime = 0f;
        maxfireTime = 15f;

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
      
        if (distToTarget < 30f)
        {
            fireMTime += Time.deltaTime;

            if (fireMTime > maxfireTime)
            {
                fireMTime = 0f;

                FireMissile();
                audioSource.PlayOneShot(fireClip);
            }
        }
    }

    public void FireMissile()
    {
        GameManager.Instance.poolManager.
            SpawnFromPool(RDefine.ENEMY_ROCKET, this.transform.position, Quaternion.identity);
       
    }


    protected override void Die()
    {
        GameManager.Instance.poolManager.SpawnFromPool(RDefine.ENEMY_EXPLOSION, this.transform.position, Quaternion.identity);
        // TODO : �ı� ��Ÿ ���� �߰� 
    }



}
