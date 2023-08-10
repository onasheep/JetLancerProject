using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteBullet : MonoBehaviour
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
        //{ PlayerController�� �ִ� OnDamage() �� ȣ����        
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            // TODO : ���� ������ �߰��Ǹ� �ӽú����� �ƴ� �����ͼ� ����
            int damge = 10;
            collision.GetComponent<EnemyBase>().OnDamage(damge);
        }       // if : layer�� ���ؼ� playerController�� �������� �������� ��
        else { /* Do Nothing */ }
        // PlayerController�� �ִ� OnDamage() �� ȣ���� }


        Destroy(gameObject);

    }
}
