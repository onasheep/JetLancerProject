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
        FindTarget();
    }

    private void Update()
    {
        Move();
        FireBullet();
    }

    protected override void Init()
    {
        Type = TYPE.BASIC;
        base.hp = 20;
        base.damage = 1;
        base.speed = 5f;
        base.maxSpeed = 10f;
        base.rigid = GetComponent<Rigidbody2D>();

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
        // 공격 범위 체크용 Debug Line
        Debug.DrawLine(this.transform.position, this.transform.position + new Vector3(6f, 1f, 0));
        Debug.DrawLine(this.transform.position, this.transform.position + new Vector3(6f, -1f, 0));

        

    }

    protected override void FindTarget()
    {
        if (base.target.IsValid() == false)
        {
            base.target = FindObjectOfType<Player>().gameObject;
        }       // if : 타겟이 null 이거나 default인 경우 타겟을 가져옴
        else
        {
            Debug.LogWarning("Target is null.");
        }       // else : 타겟이 없으면 경고문 출력
    }       // FindTarget()

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
