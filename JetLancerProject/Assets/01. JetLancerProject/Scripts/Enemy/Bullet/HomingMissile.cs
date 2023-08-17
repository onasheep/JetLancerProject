using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;

public class HomingMissile : MonoBehaviour, IDamageable, IDeactive
{
    private Transform target;
    private Rigidbody2D rigid;
    private Vector2 dir;

    private int hp = default;
    private float speed = default;
    private float maxChaseTime = default;



    private void OnEnable()
    {
        Init();
        Invoke("Deactive", maxChaseTime);
    }

    private void Init()
    {
        hp = 3;
        speed = 5f;
        maxChaseTime = 15f;

        // 매직넘버 10 딱 적당한것 같음
        rigid = GetComponent<Rigidbody2D>();
        target = FindObjectOfType<PlayerController>().transform;
        rigid.velocity = Vector3.zero;
    }
   
    // Update is called once per frame
    void Update()
    {
        dir = target.position - transform.position;
        dir.Normalize();

        // 최대 속도 제한 + 속도가 시간지남에 따라 빨라짐
        speed += Time.deltaTime;
        if(speed > 9f)
        {
            speed = 9f;
        }

        // angularvelocity를 사용하여 따라가는 로직, 원하는 만큼 회전하며 따라가지만 
        // velocity를 건드린다는 문제가 있음.. 
        // 정 안되면 이 부분으로 가겠지만 , 한번 추가적으로 알아볼 필요가 있다.
        float rotateAmount = Vector3.Cross(dir, transform.right).z;
        rigid.angularVelocity = -rotateAmount * 150f;
        rigid.velocity = transform.right * speed;

 
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
            collision.GetComponent<PlayerController>().OnDamage(damage);
            // 데미지를 주고 나면 파괴
            Destroy(this.gameObject);
        }
    }
    public void Deactive()
    {
        // Interface 내용
        this.gameObject.SetActive(false);

    }       // Deactive()

}
