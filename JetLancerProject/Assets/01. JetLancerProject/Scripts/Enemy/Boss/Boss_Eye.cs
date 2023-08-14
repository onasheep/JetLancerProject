using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;

public class Boss_Eye : MonoBehaviour, IDamageable
{
    private enum PATTERN
    {
        NONE = -1, BULLET, LASER
    }

    [SerializeField]
    PATTERN myPattern = default;

    // TEST : test를 위한 바인딩들 
    // TODO : 추후 찾아와서 가져올 오브젝트들
    public GameObject barrel = default;
    public GameObject laser_Start = default;
    public GameObject laser_Hitbox = default;
    public Transform laserBarrelPos = default;    
    public GameObject bulletPrefabs = default;
    public Transform GunBarrel_Left = default;
    public Transform GunBarrel_Right = default;

    float distToTarget = default;
    float targetAngle = default;
    float playTime = 0f;
    float count = 0f;

    float hp = 100f;
    

    // 쏠 준비 탄환    
    float bulletFireTime = 3f;
    float time = 0f;

    // 쏠 준비 레이저
    bool isLaser = false;

    // target 확인용 임시 
    private GameObject target = default;
    Vector2 dirToTarget = default;

    // PoolManger
    PoolManager poolmanager;
    
    // Start is called before the first frame update
    void Start()
    {

        poolmanager = GameManager.Instance.poolManager;
        myPattern = PATTERN.BULLET;
        SetTarget();
    }

    // Update is called once per frame
    void Update()
    {
        CheckTarget();

        switch (myPattern)
        {
            case PATTERN.NONE:
                playTime += Time.deltaTime;
                isLaser = false;

                float pauseTime = 1f;
                if (playTime > pauseTime)
                {
                    // 패턴은 추후 상세히 구현 
                    // 내가 생각하는 패턴의 비율은 총알 3, 레이저 7
                    float randValue = Random.Range(0, 10);
                    if (randValue < 3 && 0 < randValue)
                    {
                        myPattern = PATTERN.BULLET;
                    }
                    else
                    {
                        myPattern = PATTERN.LASER;
                    }
                    playTime = 0f;
                }
                break;
            case PATTERN.BULLET:

                RotateBerral();          
                break;
            case PATTERN.LASER:

                FireLaser();
                break;
        }
    }

    private void SetTarget()
    {
        if (target.IsValid() == false)
        {
            // TODO : Player 스크립트를 찾아오지만 통합하게 되면 다른 플레이어의 스크립트를 찾아오게 될것
            target = FindObjectOfType<PlayerController>().gameObject;
        }       // if : 타겟이 null 이거나 default인 경우 타겟을 가져옴
        else
        {
            Debug.LogWarning("Target already exist.");
        }       // else : 타겟이 있으면 로그 출력
    }       // SetTarget()

    private void CheckTarget()
    {
        Transform targetPos = target.transform;

        distToTarget = (targetPos.position - this.transform.position).magnitude;
        dirToTarget = (targetPos.position - this.transform.position).normalized;
        targetAngle = Mathf.Atan(dirToTarget.y / dirToTarget.x) * Mathf.Rad2Deg;
    }       // CheckTarget()


   

