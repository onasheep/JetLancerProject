using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteBullet : MonoBehaviour, IDeactive
{
    // �̸� ���� deleteTimer => existTime
    private float existTime = 2f;
    private Rigidbody2D rigid;

    // Start is called before the first frame update
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
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
        // Interface ����
        this.gameObject.SetActive(false);
    }       // Deactive()

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // SJ_
        //{ EnemyBase �� �ִ� OnDamage() �� ȣ����        

        // TODO : ���� ������ �߰��Ǹ� �ӽú����� �ƴ� �����ͼ� ����
        // TEST : 1 => 10
        int damge = 10;

        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")) ||
            collision.gameObject.layer.Equals(LayerMask.NameToLayer("Boss")))
        {
            collision.GetComponent<IDamageable>().OnDamage(damge);
            Deactive();
        }       // if : layer�� ���ؼ� EnemyBase�� ������ �ִ� OnDamage()�� ȣ��
        else { /* Do Nothing */ }

        // TEST : �ӽ÷� �ش� ������ ������ �ִ� ��ũ��Ʈ�� �������
        // TODO : BossBase�� ���� IDamageable�� ��� �ְ� �Ұ� 
        //collision.GetComponent<IDamageable>().OnDamage(damge);
              // if : layer�� ���ؼ� Boss�� ������ �ִ� OnDamage()�� ȣ��

        // EnemyBase, Boss_Eye �� �ִ� OnDamage() �� ȣ���� }

        // Destroy() => Deactive �� ����

    }
}
