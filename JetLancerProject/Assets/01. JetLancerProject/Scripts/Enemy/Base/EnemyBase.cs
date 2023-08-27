using System.Collections;
using System.Collections.Generic;
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
    
    protected SpriteRenderer sprite = default;

    // Test : 데미지 입엇을때 material 변경
    // default 메터리얼과, SDF 메테리얼로 Swap 시도해보기 
    public Material[] materials = default;
    
    protected AudioSource audioSource = default;

    public AudioClip fireClip = default;
    public Transform trailPos = default; 
 
    // Target 정보
    protected GameObject target = default;

    protected virtual void Init()
    {
        rigid = GetComponent<Rigidbody2D>();
        
        audioSource = GetComponent<AudioSource>();

        sprite = GetComponent<SpriteRenderer>();

        trailPos = this.gameObject.FindChildComponent<Transform>("trailPos");

    }
    protected virtual void SetTarget()
    {
        if (target.IsValid() == false)
        {
            target = GFunc.GetRootObj("Player");
            targetPos = target.transform;
        }       // if : 타겟이 null 이거나 default인 경우 타겟을 가져옴
        else
        {
            Debug.LogWarning("Target is already exist.");
        }       // else : 타겟이 있으면 로그 출력
    }       // SetTarget()


    // 플레이어 포지션, 플레이어와의 거리, 방향벡터, 각들을 계산
    protected virtual void CheckTarget()
    {
        if(target.IsValid() == false) {  return; }
     
        // { 타겟, 거리, 방향 정보값, 발사에 필요한 정보가 담겨 있으므로, 위치가 변경 될 수 있음
        Transform targetPos = target.transform;
        distToTarget = (targetPos.position - this.transform.position).magnitude;
        dirToTarget = (targetPos.position - this.transform.position).normalized;
        targetAngle = Mathf.Atan(dirToTarget.y / dirToTarget.x) * Mathf.Rad2Deg;
        //  타겟, 거리, 방향 정보값, 발사에 필요한 정보가 담겨 있으므로, 위치가 변경 될 수 있음 }
    }       // CheckTarget()



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
        float rotateAmount = Vector3.Cross(dirToTarget, transform.right).z;
        if (rigid.velocity.magnitude > maxSpeed)
        {
            rigid.velocity = rigid.velocity.normalized * maxSpeed;
        }       // if : 속도가 최대 속도를 넘어가면 최대속도로 고정

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
        MakeCloud();
    }       // DefaultMove()

    private void MakeCloud()
    {
        GameObject player_trail = GameManager.Instance.poolManager.
            SpawnFromPool(RDefine.PLAYER_TRAIL, trailPos.position, Quaternion.identity);
        // { player_trail scale 변동 
        float randScale = Random.Range(0.1f, 0.4f);
        player_trail.transform.localScale =
            new Vector3(randScale, randScale);
        //  player_trail scale 변동 }
    }


    #region 회피 기동 코루틴
    IEnumerator EvadeMotion(float rotateAmount_)
    {
        float evadeTime = 3f;
        float time = Time.time;
        while (evadeTime < time)
        {
            time = 0f;
            float randomRotSpeed = Random.Range(150f, 200f);
            rigid.angularVelocity = rotateAmount_ * randomRotSpeed;
            rigid.velocity = transform.right * speed;
            yield return null;  
        }
    }       // EvadeMotion()

    // 겹치지 않는 랜덤 움직임 만들기

    IEnumerator EvadeRandom()
    {
        float evadeTime = 1f;
        float randRotAmount = Random.Range(-1, 1);
        float randomRotSpeed = Random.Range(150f, 200f);
        float time = Time.time;
        while (evadeTime > time)
        {
            time = 0f;
            rigid.angularVelocity = randRotAmount * randomRotSpeed;
            rigid.velocity = transform.right * speed;
            yield return null;
        }
    }
    #endregion


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

        bulletTimer = 0f;
        GameObject bulletObj = GameManager.Instance.poolManager.
            SpawnFromPool(RDefine.ENEMY_BULLET, this.transform.position, Quaternion.identity);

        Quaternion quaternion = Quaternion.AngleAxis(angle, this.transform.forward);
        Vector2 dirBullet = quaternion * this.transform.right;

        dirBullet.Normalize();
        bulletObj.transform.right = dirBullet;

        bulletObj.GetComponent<Rigidbody2D>().velocity = dirBullet * 10f;
    }       // DefaultFire()


    // 탄환이 Layer를 검사해서 해당 함수를 호출할 것
    public void OnDamage(int damage)
    {
        if (hp > damage)
        {
            StartCoroutine(SwapSprite());
            hp -= damage;
        }       // if : damage보다 클때만 동작
        else
        {
            Deactive();
            Die();
        }       // else : damage보다 작을 때 Die() 함수 호출
    }
    
    
   IEnumerator SwapSprite()
    {
        sprite.sharedMaterial = materials[1];
        yield return new WaitForSeconds(0.1f);
        sprite.sharedMaterial = materials[0];
    }       // SwapSprite()

    protected abstract void Die();

    public void Deactive()
    {
        // Interface 내용
        GameManager.Instance.AddScore(50);

        this.gameObject.SetActive(false);

    }       // Deactive()



}
