using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EnemyBase : MonoBehaviour
{
    // Enemy Ÿ��
    protected enum TYPE
    {
        NONE = -1, BASIC, GOLD, HOMING, BIGBULLET
    }   
    protected TYPE Type = TYPE.NONE;
    protected UnityAction moveFunc = default;

    // Enemy ����
    protected int hp = default;
    protected int damage = default;
    protected int score = default;    
    
    protected float speed = default;
    protected float maxSpeed = default;

    // Enemy ���ݽ� Ž�� ���� �� ����
    protected float detectRange = 30f;
    protected float detectRadius = 6f;

    protected Rigidbody2D rigid = default;

    // Target ����
    protected GameObject target = default;

    protected abstract void Init();
    protected abstract void FindTarget();

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
        // { Ÿ��, �Ÿ�, ���� ������, �߻翡 �ʿ��� ������ ��� �����Ƿ�, ��ġ�� ���� �� �� ����
        Transform targetPos = target.transform;
        float dist = (targetPos.position - this.transform.position).magnitude;
        Vector2 dir = (targetPos.position - this.transform.position).normalized;
        float angle = Mathf.Atan(dir.y / dir.x) * Mathf.Rad2Deg;
        //  Ÿ��, �Ÿ�, ���� ������, �߻翡 �ʿ��� ������ ��� �����Ƿ�, ��ġ�� ���� �� �� ���� }

        if (rigid.velocity.magnitude > maxSpeed)
        {
            rigid.velocity = rigid.velocity.normalized * maxSpeed;
        }       // if : �ӵ��� �ִ� �ӵ��� �Ѿ�� �ִ�ӵ��� ����

        // �ӽ� ������ ���� �Ÿ��� 5f 
        if (dist > 5f)
        {
            rigid.AddForce(speed * Time.deltaTime * dir, ForceMode2D.Impulse);
            if (this.transform.position.x > 0 || angle > 180f)
            {
                angle -= 180f;
            }
            rigid.rotation = Mathf.Lerp(this.transform.rotation.z, angle, 0.5f * Time.time);
        }       // if : ���� �Ÿ����� �÷��̾ ���� �̵�
        else
        {
            // TODO : źȯ �߻� Ȥ�� ȸ�Ǳ⵿ �����ϱ�

        }       // else : ���� �Ÿ� ���� ���� �� �� �ൿ   
    }
}
