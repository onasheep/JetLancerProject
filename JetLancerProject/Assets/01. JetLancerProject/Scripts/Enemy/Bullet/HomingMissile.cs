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

    private void Awake()
    {
        target = FindObjectOfType<PlayerController>().transform;
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

        // �ִ� �ӵ� ���� + �ӵ��� �ð������� ���� ������
        speed += Time.deltaTime;
        if(speed > 9f)
        {
            speed = 9f;
        }

        // angularvelocity�� ����Ͽ� ���󰡴� ����, ���ϴ� ��ŭ ȸ���ϸ� �������� 
        // velocity�� �ǵ帰�ٴ� ������ ����.. 
        // �� �ȵǸ� �� �κ����� �������� , �ѹ� �߰������� �˾ƺ� �ʿ䰡 �ִ�.
        float rotateAmount = Vector3.Cross(dir, transform.right).z;
        rigid.angularVelocity = -rotateAmount * 200f;
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


            // TODO : �÷��̾ ȸ�� ���� �� , �����ð� �� �ڵ����� ������ ����ź
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
        // Interface ����
       
        this.gameObject.SetActive(false);
    }       // Deactive()

}
