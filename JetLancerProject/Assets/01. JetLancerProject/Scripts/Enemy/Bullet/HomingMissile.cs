using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;

public class HomingMissile : MonoBehaviour, IDamageable
{
    public Transform target;
    private Rigidbody2D rigid;
    private Vector2 dir;

    private int hp = default;
    private float speed = default;
    private float chaseTime = default;
    private float maxChaseTime = default;

    // Start is called before the first frame update
    void Start()
    {
        //Init();

        speed = 1f;
        // 매직넘버 10 딱 적당한것 같음
        rigid = GetComponent<Rigidbody2D>();
        target = FindObjectOfType<playerController>().transform;
        rigid.velocity = Vector3.zero;
        //rigid.AddForce(5f * Time.deltaTime * dir, ForceMode2D.Impulse);
        //StartCoroutine(ChaseTarget(target));
    }

    // Update is called once per frame
    void Update()
    {
        //dist = (target.position - transform.position).magnitude;
        dir = target.position - transform.position;
        dir.Normalize();

        // 최대 속도 제한 + 속도가 시간지남에 따라 빨라짐
        speed = speed + Time.deltaTime;
        if(speed > 10f)
        {
            speed = 10f;
        }



        // angularvelocity를 사용하여 따라가는 로직, 원하는 만큼 회전하며 따라가지만 
        // velocity를 건드린다는 문제가 있음.. 
        // 정 안되면 이 부분으로 가겠지만 , 한번 추가적으로 알아볼 필요가 있다.
        float rotateAmount = Vector3.Cross(dir, transform.right).z;
        rigid.angularVelocity = -rotateAmount * 200;
        rigid.velocity = transform.right * speed;
        Debug.LogFormat("rigid.velocity : {0}", rigid.velocity);

        // 해당 부분이 동작함 ! 

        // 파도 웨이브 
        //rigid.rotation = Mathf.Sin(Time.time) * 0.5f * Mathf.Rad2Deg;

        // 위 부분으로 동작할 경우 필요 없는 부분 
        if (rigid.velocity.magnitude > 10f)
        {
            rigid.velocity = rigid.velocity.normalized * 10f;
        }
    }

    private void Init()
    {
        hp = 3;
        speed = 1f;
        chaseTime = 0f;
        maxChaseTime = 20f;
    }

    public void OnDamage(int damage)
    {
        if(hp > damage)
        {
            Debug.LogFormat("hp{0}", hp);
            hp -= damage;
        }
        else
        {
            // TODO : 오브젝트 풀 시 수정
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            int damage = 1;
            collision.GetComponent<playerController>().OnDamage(damage);
            // 데미지를 주고 나면 파괴
            Destroy(this.gameObject);
        }
    }
        
   
}