    private void FireLaser()
    {
        time += Time.deltaTime;

        float laserTime = 10f;
        if(time > laserTime)
        {
            
            myPattern = PATTERN.NONE;
            laser_Hitbox.SetActive(false);
        }


        // Laser 방향 체크용 Debug 선
        Debug.DrawLine(this.transform.position, dirToTarget * 1000f, Color.red);
        
        if (isLaser == false)
        {

            // TODO : Test용 생성 파괴 로직 추후 수정
            // 생성 
            //GameObject laser = Instantiate(laser_Hitbox, laserBarrelPos.transform);
            laser_Hitbox.SetActive(true);
            laser_Hitbox.transform.DOPunchScale(new Vector3(0, 1f), 5f,7, 1f);
            //파괴 
            isLaser = true;
        }


        // { Chat Gpt 잘 동작한다
        // 내가 짠 원본 코드와 다른점은 AngleAxis 사용 부분과 회전 값을 변수로 만들어서 줬다는 점, 외적 사용 조건시 -1을 처리해주는 부분
        float theta = Vector3.Dot(laserBarrelPos.right, dirToTarget);
        float angleToTarget = Mathf.Acos(theta) * Mathf.Rad2Deg;

        Vector3 cross = Vector3.Cross(laserBarrelPos.transform.right, dirToTarget);

        if (cross.z > 0)
        {
            //Debug.Log("Player가 왼쪽");
        }
        else if (cross.z < 0)
        {
            angleToTarget = -angleToTarget;
            //Debug.Log("Player가 오른쪽");
        }

        Quaternion targetRotation = Quaternion.AngleAxis(angleToTarget, Vector3.forward) * laserBarrelPos.rotation;

        float rotationSpeed = 2.0f; // Adjust this as needed
        laserBarrelPos.rotation = Quaternion.Slerp(laserBarrelPos.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        // } Chat Gpt

      
        // { LEGACY : 작동 불안하던 코드 
        //float theta = Vector3.Dot(laserBarrelPos.right, dirToTarget);
        //float angleToTarget = Mathf.Acos(theta) * Mathf.Rad2Deg;


        //Vector3 cross = Vector3.Cross(laserBarrelPos.transform.right, dirToTarget);

        //if (cross.z > 0)
        //{
        //    //angleToTarget = theta * Mathf.Rad2Deg;

        //    Debug.Log("Player가 왼쪽");
        //}
        //else if (cross.z < 0)
        //{
        //    //angleToTarget = -theta * Mathf.Rad2Deg;
        //    Debug.Log("Player가 오른쪽");
        //}

        //laserBarrelPos.rotation = Quaternion.AngleAxis(angleToTarget, Vector3.forward) * laserBarrelPos.right;
        //laserBarrelPos.rotation = Quaternion.Slerp(laserBarrelPos.rotation, targetRot, 0.5f * Time.time);

        // { LEGACY : 작동 불안하던 코드 

        //laserBarrelPos.rotation = Quaternion.Slerp(laserBarrelPos.rotation, laserBarrelPos.rotation, Time.time * 0.1f);
        //    time += Time.deltaTime;
        //}
        // 최초 조준
        //if (Input.GetKeyDown(KeyCode.KeypadEnter))
        //{
        //    Debug.Log("들어옴?1");
        //    Vector3 firstDirToTarget = dirToTarget;
        //    laser_Hitbox.transform.right = firstDirToTarget;
        //    while (spendTime < 10f)
        //    {
        //        spendTime += Time.deltaTime;
        //        Debug.Log("들어옴?1");
        //    }

        //    spendTime = 0f;
        //}
        //// 데미지 발사 레이저 
        //laser_Hitbox.transform.right =
        //    Vector3.Slerp(firstDirToTarget, dirToTarget, 0.1f * Time.deltaTime);

        // 레이저 쏘기 전 사전 단계
        // TODO : 폴리싱 때 추가 예정
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    GameObject laser = Instantiate(laser_Start, headPos.position, Quaternion.identity);
        //    float punchTime = 5f;

        //    laser.transform.DOPunchScale(new Vector3(-2f, -2f, 0f), punchTime, 5, 0.3f);

    }       // FireLaser()


    private void RotateBerral()
    {
        if (count > 3)
        {
            count = 0;
            myPattern = PATTERN.NONE;
        }
        // LookRotation, FromToRotation으로 해결하려고 함. 짐벌락 남아있고 회전 자체가 망가짐
        //barrel.transform.rotation = Quaternion.FromToRotation(Vector3.Slerp(barrel.transform.up, dirToTarget, Time.deltaTime * 0.5f),Vector3.up);

        // Quaternion으로 변경하여 짐벌락을 해결하려 했지만 여전히 짐벌락 현상이 남아 있음
        //barrel.transform.rotation = 
        //    Quaternion.Slerp(barrel.transform.rotation,
        //    Quaternion.FromToRotation(barrel.transform.up , dirToTarget ),Time.deltaTime * 0.5f);


        // 원하는 회전이 나오지만 간혹 x축과 y축이 같이 회전이 걸리는 경우가 있음 => 짐벌락
        barrel.transform.up = Vector3.Slerp(barrel.transform.up, dirToTarget, Time.deltaTime * 0.5f);
        // 일단 이 방법으로 가고 추후 수정
        time += Time.deltaTime;
        if (time > bulletFireTime)
        {
            time = 0;
            FireBullet();
            count++;
          
        }

        Vector3 cross = Vector3.Cross(barrel.transform.up, dirToTarget);

        if (cross.z > 0)
        {
            //Debug.Log("Player가 왼쪽");
        }
        else if (cross.z < 0)
        {
            //Debug.Log("Player가 오른쪽");
        }
    }       // RotateBerral()

    private void FireBullet()
    {
        // 포문 두개 


        // 총알 발사 로직 


        //GameObject bossBullet = 
        //    Instantiate(bulletPrefabs, GunBarrel_Left.position, Quaternion.identity);
        //Rigidbody2D bulletRigid =
        //    bossBullet.GetComponent<Rigidbody2D>();

        //GameObject bossBullet1 = 
        //    Instantiate(bulletPrefabs, GunBarrel_Right.position, Quaternion.identity);
        //Rigidbody2D bulletRigid1 =
        //    bossBullet1.GetComponent<Rigidbody2D>();


        GameObject bossBullet = poolmanager.
            SpawnFromPool(RDefine.BOSS_BULLET, GunBarrel_Left.position, Quaternion.identity);
        Rigidbody2D bulletRigid =
            bossBullet.GetComponent<Rigidbody2D>();
        GameObject bossBullet1 = poolmanager.
            SpawnFromPool(RDefine.BOSS_BULLET, GunBarrel_Right.position, Quaternion.identity);
        Rigidbody2D bulletRigid1 =
            bossBullet1.GetComponent<Rigidbody2D>();

        float bulletSpeed = 2f;
        bulletRigid.AddForce(barrel.transform.up * bulletSpeed, ForceMode2D.Impulse);
        bulletRigid.transform.right = barrel.transform.up;
        bulletRigid1.AddForce(barrel.transform.up * bulletSpeed, ForceMode2D.Impulse);
        bulletRigid1.transform.right = barrel.transform.up;

        //Invoke(GameManager.Instance.poolManager.DeactiveObj(bossBullet), 5);
        //Invoke(GameManager.Instance.poolManager.DeactiveObj(bossBullet1), 5);


    }

    private void Die()
    {


    }

    public void OnDamage(int damage)
    {
        if (hp > damage)
        {
            hp -= damage;
        }       // if : damage보다 클때만 동작
        else
        {
            // TEST : OnDamge 테스트용
            // 추후 오브젝트 풀이 추가되면 수정 예정
            Destroy(this.gameObject);
            Die();
        }       // else : damage보다 작을 떄 Die() 함수 호출
    }
}
