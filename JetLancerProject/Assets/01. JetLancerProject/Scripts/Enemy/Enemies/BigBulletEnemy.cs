using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BigBulletEnemy : EnemyBase, IDamageable
{
    // 임시 프리팹용 탄환 
    // 추후 리소스 매니저로 관리 할 것
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

    // TODO : 추후 폴리싱 기간에 가능하다면 CSV 파일로 정보를 읽어와서 넣어주기
    // 해당 초기화 값은 임시값 
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
        // 여기서 타입별로 moveFunc을 다르게 준다.
        this.moveFunc = () =>
        {
            // TODO : 공용 움직임 구현 한다면, 상위 기체에 추가적인 움직임 추가 
        };

        // {위 TODO 부분은 null도 아니고 default도 아니기 때문에 default 초기화 해줌
        this.moveFunc = default;
        // }위 TODO 부분은 null도 아니고 default도 아니기 때문에 default 초기화 해줌

        base.Move();
    }       // Move()

    protected override void FireBullet()
    {
        //// 공격 범위 체크용 Debug Line
        //Debug.DrawLine(this.transform.position, this.transform.position + new Vector3(6f, 1f, 0));
        //Debug.DrawLine(this.transform.position, this.transform.position + new Vector3(6f, -1f, 0));
        //

        if(distToTarget < detectRadius)
        {
            // 탐지가 되는 동안만 발사 쿨타임이 돌도록 if문 안에 넣어둠
            bulletTimer += Time.deltaTime;


            Debug.LogFormat("bulletTimer : {0}", bulletTimer);
            // { 타겟과 적의 앞방향을 내적해서 각을 구함
            float dot = Vector2.Dot(dirToTarget, transform.right);
            float theta = Mathf.Acos(dot);
            float degree = theta * Mathf.Rad2Deg;
            //  타겟과 적의 앞방향을 내적해서 각을 구함}

            if (degree <= detectAngle / 2f && bulletTimer > fireTime)
            {
                // TODO : 탄환 발사 
                // 추후 리소스 매니저와 오브젝트 풀을 추가하면 수정 예정
                Debug.Log("탄환 발사!");
                bulletTimer = 0f;
                Debug.LogFormat("fireTime after shot: {0}", fireTime);
                GameObject bulletObj = Instantiate(bullet, this.transform.position, Quaternion.identity);
                bulletObj.transform.right = dirToTarget;
                bulletObj.GetComponent<Rigidbody2D>().AddForce(10f * rigid.velocity.magnitude * Time.deltaTime * dirToTarget, ForceMode2D.Impulse);
            }       // if: 탐지각 안에 플레이어가 있고, 발사 쿨타임이 되면 총알을 발사하고 Timer를 0으로 초기화
            else { /* Do nothing */ }

        }       // if : 감지 범위안에 들어오면 탄환 발사
        else { /* Do noting */ }
        

    }

    protected override void SetTarget()
    {
        if (target.IsValid() == false)
        {
            target = FindObjectOfType<playerController>().gameObject;
            targetPos = target.transform;
        }       // if : 타겟이 null 이거나 default인 경우 타겟을 가져옴
        else
        {
            Debug.LogWarning("Target is already exist.");
        }       // else : 타겟이 있으면 로그 출력
    }       // SetTarget()

    // 플레이어 포지션, 플레이어와의 거리, 방향벡터, 각들을 계산
    protected override void CheckTarget()
    {
        // { 타겟, 거리, 방향 정보값, 발사에 필요한 정보가 담겨 있으므로, 위치가 변경 될 수 있음
        Transform targetPos = target.transform;
        distToTarget = (targetPos.position - this.transform.position).magnitude;
        dirToTarget = (targetPos.position - this.transform.position).normalized;
        targetAngle = Mathf.Atan(dirToTarget.y / dirToTarget.x) * Mathf.Rad2Deg;
        //  타겟, 거리, 방향 정보값, 발사에 필요한 정보가 담겨 있으므로, 위치가 변경 될 수 있음 }
    }       // CheckTarget()


    protected override void Die()
    {
        // TODO : 파괴 기타 사항 추가 
    }

    public void OnDamage(int damage)
    {
        if(hp > 0)
        {
            base.hp -= damage;
        }       // if : 0보다 클때만 동작
        else
        {
            Die();
        }       // else : 0보다 작으면 Die() 함수 호출
    }

}
