using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMove : MonoBehaviour
{
    private Rigidbody2D enemyRigid = default;
    private EnemyShooter shooter = default;

    private Vector2 dir = default;
    private float distance = default;


    // temp speed 
    private float moveSpeed = 2f;
    private float maxSpeed = 5f;
    public bool isRotate = false;

    public enum STATE
    {
        NONE = - 1, IDLE, ATTACK, AROUND, DIE
    }       // AROUND : 타겟 주변을 선회하는 상태

    public STATE enemyState = STATE.NONE;

    private void Awake()
    {

    }
    void Start()
    {
        enemyRigid = GetComponent<Rigidbody2D>();
        shooter = GetComponent<EnemyShooter>();

        enemyState = STATE.IDLE;
    }

    // Update is called once per frame
    void Update()
    {

        //Transform targetPos = enemyAi.target.transform;
        //distance = (targetPos.position - this.transform.position).sqrMagnitude;
        //dir = (targetPos.position - this.transform.position).normalized;
        Vector3 cross = Vector3.Cross(this.transform.right, dir);


        float dot = Vector3.Dot(dir, this.transform.right);
        float angle = Vector3.Angle(dir, this.transform.right);

        Debug.LogFormat("dot : {0}", dot);
        Debug.LogFormat("angle : {0}", angle);
        if (cross.z > 0)
        {
            Debug.Log("Player가 왼쪽");
        }
        else if (cross.z < 0)
        {
            Debug.Log("Player가 오른쪽");
        }

  

        //switch (enemyState)
        //{
        //    case STATE.IDLE:
        //        MoveToTarget(distance, targetPos);
              
        //        break;
        //    case STATE.ATTACK:
        //        if(bulletFireRate < Time.time)
        //        {
        //            bulletFireRate += Time.time;
        //            shooter.Fire();
        //            if(distance > 3f)
        //            {
        //                enemyState = STATE.IDLE;
        //            }
        //        }
        //        break;
        //    case STATE.AROUND:


        //        //StartCoroutine(RotateEnemy(angle));
              
        //        enemyState = STATE.IDLE;
        //        break;

        //    case STATE.DIE:
        //        break;
        //}
    }

    private void FixedUpdate()
    {
        if (enemyRigid.velocity.magnitude > maxSpeed)
        {
            enemyRigid.velocity = enemyRigid.velocity.normalized * maxSpeed;
        }
    }

    // LEGACY : 코루틴 만들었는데, 원하는 방식의 회전이 나오지 않음.. 각도를 이용해서 사용해야 할 것 같음 혹시 모르니 일단 남겨 둘것
    //IEnumerator RotateEnemy(float angle)
    //{
    //    float st = transform.eulerAngles.z;
    //    float er = 360f;

    //    float t = 0f;

    //    while (t < 5f)
    //    {
    //        t += Time.deltaTime;
    //        float speed = t;
    //        float zRotation = Mathf.Lerp(st, er, speed) % 360.0f;
    //        enemyRigid.AddForce(this.transform.right * 1f * Time.deltaTime, ForceMode2D.Impulse);
    //        transform.rotation = Quaternion.AngleAxis(zRotation, Vector3.forward);

    //        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);
            
    //        Debug.Log(zRotation + "// " + transform.eulerAngles.z);

    //        yield return null;

    //    }

    //}

    // 외적을 통해서 플레이어가 적 왼쪽 오른쪽에 있는지를 찾아냄 
    // 추후 폴리싱 기간에 움직임 수정 때 고려할 것!
    void MoveTest()
    {
        ////Transform targetPos = enemyAi.target.transform;
        //distance = (targetPos.position - this.transform.position).sqrMagnitude;
        //dir = (targetPos.position - this.transform.position).normalized;

        Vector3 cross = Vector3.Cross(this.transform.forward, dir);

        if(cross.z < 0 )
        {
            Debug.Log("Player가 왼쪽");
        }
        else if(cross.z > 0 )
        {
            Debug.Log("Player가 오른쪽");
        }

    }


    // 1차 움직임
    void MoveToTarget(float distance, Transform targetPos)
    {
        float angle = Mathf.Atan(dir.y / dir.x) * Mathf.Rad2Deg;

        dir = (targetPos.position - this.transform.position).normalized;

        if (distance > 5f)
        {
            enemyRigid.AddForce(dir * moveSpeed * Time.deltaTime, ForceMode2D.Impulse);
            if (this.transform.position.x > 0)
            {
                angle -= 180f;
            }

            enemyRigid.rotation = Mathf.Lerp(this.transform.rotation.z, angle, 0.5f * Time.time);


            if (angle > 180)
            {
                enemyRigid.rotation = Mathf.Lerp(this.transform.rotation.z, angle - 180f, 0.5f * Time.time);

            }
            else
            {
                enemyRigid.rotation = Mathf.Lerp(this.transform.rotation.z, angle, 0.5f * Time.time);
            }
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, angle), 0.5f * Time.time);
            
        }       // if : 타겟과의 거리가 일정 수준 이상일 떄, 타겟을 향해 비행함
        else if (distance <= 5f)
        {

        }
    }       // MoveToTarget()
}

