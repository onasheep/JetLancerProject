using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Events;

public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    // Enemy 타입
    protected enum TYPE
    {
        NONE = -1, BASIC, GOLD, HOMING, BIGBULLET
    }   
    protected TYPE Type = TYPE.NONE;

    protected UnityAction moveFunc = default;
    protected UnityAction fireFunc = default;

    // {Enemy 정보
    protected int hp = default;
    protected int damage = default;
    protected int score = default;    
    
    protected float speed = default;
    protected float maxSpeed = default;

    // Test
    float bulletTimer = 0f;

    protected float fireTime = 5f;
    // Enemy 정보}

    // {Enemy 공격시 탐지 범위 및 각도
    protected float detectAngle = 35f;
    protected float detectRadius = 10f;
    protected float distToTarget = default;
    protected Vector2 dirToTarget = default;
    protected Vector2 dirToShoot = default;
    protected Transform targetPos = default;
    protected float targetAngle = default; 
    // Enemy 공격시 탐지 범위 및 각도}

    protected Rigidbody2D rigid = default;

    // Target 정보
    protected GameObject target = default;

    protected abstract void Init();
    protected abstract void SetTarget();
    protected abstract void CheckTarget();

    // 교수님이 예시용으로 짜주신 Move Delegate 활용 
    protected virtual void Move()
    {
        if (moveFunc == default)
        {
            this.moveFunc = () => DefaultMove();
        }

        // 여기 뭔가 재사용 가능한 움직임들
        this.moveFunc.Invoke();
        // 여기 뭔가 재사용 가능한 움직임들

    }       // Move()

    private void DefaultMove()
    {

        if (rigid.velocity.magnitude > maxSpeed)
        {
            rigid.velocity = rigid.velocity.normalized * maxSpeed;
        }       // if : 속도가 최대 속도를 넘어가면 최대속도로 고정

        // 임시 움직임 제한 거리값 5f 
        if (distToTarget > 4f)
        {
            float rotateAmount = Vector3.Cross(dirToTarget, transform.right).z;            
            rigid.angularVelocity = -rotateAmount * 100;
            rigid.velocity = transform.right * speed;
        }       // if : 일정 거리까지 플레이어를 향해 이동
        else
        {
            // TODO : 탄환 발사 혹은 회피기동 구현하기
            StartCoroutine(EvadeMotion());
        }       // else : 일정 거리 내에 들어갓을 때 할 행동   
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

    // 위에 배껴서 짠 Fire Delegate 활용
    protected virtual void Fire()
    {
        if (fireFunc == default)
        {
            
            // 3 way fire 구현할 떄 
            // fireFunc에 DefaultFire를 3개 += 로 추가하되, 
            // 나머지 두가지 Fire는 기본 dir 방향에서 angle 값을 + 세타, -세타 만큼 회전 시켜준 방향으로
            // 발사 시키면 된다.
            this.fireFunc = () => DefaultFire(0f);
        }

        if (distToTarget < detectRadius)
        {
            // 탐지가 되는 동안만 발사 쿨타임이 돌도록 if문 안에 넣어둠            
            bulletTimer += Time.deltaTime;
            // 쿨타임 체크용 Debug

            // { 타겟과 적의 앞방향을 내적해서 각을 구함
            float dot = Vector2.Dot(dirToTarget, transform.right);
            float theta = Mathf.Acos(dot);
            float degree = theta * Mathf.Rad2Deg;
            //  타겟과 적의 앞방향을 내적해서 각을 구함}

            if (degree <= (detectAngle / 2f) && bulletTimer > fireTime)
            {
                this.fireFunc.Invoke();

            }       // if: 탐지각 안에 플레이어가 있고, 발사 쿨타임이 되면 총알을 발사
            else { /* Do nothing */ }

        }       // if : 감지 범위안에 들어오면 탄환 발사
        else { /* Do noting */ }
    }       // Fire()

    protected void DefaultFire(float angle)
    {

        // TODO : 탄환 발사 
        // 추후 리소스 매니저와 오브젝트 풀을 추가하면 수정 예정
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

    // 탄환이 Layer를 검사해서 해당 함수를 호출할 것
    public void OnDamage(int damage)
    {
        if (hp > damage)
        {
            hp -= damage;
            Debug.LogFormat("{0}", hp);
        }       // if : damage보다 클때만 동작
        else
        {
            // TODO : 체력이 0이 되면 파괴되도록 함 (테스트 완료)
            // 추후 오브젝트 풀이 추가되면 수정 예정
            Destroy(this.gameObject);
            Die();
        }       // else : damage보다 작을 때 Die() 함수 호출
    }
    protected abstract void Die();
    
   





}
