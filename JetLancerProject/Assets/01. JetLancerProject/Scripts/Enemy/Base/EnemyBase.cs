using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Events;

public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    // Enemy Ÿ��
    protected enum TYPE
    {
        NONE = -1, BASIC, GOLD, HOMING, BIGBULLET
    }   
    protected TYPE Type = TYPE.NONE;

    protected UnityAction moveFunc = default;
    protected UnityAction fireFunc = default;

    // {Enemy ����
    protected int hp = default;
    protected int damage = default;
    protected int score = default;    
    
    protected float speed = default;
    protected float maxSpeed = default;

    // Test
    float bulletTimer = 0f;

    protected float fireTime = 5f;
    // Enemy ����}

    // {Enemy ���ݽ� Ž�� ���� �� ����
    protected float detectAngle = 35f;
    protected float detectRadius = 10f;
    protected float distToTarget = default;
    protected Vector2 dirToTarget = default;
    protected Vector2 dirToShoot = default;
    protected Transform targetPos = default;
    protected float targetAngle = default; 
    // Enemy ���ݽ� Ž�� ���� �� ����}

    protected Rigidbody2D rigid = default;

    // Target ����
    protected GameObject target = default;

    protected abstract void Init();
    protected abstract void SetTarget();
    protected abstract void CheckTarget();

    // �������� ���ÿ����� ¥�ֽ� Move Delegate Ȱ�� 
    protected virtual void Move()
    {
        if (moveFunc == default)
        {
            this.moveFunc = () => DefaultMove();
        }

        // ���� ���� ���� ������ �����ӵ�
        this.moveFunc.Invoke();
        // ���� ���� ���� ������ �����ӵ�

    }       // Move()

    private void DefaultMove()
    {

        if (rigid.velocity.magnitude > maxSpeed)
        {
            rigid.velocity = rigid.velocity.normalized * maxSpeed;
        }       // if : �ӵ��� �ִ� �ӵ��� �Ѿ�� �ִ�ӵ��� ����

        // �ӽ� ������ ���� �Ÿ��� 5f 
        if (distToTarget > 4f)
        {
            float rotateAmount = Vector3.Cross(dirToTarget, transform.right).z;            
            rigid.angularVelocity = -rotateAmount * 100;
            rigid.velocity = transform.right * speed;
        }       // if : ���� �Ÿ����� �÷��̾ ���� �̵�
        else
        {
            // TODO : źȯ �߻� Ȥ�� ȸ�Ǳ⵿ �����ϱ�
            StartCoroutine(EvadeMotion());
        }       // else : ���� �Ÿ� ���� ���� �� �� �ൿ   
    }       // DefaultMove()

    IEnumerator EvadeMotion()
    {
        float evadeTime = 3f;
        float time = Time.time;
        float rotateAmount = Vector3.Cross(dirToTarget, transform.right).z;
        while (evadeTime < time)
        {
            time = 0f;
            float randomRot = Random.Range(50f, 250f);
            rigid.angularVelocity = rotateAmount * randomRot;
            rigid.velocity = transform.right * speed;
            yield return null;  
        }
    }       // EvadeMotion()

    // ���� �貸�� § Fire Delegate Ȱ��
    protected virtual void Fire()
    {
        if (fireFunc == default)
        {
            
            // 3 way fire ������ �� 
            // fireFunc�� DefaultFire�� 3�� += �� �߰��ϵ�, 
            // ������ �ΰ��� Fire�� �⺻ dir ���⿡�� angle ���� + ��Ÿ, -��Ÿ ��ŭ ȸ�� ������ ��������
            // �߻� ��Ű�� �ȴ�.
            this.fireFunc = () => DefaultFire(0f);
        }

        if (distToTarget < detectRadius)
        {
            // Ž���� �Ǵ� ���ȸ� �߻� ��Ÿ���� ������ if�� �ȿ� �־��            
            bulletTimer += Time.deltaTime;
            // ��Ÿ�� üũ�� Debug

            // { Ÿ�ٰ� ���� �չ����� �����ؼ� ���� ����
            float dot = Vector2.Dot(dirToTarget, transform.right);
            float theta = Mathf.Acos(dot);
            float degree = theta * Mathf.Rad2Deg;
            //  Ÿ�ٰ� ���� �չ����� �����ؼ� ���� ����}

            if (degree <= (detectAngle / 2f) && bulletTimer > fireTime)
            {
                this.fireFunc.Invoke();

            }       // if: Ž���� �ȿ� �÷��̾ �ְ�, �߻� ��Ÿ���� �Ǹ� �Ѿ��� �߻�
            else { /* Do nothing */ }

        }       // if : ���� �����ȿ� ������ źȯ �߻�
        else { /* Do noting */ }
    }       // Fire()

    protected void DefaultFire(float angle)
    {

        // TODO : źȯ �߻� 
        // ���� ���ҽ� �Ŵ����� ������Ʈ Ǯ�� �߰��ϸ� ���� ����
        bulletTimer = 0f;
        GameObject bulletObj = GameManager.Instance.poolManager.
            SpawnFromPool(RDefine.ENEMY_BULLET, this.transform.position, Quaternion.identity);

        Quaternion quaternion = Quaternion.AngleAxis(angle, this.transform.forward);
        Vector2 dirBullet = quaternion * this.transform.right;

        dirBullet.Normalize();
        bulletObj.transform.right = dirBullet;

        bulletObj.GetComponent<Rigidbody2D>().
            AddForce(300f * rigid.velocity.magnitude * dirBullet * Time.deltaTime, ForceMode2D.Impulse);

    }       // DefaultFire()

    // źȯ�� Layer�� �˻��ؼ� �ش� �Լ��� ȣ���� ��
    public void OnDamage(int damage)
    {
        if (hp > damage)
        {
            hp -= damage;
            Debug.LogFormat("{0}", hp);
        }       // if : damage���� Ŭ���� ����
        else
        {
            // TODO : ü���� 0�� �Ǹ� �ı��ǵ��� �� (�׽�Ʈ �Ϸ�)
            // ���� ������Ʈ Ǯ�� �߰��Ǹ� ���� ����
            Destroy(this.gameObject);
            Die();
        }       // else : damage���� ���� �� Die() �Լ� ȣ��
    }
    protected abstract void Die();
    
   





}
