using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteBullet : MonoBehaviour
{
    private float deleteTimer;
    // Start is called before the first frame update
    void Start()
    {
        deleteTimer = Time.time + 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (deleteTimer < Time.time)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // SJ_
        //{ EnemyBase �� �ִ� OnDamage() �� ȣ����        

        // TODO : ���� ������ �߰��Ǹ� �ӽú����� �ƴ� �����ͼ� ����
        int damge = 1;

        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            collision.GetComponent<IDamageable>().OnDamage(damge);
        }       // if : layer�� ���ؼ� EnemyBase�� ������ �ִ� OnDamage()�� ȣ��
        else if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Boss")))
        {
            // TEST : �ӽ÷� �ش� ������ ������ �ִ� ��ũ��Ʈ�� �������
            // TODO : BossBase�� ���� IDamageable�� ��� �ְ� �Ұ� 
            collision.GetComponent<IDamageable>().OnDamage(damge);
        }       // if : layer�� ���ؼ� Boss�� ������ �ִ� OnDamage()�� ȣ��
        else { /* Do Nothing */ }

        // EnemyBase, Boss_Eye �� �ִ� OnDamage() �� ȣ���� }

        Destroy(gameObject);

    }
}
