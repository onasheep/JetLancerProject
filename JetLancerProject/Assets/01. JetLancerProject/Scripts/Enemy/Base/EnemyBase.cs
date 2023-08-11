using System.Collections;
using System.Collections.Generic;
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

    // {Enemy ����
    protected int hp = default;
    protected int damage = default;
    protected int score = default;    
    
    protected float speed = default;
    protected float maxSpeed = default;

    protected float bulletTimer = 0f;
    protected float fireTime = 2f;
    // Enemy ����}

    // {Enemy ���ݽ� Ž�� ���� �� ����
    protected float detectAngle = 45f;
    protected float detectRadius = 6f;
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

    // �������� ���ÿ����� ¥�ֽŰ�
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

    protected abstract void FireBullet();

    protected abstract void Die();

    private void DefaultMove()
    {
        // LEGACY :
        // ��� �����̴��� �׽�Ʈ
        //// { Ÿ��, �Ÿ�, ���� ������, �߻翡 �ʿ��� ������ ��� �����Ƿ�, ��ġ�� ���� �� �� ����
        //Transform targetPos = target.transform;
        //dist = (targetPos.position - this.transform.position).magnitude;
        //dir = (targetPos.position - this.transform.position).normalized;
        //float angle = Mathf.Atan(dir.y / dir.x) * Mathf.Rad2Deg;
        ////  Ÿ��, �Ÿ�, ���� ������, �߻翡 �ʿ��� ������ ��� �����Ƿ�, ��ġ�� ���� �� �� ���� }

        if (rigid.velocity.magnitude > maxSpeed)
        {
            rigid.velocity = rigid.velocity.normalized * maxSpeed;
        }       // if : �ӵ��� �ִ� �ӵ��� �Ѿ�� �ִ�ӵ��� ����

        // �ӽ� ������ ���� �Ÿ��� 5f 
        if (distToTarget > 5f)
        {
            rigid.AddForce(speed * Time.deltaTime * dirToTarget, ForceMode2D.Impulse);
            if (this.transform.position.x > 0 || targetAngle > 180f)
            {
                targetAngle -= 180f;
            }
            rigid.rotation = Mathf.Lerp(this.transform.rotation.z, targetAngle, 0.5f * Time.time);
        }       // if : ���� �Ÿ����� �÷��̾ ���� �̵�
        else
        {
            // TODO : źȯ �߻� Ȥ�� ȸ�Ǳ⵿ �����ϱ�

        }       // else : ���� �Ÿ� ���� ���� �� �� �ൿ   
    }       // DefaultMove()

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

}
