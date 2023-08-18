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

    // TEST : test�� ���� ���ε��� 
    // TODO : ���� ã�ƿͼ� ������ ������Ʈ��
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

    float hp = 1f;
    

    // �� �غ� źȯ    
    float bulletFireTime = 3f;
    float time = 0f;

    // �� �غ� ������
    bool isLaser = false;
    float track_StartTime = 3f;
    float rotationSpeed = 0f;

    // target Ȯ�ο� �ӽ� 
    private GameObject target = default;
    Vector2 dirToTarget = default;

    // PoolManger
    PoolManager poolmanager;
    
    // Start is called before the first frame update
    void Start()
    {
        // TEST GPT
        initialRotationSpeed = rotationSpeed;
        //

        poolmanager = GameManager.Instance.poolManager;
        myPattern = PATTERN.LASER;
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
                laser_Hitbox.SetActive(false);

                float pauseTime = 3f;
                if (playTime > pauseTime)
                {
                    // ������ ���� ���� ���� 
                    // ���� �����ϴ� ������ ������ �Ѿ� 3, ������ 7
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
            // TODO : Player ��ũ��Ʈ�� ã�ƿ����� �����ϰ� �Ǹ� �ٸ� �÷��̾��� ��ũ��Ʈ�� ã�ƿ��� �ɰ�
            target = FindObjectOfType<PlayerController>().gameObject;
        }       // if : Ÿ���� null �̰ų� default�� ��� Ÿ���� ������
        else
        {
            Debug.LogWarning("Target already exist.");
        }       // else : Ÿ���� ������ �α� ���
    }       // SetTarget()

    private void CheckTarget()
    {
        // ������ return
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

        while (time < 3f)
        {

            time += Time.deltaTime;
            chaseAcu -= 0.001f ;
            laserBarrelPos.transform.right = Vector3.Slerp(laserBarrelPos.transform.right, dirToTarget, Mathf.Clamp(chaseAcu,0f,1f)* Time.deltaTime);
            laser_Hitbox.transform.localScale = new Vector3(10f, Mathf.Clamp(0f + time / 2f,0f,1.5f) , 0f);
            yield return null;
        }

        laser_Hitbox.GetComponent<Collider2D>().enabled = true;

    }

    // TEST
    private bool laserTrackingStarted = false;
    private float initialRotationSpeed;
    private float trackingStartTime;
    private void FireLaser()
    {    

        if (!laserTrackingStarted)
        {
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
            laserBarrelPos.GetComponentInChildren<Animator>().SetTrigger("IsCool");
            
            Debug.LogFormat("{0}", laserBarrelPos.GetComponentInChildren<Animator>() == null);
            laserTrackingStarted = false;
            rotationSpeed = initialRotationSpeed;
            myPattern = PATTERN.NONE;
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
            myPattern = PATTERN.NONE;
        }
        // LookRotation, FromToRotation���� �ذ��Ϸ��� ��. ������ �����ְ� ȸ�� ��ü�� ������
        //barrel.transform.rotation = Quaternion.FromToRotation(Vector3.Slerp(barrel.transform.up, dirToTarget, Time.deltaTime * 0.5f),Vector3.up);

        // Quaternion���� �����Ͽ� �������� �ذ��Ϸ� ������ ������ ������ ������ ���� ����
        //barrel.transform.rotation = 
        //    Quaternion.Slerp(barrel.transform.rotation,
        //    Quaternion.FromToRotation(barrel.transform.up , dirToTarget ),Time.deltaTime * 0.5f);


        // ���ϴ� ȸ���� �������� ��Ȥ x��� y���� ���� ȸ���� �ɸ��� ��찡 ���� => ������
        barrel.transform.up = Vector3.Slerp(barrel.transform.up, dirToTarget, Time.deltaTime * 0.5f);
        // �ϴ� �� ������� ���� ���� ����
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
            //Debug.Log("Player�� ����");
        }
        else if (cross.z < 0)
        {
            //Debug.Log("Player�� ������");
        }
    }       // RotateBerral()

    private void FireBullet()
    {
        // ���� �ΰ� 

        // �Ѿ� �߻� ���� 

        // { ���� ����
        GameObject bossBullet = poolmanager.
            SpawnFromPool(RDefine.BOSS_BULLET, GunBarrel_Left.position, Quaternion.identity);
        Rigidbody2D bulletRigid =
            bossBullet.GetComponent<Rigidbody2D>();
         
        bossBullet.transform.localScale *= 2f;
        //  ���� ���� }

        // { ������ ����
        GameObject bossBullet1 = poolmanager.
            SpawnFromPool(RDefine.BOSS_BULLET, GunBarrel_Right.position, Quaternion.identity);
        Rigidbody2D bulletRigid1 =
            bossBullet1.GetComponent<Rigidbody2D>();

        bossBullet1.transform.localScale *= 2f;
        //  ������ ���� }

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
        GameManager.Instance.poolManager.
            SpawnFromPool(RDefine.ENEMY_EXPLOSION, this.transform.position , Quaternion.identity)
            .transform.localScale *= 2f;
    }

    public void OnDamage(int damage)
    {
        if (hp > damage)
        {
            hp -= damage;
        }       // if : damage���� Ŭ���� ����
        else
        {
            // TEST : OnDamge �׽�Ʈ��
            // ���� ������Ʈ Ǯ�� �߰��Ǹ� ���� ����
            Destroy(this.gameObject);
            Die();
        }       // else : damage���� ���� �� Die() �Լ� ȣ��
    }
}
