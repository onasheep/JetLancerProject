using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteBullet : MonoBehaviour, IDeactive
{
    // 이름 변경 deleteTimer => existTime
    private float existTime = 2f;
    private Rigidbody2D rigid;
    public float damage = default;

    // Start is called before the first frame update
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        damage = 0;
    }
    private void OnEnable()
    {
        Invoke("Deactive", existTime);
    }
  
    private void OnDisable()
    {
        rigid.velocity = Vector3.zero;
        this.transform.rotation = Quaternion.identity;  
    }

    // Update is called once per frame

    public void Deactive()
    {
        // Interface 내용
        this.gameObject.SetActive(false);
    }       // Deactive()

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // SJ_
        //{ EnemyBase 에 있는 OnDamage() 를 호출함        

        // TODO : 추후 데미지 추가되면 임시변수가 아닌 가져와서 쓸것        
        int damge = 1;

        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")) ||
            collision.gameObject.layer.Equals(LayerMask.NameToLayer("Boss")))
        {
            collision.GetComponent<IDamageable>().OnDamage(damge);
            Deactive();
        }       // if : layer를 통해서 EnemyBase가 가지고 있는 OnDamage()를 호출
        else { /* Do Nothing */ }

        // TEST : 임시로 해당 보스가 가지고 있는 스크립트를 가지고옴
        // TODO : BossBase를 만들어서 IDamageable을 들고 있게 할것 
        //collision.GetComponent<IDamageable>().OnDamage(damge);
              // if : layer를 통해서 Boss가 가지고 있는 OnDamage()를 호출

        // EnemyBase, Boss_Eye 에 있는 OnDamage() 를 호출함 }

        // Destroy() => Deactive 로 변경

    }
}
