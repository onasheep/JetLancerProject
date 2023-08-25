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
    private Animator overlayAnim;
    private Vector2 dir;
    
    private int hp = 2;
    private float speed = 5f;
    private float maxChaseTime = 15f;
    private float rotateSpeed = 200f;

    private void Awake()
    {
        target = GFunc.GetRootObj("Player").GetComponent<PlayerController>().transform;
        overlayAnim = GetComponentInChildren<Animator>();      
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        overlayAnim.SetTrigger("isOverlay");
        Invoke("Deactive", maxChaseTime);   
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
       
        
        if(dir.magnitude < 2)
        {
            rotateSpeed = 300f;
        }

        float rotateAmount = Vector3.Cross(dir, transform.right).z;
        rigid.angularVelocity = -rotateAmount * rotateSpeed;
        rigid.velocity = transform.right * speed;

 
    }

    public void OnDamage(int damage)
    {
        if(hp > damage)
        {
            hp -= damage;
        }
        else
        {
            GameObject missile_explosion = GameManager.Instance.poolManager.SpawnFromPool(RDefine.MISSILE_EXPLOSION, this.transform.position, Quaternion.identity);
            missile_explosion.transform.localScale *= 0.5f;

            Deactive();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {            
            int damage = 1;
            PlayerController player = collision.GetComponent<PlayerController>();
            
            player.OnDamage(damage);


            // TODO : 플레이어가 회피 했을 때 , 일정시간 후 자동으로 터지는 유도탄
            if (player.isDodge == true)
            {
                
            }

            GameObject missile_explosion = GameManager.Instance.poolManager.SpawnFromPool(RDefine.MISSILE_EXPLOSION, this.transform.position, Quaternion.identity);
            missile_explosion.transform.localScale *= 0.5f;
            Deactive();
        }
    }
    public void Deactive()
    {
        // Interface 내용       
        this.gameObject.SetActive(false);
    }       // Deactive()

}
