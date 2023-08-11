using System.Collections;
using System.Collections.Generic;
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

    // {Enemy 정보
    protected int hp = default;
    protected int damage = default;
    protected int score = default;    
    
    protected float speed = default;
    protected float maxSpeed = default;

    protected float bulletTimer = 0f;
    protected float fireTime = 2f;
    // Enemy 정보}

    // {Enemy 공격시 탐지 범위 및 각도
    protected float detectAngle = 45f;
    protected float detectRadius = 6f;
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

    // 교수님이 예시용으로 짜주신거
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

    protected abstract void FireBullet();

    protected abstract void Die();

    private void DefaultMove()
    {
        // LEGACY :
        // 없어도 움직이는지 테스트
        //// { 타겟, 거리, 방향 정보값, 발사에 필요한 정보가 담겨 있으므로, 위치가 변경 될 수 있음
        //Transform targetPos = target.transform;
        //dist = (targetPos.position - this.transform.position).magnitude;
        //dir = (targetPos.position - this.transform.position).normalized;
        //float angle = Mathf.Atan(dir.y / dir.x) * Mathf.Rad2Deg;
        ////  타겟, 거리, 방향 정보값, 발사에 필요한 정보가 담겨 있으므로, 위치가 변경 될 수 있음 }

        if (rigid.velocity.magnitude > maxSpeed)
        {
            rigid.velocity = rigid.velocity.normalized * maxSpeed;
        }       // if : 속도가 최대 속도를 넘어가면 최대속도로 고정

        // 임시 움직임 제한 거리값 5f 
        if (distToTarget > 5f)
        {
            rigid.AddForce(speed * Time.deltaTime * dirToTarget, ForceMode2D.Impulse);
            if (this.transform.position.x > 0 || targetAngle > 180f)
            {
                targetAngle -= 180f;
            }
            rigid.rotation = Mathf.Lerp(this.transform.rotation.z, targetAngle, 0.5f * Time.time);
        }       // if : 일정 거리까지 플레이어를 향해 이동
        else
        {
            // TODO : 탄환 발사 혹은 회피기동 구현하기

        }       // else : 일정 거리 내에 들어갓을 때 할 행동   
    }       // DefaultMove()

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

}
