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

    float hp = 100f;
    

    // �� �غ� źȯ    
    float bulletFireTime = 3f;
    float time = 0f;

    // �� �غ� ������
    bool isLaser = false;

    // target Ȯ�ο� �ӽ� 
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


        // Laser ���� üũ�� Debug ��
        Debug.DrawLine(this.transform.position, dirToTarget * 1000f, Color.red);
        
        if (isLaser == false)
        {

            // TODO : Test�� ���� �ı� ���� ���� ����
            // ���� 
            //GameObject laser = Instantiate(laser_Hitbox, laserBarrelPos.transform);
            laser_Hitbox.SetActive(true);
            laser_Hitbox.transform.DOPunchScale(new Vector3(0, 1f), 5f,7, 1f);
            //�ı� 
            isLaser = true;
        }


        // { Chat Gpt �� �����Ѵ�
        // ���� § ���� �ڵ�� �ٸ����� AngleAxis ��� �κа� ȸ�� ���� ������ ���� ��ٴ� ��, ���� ��� ���ǽ� -1�� ó�����ִ� �κ�
        float theta = Vector3.Dot(laserBarrelPos.right, dirToTarget);
        float angleToTarget = Mathf.Acos(theta) * Mathf.Rad2Deg;

        Vector3 cross = Vector3.Cross(laserBarrelPos.transform.right, dirToTarget);

        if (cross.z > 0)
        {
            //Debug.Log("Player�� ����");
        }
        else if (cross.z < 0)
        {
            angleToTarget = -angleToTarget;
            //Debug.Log("Player�� ������");
        }

        Quaternion targetRotation = Quaternion.AngleAxis(angleToTarget, Vector3.forward) * laserBarrelPos.rotation;

        float rotationSpeed = 2.0f; // Adjust this as needed
        laserBarrelPos.rotation = Quaternion.Slerp(laserBarrelPos.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        // } Chat Gpt

      
        // { LEGACY : �۵� �Ҿ��ϴ� �ڵ� 
        //float theta = Vector3.Dot(laserBarrelPos.right, dirToTarget);
        //float angleToTarget = Mathf.Acos(theta) * Mathf.Rad2Deg;


        //Vector3 cross = Vector3.Cross(laserBarrelPos.transform.right, dirToTarget);

        //if (cross.z > 0)
        //{
        //    //angleToTarget = theta * Mathf.Rad2Deg;

        //    Debug.Log("Player�� ����");
        //}
        //else if (cross.z < 0)
        //{
        //    //angleToTarget = -theta * Mathf.Rad2Deg;
        //    Debug.Log("Player�� ������");
        //}

        //laserBarrelPos.rotation = Quaternion.AngleAxis(angleToTarget, Vector3.forward) * laserBarrelPos.right;
        //laserBarrelPos.rotation = Quaternion.Slerp(laserBarrelPos.rotation, targetRot, 0.5f * Time.time);

        // { LEGACY : �۵� �Ҿ��ϴ� �ڵ� 

        //laserBarrelPos.rotation = Quaternion.Slerp(laserBarrelPos.rotation, laserBarrelPos.rotation, Time.time * 0.1f);
        //    time += Time.deltaTime;
        //}
        // ���� ����
        //if (Input.GetKeyDown(KeyCode.KeypadEnter))
        //{
        //    Debug.Log("����?1");
        //    Vector3 firstDirToTarget = dirToTarget;
        //    laser_Hitbox.transform.right = firstDirToTarget;
        //    while (spendTime < 10f)
        //    {
        //        spendTime += Time.deltaTime;
        //        Debug.Log("����?1");
        //    }

        //    spendTime = 0f;
        //}
        //// ������ �߻� ������ 
        //laser_Hitbox.transform.right =
        //    Vector3.Slerp(firstDirToTarget, dirToTarget, 0.1f * Time.deltaTime);

        // ������ ��� �� ���� �ܰ�
        // TODO : ������ �� �߰� ����
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
