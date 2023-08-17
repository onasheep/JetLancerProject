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

        // �����ѹ� 10 �� �����Ѱ� ����
        rigid = GetComponent<Rigidbody2D>();
        target = FindObjectOfType<PlayerController>().transform;
        rigid.velocity = Vector3.zero;
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
            // TODO : ������Ʈ Ǯ �� ����
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            int damage = 1;
            collision.GetComponent<PlayerController>().OnDamage(damage);
            // �������� �ְ� ���� �ı�
            Destroy(this.gameObject);
        }
    }
    public void Deactive()
    {
        // Interface ����
        this.gameObject.SetActive(false);

    }       // Deactive()

}
