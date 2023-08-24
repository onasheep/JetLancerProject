using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldEnemy : EnemyBase, IDamageable
{
    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        base.SetTarget();
    }

    private void Update()
    {
        base.CheckTarget();
        Move();
        Fire();
    }

    // TODO : 추후 폴리싱 기간에 가능하다면 CSV 파일로 정보를 읽어와서 넣어주기
    // 해당 초기화 값은 임시값 
    protected override void Init()
    {
        base.Init();

        Type = TYPE.GOLD;
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
        this.fireFunc = () =>
        {
            // TODO : 특정 공격 방식을 구현한다면, 여기에다가 추가
        };

        this.fireFunc = default;

        base.Fire();

    }


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
