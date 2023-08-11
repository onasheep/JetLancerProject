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
    protected UnityAction fireFunc = default;

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

    // ���� �貸�� § Fire Delegate Ȱ��
    protected virtual void Fire(GameObject bulletPrefab_)
    {
        if(fireFunc == default)
        {
            // 3 way fire ������ �� 
            // fireFunc�� DefaultFire�� 3�� += �� �߰��ϵ�, 
            // ������ �ΰ��� Fire�� �⺻ dir ���⿡�� angle ���� + ��Ÿ, -��Ÿ ��ŭ ȸ�� ������ ��������
            // �߻� ��Ű�� �ȴ�.
            this.fireFunc = () => DefaultFire(bulletPrefab_);
        }

        // ���� ���� ���� ������ �����ӵ�
        this.fireFunc.Invoke();
        // ���� ���� ���� ������ �����ӵ�

    }       // Fire()

    private  void DefaultFire(GameObject bulletPrefab_)
    {
        if (distToTarget < detectRadius)
        {
            // Ž���� �Ǵ� ���ȸ� �߻� ��Ÿ���� ������ if�� �ȿ� �־��
            bulletTimer += Time.deltaTime;
            // ��Ÿ�� üũ�� Debug
            //Debug.LogFormat("bulletTimer : {0}", bulletTimer);

            // { Ÿ�ٰ� ���� �չ����� �����ؼ� ���� ����
            float dot = Vector2.Dot(dirToTarget, transform.right);
            float theta = Mathf.Acos(dot);
            float degree = theta * Mathf.Rad2Deg;
            //  Ÿ�ٰ� ���� �չ����� �����ؼ� ���� ����}

            if (degree <= detectAngle / 2f && bulletTimer > fireTime)
            {
                // TODO : źȯ �߻� 
                // ���� ���ҽ� �Ŵ����� ������Ʈ Ǯ�� �߰��ϸ� ���� ����
                bulletTimer = 0f;
                Debug.LogFormat("fireTime after shot: {0}", fireTime);
                GameObject bulletObj = Instantiate(bulletPrefab_, this.transform.position, Quaternion.identity);
                bulletObj.transform.right = dirToTarget;
                bulletObj.GetComponent<Rigidbody2D>().AddForce(10f * rigid.velocity.magnitude * Time.deltaTime * dirToTarget, ForceMode2D.Impulse);
            }       // if: Ž���� �ȿ� �÷��̾ �ְ�, �߻� ��Ÿ���� �Ǹ� �Ѿ��� �߻��ϰ� Timer�� 0���� �ʱ�ȭ
            else { /* Do nothing */ }

        }       // if : ���� �����ȿ� ������ źȯ �߻�
        else { /* Do noting */ }
    }       // DefaultFire()

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
