using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class HomingMissile : MonoBehaviour
{
    public Transform target;
    private Rigidbody2D rigid;
    private float speed;
    //private float dist;
    private float chaseTime;
    private float maxChaseTime = 20f;
    private Vector2 dir;
    float angle = default;
    float degree = default;
    // Start is called before the first frame update
    void Start()
    {

        //dist = (target.position - transform.position).magnitude;
        //dir = target.position - transform.position;

        speed = 10f;
        rigid = GetComponent<Rigidbody2D>();
        //rigid.AddForce(5f * Time.deltaTime * dir, ForceMode2D.Impulse);
        //StartCoroutine(ChaseTarget(target));
    }

    // Update is called once per frame
    void Update()
    {
        //dist = (target.position - transform.position).magnitude;
        dir = target.position - transform.position;
        dir.Normalize();


        //rigid.AddTorque( this.transform.position.z * 100f * 30f,ForceMode2D.Impulse);
        //else if(Mathf.Approximately)

        //rigid.AddForce(transform.up * Mathf.Cos(Time.time) * 0.1f, ForceMode2D.Impulse);
        //this.transform.position = new Vector2(0f, Mathf.Sin(Time.time));


        // angularvelocity�� ����Ͽ� ���󰡴� ����, ���ϴ� ��ŭ ȸ���ϸ� �������� 
        // velocity�� �ǵ帰�ٴ� ������ ����.. 
        // �� �ȵǸ� �� �κ����� �������� , �ѹ� �߰������� �˾ƺ� �ʿ䰡 �ִ�.
        float rotateAmount = Vector3.Cross(dir, transform.right).z;
        rigid.angularVelocity = -rotateAmount * 200f;
        rigid.velocity = transform.right * speed;

        // �ش� �κ��� ������ ! 
        
        // �ĵ� ���̺� 
        //rigid.rotation = Mathf.Sin(Time.time) * 0.5f * Mathf.Rad2Deg;

        // �� �κ����� ������ ��� �ʿ� ���� �κ� 
        if (rigid.velocity.magnitude > 10f)
        {
            rigid.velocity = rigid.velocity.normalized * 10f;
        }
    }

    // �ڷ�ƾ���� �����ϰ� �;����� 
    // ������ ������ ����� �� ���� , ����    
    // �ٸ� ���� �̵��� ���� �ʴ´ٴ� ������ ����
    IEnumerator ChaseTarget(Transform target)
    {
        while (chaseTime < maxChaseTime)
        {
            chaseTime += Time.deltaTime;

            //this.transform.right = dir;
            rigid.AddForce(2f * Time.deltaTime * transform.right, ForceMode2D.Impulse);
            //rigid.AddForce(2f * Time.deltaTime * transform.right, ForceMode2D.Impulse);

            //rigid.AddForce(Mathf.Sin(Time.deltaTime * 5f) * 0.5f * transform.up, ForceMode2D.Impulse);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
