using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyAi enemyAi = default;
    private Rigidbody2D enemyRigid = default;
    private EnemyShooter shooter = default;

    private Vector2 dir = default;
    private float distance = default;

    // temp speed 
    private float moveSpeed = 5f;

    private float bulletFireRate = 0.8f;

    enum STATE
    {
        NONE = -1, IDLE, ATTACK, AROUND, DIE
    }       // AROUND : 타겟 주변을 선회하는 상태

    STATE enemyState = STATE.NONE;

    private void Awake()
    {

    }
    void Start()
    {
        enemyAi = GetComponent<EnemyAi>();
        enemyRigid = GetComponent<Rigidbody2D>();
        shooter = GetComponent<EnemyShooter>();

        enemyState = STATE.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        Transform targetPos = enemyAi.target.transform;
        distance = (targetPos.position - this.transform.position).magnitude;

        switch (enemyState)
        {
            case STATE.IDLE:
                MoveToTarget(distance, targetPos);
                break;
            case STATE.ATTACK:
                if(bulletFireRate < Time.time)
                {
                    bulletFireRate += Time.time;
                    shooter.Fire();
                    if(distance > 3f)
                    {
                        enemyState = STATE.IDLE;
                    }
                }
                break;
            case STATE.AROUND:
                break;
            case STATE.DIE:
                break;
        }
    }

    void MoveToTarget(float distance, Transform targetPos)
    {


        //// TODO : 각도 계산 후 회전 시켜줄 것
        dir = (targetPos.position - this.transform.position).normalized;

        //if(dir.x < 0)
        //{
        //    angle += 180;
        //}
        //transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        ////
        
        float enemyY = (Mathf.Sin(Time.deltaTime) * 1f);
        float enemyX = (Mathf.Sin(Time.deltaTime) * 1f);

        if(transform.rotation.x >= 0f)
        {

        }
        else
        {

        }

        float angle = Mathf.Atan(dir.y / dir.x) * Mathf.Rad2Deg;

        float rotX = transform.rotation.x;
        float rotY = transform.rotation.y;


        //this.transform.rotation = Quaternion.Euler(rotX * Mathf.Cos(rotX) - rotY * Mathf.Sin(rotY), rotX* Mathf.Sin(rotX) + rotY * Mathf.Cos(rotY),0f);
        if(angle < 0f)
        {
            this.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0f, 0f, -angle);

        }
        if (distance > 3f)
        {
            enemyRigid.AddForce(dir * moveSpeed * Time.deltaTime, ForceMode2D.Impulse);
            

        }       // if : 타겟과의 거리가 일정 수준 이상일 떄, 타겟을 향해 비행함
        //else
        //{

        //    enemyState = STATE.ATTACK;
        //}       // else : 타겟과의 거리가 일정 수준 이하일 떄, 공격하거나 선회비행함
    }
}

