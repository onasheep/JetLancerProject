using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BasicEnemy : EnemyBase, IDamageable
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

        Type = TYPE.BASIC;
        hp = 5;
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
        //this.fireFunc = () =>
        //{
        //    // TODO : 특정 공격 방식을 구현한다면, 여기에다가 추가
        //};

        this.fireFunc = default;

        base.Fire();
    }

    protected override void SetTarget()
    {
        if (target.IsValid() == false)
        {
            // TODO : Player 스크립트를 찾아오지만 통합하게 되면 다른 플레이어의 스크립트를 찾아오게 될것
             target = FindObjectOfType<PlayerController>().gameObject;
        }       // if : 타겟이 null 이거나 default인 경우 타겟을 가져옴
        else
        {
            Debug.LogWarning("Target is already exist.");
        }       // else : 타겟이 있으면 로그 출력
    }       // SetTarget()

    // 플레이어 포지션, 플레이어와의 거리, 방향벡터, 각들을 계산
    protected override void CheckTarget()
    {
        // 없으면 return
        if (target.IsValid() == false) { return; }

        // { 타겟, 거리, 방향 정보값, 발사에 필요한 정보가 담겨 있으므로, 위치가 변경 될 수 있음
        Transform targetPos = target.transform;
        distToTarget = (targetPos.position - this.transform.position).magnitude;
        dirToTarget = (targetPos.position - this.transform.position).normalized;
        targetAngle = Mathf.Atan(dirToTarget.y / dirToTarget.x) * Mathf.Rad2Deg;
        //  타겟, 거리, 방향 정보값, 발사에 필요한 정보가 담겨 있으므로, 위치가 변경 될 수 있음 }
    }       // CheckTarget()


    protected override void Die()
    {
        GameManager.Instance.poolManager.SpawnFromPool(RDefine.ENEMY_EXPLOSION, this.transform.position, Quaternion.identity);
        // TODO : 파괴 기타 사항 추가 
    }



}
