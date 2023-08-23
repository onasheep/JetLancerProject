using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Events;

public class GoldEnemy : EnemyBase, IDamageable
{
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
        Fire();
    }

    // TODO : 추후 폴리싱 기간에 가능하다면 CSV 파일로 정보를 읽어와서 넣어주기
    // 해당 초기화 값은 임시값 
    protected override void Init()
    {
        base.Init();

        Type = TYPE.GOLD;
        hp = 1; //5;
        damage = 1;
        speed = 5f;
        maxSpeed = 10f;
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

    protected override void Fire()
    {
        this.fireFunc = () =>
        {
            // TODO : 특정 공격 방식을 구현한다면, 여기에다가 추가
        };

        this.fireFunc = default;

        base.Fire();

    }

    protected override void SetTarget()
    {
        if (target.IsValid() == false)
        {
            target = FindObjectOfType<PlayerController>().gameObject;
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
        SpawnDieBullet();
        GameManager.Instance.poolManager.SpawnFromPool(RDefine.ENEMY_EXPLOSION, this.transform.position, Quaternion.identity);
        // TODO : 파괴 기타 사항 추가 
    }

    // 죽을 떄 8방향으로 탄환을 뿌리는 함수
    private void SpawnDieBullet()
    {
        // 탄환 한개당 회전할 각도 
        float angle = 45f;

        // 탄환이 8개 이므로 8번 돌아가는 for문 
        for(int i = 0; i < 8; i++)
        {
            Quaternion quaternion = Quaternion.AngleAxis(angle * i, this.transform.forward);
            Vector3 dirBullet = quaternion * this.transform.right;
            GameObject bullet = GameManager.Instance.poolManager.
                SpawnFromPool(RDefine.ENEMY_BULLET, this.transform.position, Quaternion.identity);
            bullet.transform.right = dirBullet;
            bullet.GetComponent<Rigidbody2D>().
                AddForce(200f * Time.deltaTime * dirBullet, ForceMode2D.Impulse);
        }   // loop : 8개의 탄환을 가져온 뒤, 각각의 총알 방향을 지정해준뒤 쏴줌

    }       // SpawnDieBullet()



}
