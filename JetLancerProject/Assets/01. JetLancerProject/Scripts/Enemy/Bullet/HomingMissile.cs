using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;

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

        // 매직넘버 10 딱 적당한것 같음
        speed = 5f;
        rigid = GetComponent<Rigidbody2D>();
        target = FindObjectOfType<playerController>().transform;
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
        speed *= Time.time;
        if(speed > 10f)
        {
            speed = 10f;
        }

    






        // angularvelocity를 사용하여 따라가는 로직, 원하는 만큼 회전하며 따라가지만 
        // velocity를 건드린다는 문제가 있음.. 
        // 정 안되면 이 부분으로 가겠지만 , 한번 추가적으로 알아볼 필요가 있다.
        float rotateAmount = Vector3.Cross(dir, transform.right).z;
        rigid.angularVelocity = -rotateAmount * 150f;
        rigid.velocity = transform.right * speed;

        // 해당 부분이 동작함 ! 
        
        // 파도 웨이브 
        //rigid.rotation = Mathf.Sin(Time.time) * 0.5f * Mathf.Rad2Deg;

        // 위 부분으로 동작할 경우 필요 없는 부분 
        if (rigid.velocity.magnitude > 10f)
        {
            rigid.velocity = rigid.velocity.normalized * 10f;
        }
    }

    // 코루틴으로 구현하고 싶었지만 
    // 위에서 구현된 방법이 더 쉽고 , 빠름    
    // 다만 물리 이동을 하지 않는다는 단점이 있음
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
