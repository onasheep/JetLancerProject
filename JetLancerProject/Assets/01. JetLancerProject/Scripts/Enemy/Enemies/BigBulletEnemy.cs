using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BigBulletEnemy : EnemyBase, IDamageable
{
    // �ӽ� �����տ� źȯ 
    // ���� ���ҽ� �Ŵ����� ���� �� ��
    public GameObject bullet;
    //

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
        FireBullet();
    }

    // TODO : ���� ������ �Ⱓ�� �����ϴٸ� CSV ���Ϸ� ������ �о�ͼ� �־��ֱ�
    // �ش� �ʱ�ȭ ���� �ӽð� 
    protected override void Init()
    {
        Type = TYPE.BIGBULLET;
        hp = 30;
        damage = 1;
        speed = 5f;
        maxSpeed = 10f;
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

    protected override void FireBullet()
    {
        //// ���� ���� üũ�� Debug Line
        //Debug.DrawLine(this.transform.position, this.transform.position + new Vector3(6f, 1f, 0));
        //Debug.DrawLine(this.transform.position, this.transform.position + new Vector3(6f, -1f, 0));
        //

        if(distToTarget < detectRadius)
        {
            // Ž���� �Ǵ� ���ȸ� �߻� ��Ÿ���� ������ if�� �ȿ� �־��
            bulletTimer += Time.deltaTime;


            Debug.LogFormat("bulletTimer : {0}", bulletTimer);
            // { Ÿ�ٰ� ���� �չ����� �����ؼ� ���� ����
            float dot = Vector2.Dot(dirToTarget, transform.right);
            float theta = Mathf.Acos(dot);
            float degree = theta * Mathf.Rad2Deg;
            //  Ÿ�ٰ� ���� �չ����� �����ؼ� ���� ����}

            if (degree <= detectAngle / 2f && bulletTimer > fireTime)
            {
                // TODO : źȯ �߻� 
                // ���� ���ҽ� �Ŵ����� ������Ʈ Ǯ�� �߰��ϸ� ���� ����
                Debug.Log("źȯ �߻�!");
                bulletTimer = 0f;
                Debug.LogFormat("fireTime after shot: {0}", fireTime);
                GameObject bulletObj = Instantiate(bullet, this.transform.position, Quaternion.identity);
                bulletObj.transform.right = dirToTarget;
                bulletObj.GetComponent<Rigidbody2D>().AddForce(10f * rigid.velocity.magnitude * Time.deltaTime * dirToTarget, ForceMode2D.Impulse);
            }       // if: Ž���� �ȿ� �÷��̾ �ְ�, �߻� ��Ÿ���� �Ǹ� �Ѿ��� �߻��ϰ� Timer�� 0���� �ʱ�ȭ
            else { /* Do nothing */ }

        }       // if : ���� �����ȿ� ������ źȯ �߻�
        else { /* Do noting */ }
        

    }

    protected override void SetTarget()
    {
        if (target.IsValid() == false)
        {
            target = FindObjectOfType<playerController>().gameObject;
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
