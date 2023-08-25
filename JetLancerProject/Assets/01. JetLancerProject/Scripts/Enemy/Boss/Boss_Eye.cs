using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Eye : MonoBehaviour, IDamageable, IDeactive
{
    private enum PATTERN
    {
        NONE = -1, WAIT, BULLET, LASER
    }

    PATTERN myPattern = default;
        
    // TEST : test를 위한 바인딩들 
    // TODO : 추후 찾아와서 가져올 오브젝트들
    public GameObject barrel = default;
    public GameObject laser_Start = default;
    public GameObject laser_Hitbox = default;
    public Transform laserBarrelPos = default;    
    public Transform GunBarrel_Left = default;
    public Transform GunBarrel_Right = default;

    // 본체 중앙 구 spriteRender를 가져오기 위함
    private GameObject bossHead = default;
    private SpriteRenderer sprite = default;
    public Material[] materials = default;

    private float distToTarget = default;
    private float targetAngle = default;
    private float playTime = 0f;
    private float count = 0f;

    private float hp = 100f;


    // 쏠 준비 탄환    
    private float bulletFireTime = 3f;
    private float time = 0f;

    // 쏠 준비 레이저
    private bool isLaser = false;
    private float rotationSpeed = 0f;

    // target 확인용 임시 
    private GameObject target = default;
    private Vector2 dirToTarget = default;

    // PoolManger
    private PoolManager poolmanager;

    // AudioSourece
    private AudioSource audioSource;
    public AudioClip fireClip;
    public AudioClip laserPrepClip;
    public AudioClip laserFireClip;
    public AudioClip laserAfterClip;

    // Start is called before the first frame update
    private void Awake()
    {
        Init();

    }

    void Init()
    {
        initialRotationSpeed = rotationSpeed;

        poolmanager = GameManager.Instance.poolManager;
        bossHead = this.gameObject.FindChildObj("BossHead");
        sprite = bossHead.FindChildComponent<SpriteRenderer>("sp_boss_first_head_0");
        audioSource = GetComponent<AudioSource>();
        myPattern = PATTERN.WAIT;
        SetTarget();
    }

    // Update is called once per frame
    void Update()
    {
        CheckTarget();

        switch (myPattern)
        {
            case PATTERN.WAIT:
                playTime += Time.deltaTime;
                isLaser = false;
                laser_Hitbox.SetActive(false);

                float pauseTime = 3f;
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
        // 없으면 return
        if (target.IsValid() == false) { return; }
        
        Transform targetPos = target.transform;

        distToTarget = (targetPos.position - this.transform.position).magnitude;
        dirToTarget = (targetPos.position - this.transform.position).normalized;
        targetAngle = Mathf.Atan(dirToTarget.y / dirToTarget.x) * Mathf.Rad2Deg;
    }       // CheckTarget()


   
    private IEnumerator PrepareLaser()
    {
        float time = 0f;
        float chaseAcu = 1f;
        laserBarrelPos.transform.right = dirToTarget;
        laser_Hitbox.transform.localScale = new Vector3(10f, 0.1f,0f);

        audioSource.PlayOneShot(laserPrepClip);

        while (time < 3f)
        {
            time += Time.deltaTime;
            chaseAcu -= 0.001f ;
            laserBarrelPos.transform.right = Vector3.Slerp(laserBarrelPos.transform.right, dirToTarget, Mathf.Clamp(chaseAcu,0f,1f)* Time.deltaTime);
            laser_Hitbox.transform.localScale = new Vector3(10f, Mathf.Clamp(0f + time / 2f,0f,2.0f) , 0f);
            yield return null;
        }

        laser_Hitbox.transform.DOPunchScale(new Vector3(0.5f, 0.5f), 1, 1, 5f);
        laser_Hitbox.GetComponent<Collider2D>().enabled = true;
        audioSource.PlayOneShot(laserFireClip);

    }

    // TEST
    private bool laserTrackingStarted = false;
    private float initialRotationSpeed;
    private float trackingStartTime;
    private void FireLaser()
    {    

        if (!laserTrackingStarted)
        {
            audioSource.PlayOneShot(laserPrepClip);
            laser_Start.SetActive(true);
            laser_Start.transform.DOPunchScale(new Vector3(1f, 1f), 10f, 10, 0.5f);
            
            laser_Hitbox.GetComponent<Collider2D>().enabled = false;  
            laser_Hitbox.SetActive(true);
            StartCoroutine(PrepareLaser());
            laserTrackingStarted = true;
            trackingStartTime = Time.time;
        }


        float elapsedTime = Time.time - trackingStartTime;

        float laserTime = 10f;
        if (elapsedTime > laserTime)
        {

            // Reset laser tracking settings
            laser_Hitbox.SetActive(false);

            audioSource.PlayOneShot(laserAfterClip);
            laserBarrelPos.GetComponentInChildren<Animator>().SetTrigger("IsCool");

            laserTrackingStarted = false;
            rotationSpeed = initialRotationSpeed;
            myPattern = PATTERN.WAIT;
            laser_Start.SetActive(false);
            isLaser = false;
        }
        else
        {

            float normalizedTime = elapsedTime / laserTime;

            // Gradually speed up the tracking speed
            float currentRotationSpeed = 0.4f;
            rotationSpeed = currentRotationSpeed;

            // Calculate target direction
            Vector3 dirToTarget = target.transform.position - laserBarrelPos.position;
            dirToTarget.Normalize();

            float theta = Vector3.Dot(laserBarrelPos.transform.right, dirToTarget);
            float angleToTarget = Mathf.Acos(theta) * Mathf.Rad2Deg;

            Vector3 cross = Vector3.Cross(laserBarrelPos.transform.right, dirToTarget);

            if (cross.z < 0)
            {
                angleToTarget = -angleToTarget;
            }

            Quaternion targetRotation = laserBarrelPos.rotation * Quaternion.AngleAxis(angleToTarget, transform.forward);

            // Gradually adjust the laser barrel's rotation
            laserBarrelPos.rotation = Quaternion.Slerp(laserBarrelPos.rotation, targetRotation, currentRotationSpeed * normalizedTime * Time.deltaTime);

            // Additional code for laser_Start animation and isLaser logic
            if (!isLaser)
            {
                isLaser = true;
            }
        }
    
    }       // FireLaser()


    private void RotateBerral()
    {
        if (count > 3)
        {
            count = 0;
            myPattern = PATTERN.WAIT;
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

            audioSource.PlayOneShot(fireClip);

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
        // 총알 발사 로직 

        // { 왼쪽 포문
        GameObject bossBullet = poolmanager.
            SpawnFromPool(RDefine.BOSS_BULLET, GunBarrel_Left.position, Quaternion.identity);
        Rigidbody2D bulletRigid =
            bossBullet.GetComponent<Rigidbody2D>();
         
        bossBullet.transform.localScale *= 2f;
        //  왼쪽 포문 }

        // { 오른쪽 포문
        GameObject bossBullet1 = poolmanager.
            SpawnFromPool(RDefine.BOSS_BULLET, GunBarrel_Right.position, Quaternion.identity);
        Rigidbody2D bulletRigid1 =
            bossBullet1.GetComponent<Rigidbody2D>();

        bossBullet1.transform.localScale *= 2f;
        //  오른쪽 포문 }

        // 탄환 재생

        float bulletSpeed = 2f;
        bulletRigid.AddForce(barrel.transform.up * bulletSpeed, ForceMode2D.Impulse);
        bulletRigid.transform.right = barrel.transform.up;
        bulletRigid1.AddForce(barrel.transform.up * bulletSpeed, ForceMode2D.Impulse);
        bulletRigid1.transform.right = barrel.transform.up;


    }

    private void Die()
    {

        GameManager.Instance.poolManager.
            SpawnFromPool(RDefine.ENEMY_EXPLOSION, this.transform.position , Quaternion.identity)
            .transform.localScale *= 2f;
    }

    public void OnDamage(int damage)
    {
        if (hp > damage)
        {
            StartCoroutine(SwapSprite());
            hp -= damage;
        }       // if : damage보다 클때만 동작
        else
        {
            // TEST : OnDamge 테스트용
            // 추후 오브젝트 풀이 추가되면 수정 예정
            Deactive();
            Die();
        }       // else : damage보다 작을 떄 Die() 함수 호출
    }
    IEnumerator SwapSprite()
    {
        sprite.sharedMaterial = materials[1];
        yield return new WaitForSeconds(0.1f);
        sprite.sharedMaterial = materials[0];
    }       // SwapSprite()

    public void Deactive()
    {
        // Interface 내용
        GameManager.Instance.AddScore(200);

        this.gameObject.SetActive(false);

    }       // Deactive()
}
