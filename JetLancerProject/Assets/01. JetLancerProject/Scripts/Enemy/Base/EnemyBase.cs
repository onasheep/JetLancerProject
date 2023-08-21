using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Events;


public abstract class EnemyBase : MonoBehaviour, IDeactive
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
    protected Animator anim = default;

    protected AudioSource audioSource = default;

    public AudioClip fireClip = default;

    // Target 정보
    protected GameObject target = default;

    protected virtual void Init()
    {
        rigid = GetComponent<Rigidbody2D>();
        if (anim.IsValid() == true)
        {
            anim = GetComponent<Animator>();

        }
        audioSource = GetComponent<AudioSource>();
    }
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
        float rotateAmount = Vector3.Cross(dirToTarget ,transform.right ).z;

        if (rigid.velocity.magnitude > maxSpeed)
        {
            rigid.velocity = rigid.velocity.normalized * maxSpeed;
        }       // if : 속도가 최대 속도를 넘어가면 최대속도로 고정

        if(rotateAmount == Mathf.Epsilon)
        {            
            rigid.angularVelocity *= - 1f;
        }


        if ((CheckIsOverlap().transform.position - this.transform.position).magnitude < 2f)
        {
            Vector2 dir = (CheckIsOverlap().transform.position - this.transform.position).normalized;
            float rotate = Vector3.Cross(this.transform.right , - dir ).z;
            StartCoroutine(EvadeOverlap(rotate));
        }


        if (distToTarget > 3f)
        {            
            rigid.angularVelocity = -rotateAmount * 150f;
            rigid.velocity = transform.right * speed;
        }       // if : 일정 거리까지 플레이어를 향해 이동
        else
        {
            // 회피기동 구현
            StartCoroutine(EvadeMotion(rotateAmount));
        }       // else : 일정 거리 내에 들어갓을 때 할 행동   
    }       // DefaultMove()

    IEnumerator EvadeMotion(float rotateAmount_)
    {
        float evadeTime = 3f;
        float time = Time.time;
        while (evadeTime < time)
        {
            time = 0f;            
            float randomRot = UnityEngine.Random.Range(200f, 300f);
            rigid.angularVelocity = rotateAmount_ * randomRot;
            rigid.velocity = transform.right * speed;
            yield return null;  
        }
    }       // EvadeMotion()


    private GameObject CheckIsOverlap()
    {
        List<GameObject> overlapCheck = GameManager.Instance.waveManager.enemyList;
        float minDist = float.MaxValue;
        int index = 0;
        for (int i = 0; i < overlapCheck.Count; i++)
        {
            float dist = (this.transform.position - overlapCheck[i].transform.position).magnitude;
            if (dist < minDist && dist != Mathf.Epsilon)
            {
                minDist = dist;
                index = i;
            }
        }
        return overlapCheck[index];




    }

    IEnumerator EvadeOverlap(float rotatetAmount_)
    {
        float evadeTime = 3f;
        float time = Time.time;
        while (evadeTime < time)
        {
            time = 0f;
            rigid.angularVelocity = rotatetAmount_ * 200f;
            rigid.velocity = transform.right * speed;
            yield return null;
        }
    }


    // 위에 배껴서 짠 Fire Delegate 활용
    protected virtual void Fire()
    {
        if (fireFunc == default)
        {
         
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
                audioSource.PlayOneShot(fireClip);
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
            Deactive();
            Die();
        }       // else : damage보다 작을 때 Die() 함수 호출
    }
    protected abstract void Die();

    public void Deactive()
    {
        // Interface 내용
        this.gameObject.SetActive(false);

    }       // Deactive()



}
