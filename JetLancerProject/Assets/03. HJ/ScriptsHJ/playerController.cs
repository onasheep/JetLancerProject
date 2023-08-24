using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    //������Ʈ�� ���� ����
    public float moveSpeed = 5f;
    public float rotationSpeed;
    public Transform wbottlePos;
    public Transform bulletPos;
    public GameObject wPrefab;
    public GameObject bulletPrefab;
   
    public GameObject playerCanvas;
    public GameObject defeatUi;

    //���� ���� ����
    public AudioClip dodgeClip;
    public AudioClip deathClip;
    public AudioClip fireClip;
    public Texture2D cursorIcon;

    //�÷��̾� �������ͽ� ���� ����
    public float health = 3f;
    public float gas = 100f;
    private float maxGas = 100f;
    public float bulletSpeed;
    
    private float shooTimer;
    private Rigidbody2D myRigid;
    private Animator myAnimator;
    private AudioSource myAudio;
    private bool isBoost;
    public bool isOverhitBoost;
    private bool isDead;



    // SJ_
    // Damage Ȯ�ο�



    // SJ_
    // Dodge Test
    public bool isDodge = default; // public : OnDamge���� misiile�� �������� �� ��� 
    private float colliderSwitchDuration; // dodge ���ӽð�

    // LEGACY : Collider ��ȯ �� ���� 
    private Collider2D[] colliders; // ��� �ݶ��̴� ������Ʈ
    private bool isColliderSwitching = false; // �ݶ��̴� ��ȯ ������ ����

    private void Start()
    {
        Cursor.SetCursor(cursorIcon, Vector2.zero, CursorMode.Auto);
        myRigid = GetComponent<Rigidbody2D>();

        isOverhitBoost = false; 
        isBoost = false;
        //isFire = false;
        isDead = false;
        shooTimer = Time.time + 0.01f;//0.1f;


        bulletSpeed = 20f;

        colliderSwitchDuration = 1.0f; // 0.5f

        // �ݶ��̴� ������Ʈ���� ��� �����ͼ� �迭�� ����
        colliders = GetComponents<Collider2D>();
        myAnimator = GetComponent<Animator>();
        myAudio = GetComponent<AudioSource>();


    }

    // TODO : Collider ������ 
    private void Update()
    {
        if (isDead)
        {
            //myAnimator.SetTrigger("Die", isDead);
            return;
        }
        MoveCharacter();
        if (!GameManager.Instance.isEngague)
        { 
            RotateCharacter(); 
        }
        //�ν��� ������ ���� �����Դϴ�
        if (isBoost && gas > 0f && isOverhitBoost != true)
        {
            gas -= 28f * Time.deltaTime;
            BoostPlayer();
            LimitVelocity(8f);
        }
        else
        {
            if (gas < maxGas)
            {
                gas += 33f * Time.deltaTime;
            }
        }
        if (gas >= maxGas)
        {
            isOverhitBoost = false;
        }

        
        //�ν��� ������ ���� ���Դϴ�

        if (Input.GetMouseButton(0))
        {
            ShotMinigun();
        }
        if (Input.GetMouseButtonDown(1) && !isColliderSwitching)
        {
            StartCoroutine(SwitchCollidersCoroutine());
            
            myAudio.clip = dodgeClip;
            myAudio.PlayOneShot(myAudio.clip);
            isColliderSwitching = true;

        }
        myAnimator.SetBool("Dodged", isColliderSwitching);

        myAnimator.SetFloat("Rotation", transform.rotation.z);

        //Debug.Log(myRigid.velocity.magnitude);
        //Debug.Log(transform.rotation.z.ToString("F3"));
    }

    //private void OnDisable()
    //{
    //    Die();
    //}
    private void MoveCharacter()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z;
            Vector3 moveDirection = (mousePosition - transform.position).normalized;
            //transform.position += moveDirection * moveSpeed * Time.deltaTime; //�ܼ��� �̵��ϴ�
            myRigid.AddForce(moveDirection * moveSpeed * Time.deltaTime, ForceMode2D.Impulse);
            AliveChild("wBrnEffect");


            MakeCloud();

            LimitVelocity(6f);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            KillChild("wBrnEffect");
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (gas <= 0)
            {
                isOverhitBoost = true;
            }
            if (isOverhitBoost != true)
            {
                isBoost = true;
                AliveChild("downSpaceEffect");
                AliveChild("keepSpaceEffect");
                MakeCloud();
            }
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            KillChild("downSpaceEffect");
            KillChild("keepSpaceEffect");
            isBoost = false;
        }
    }
    private void BoostPlayer()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z;
            Vector3 moveDirection = (mousePosition - transform.position).normalized;
            //transform.position += moveDirection * moveSpeed * Time.deltaTime;
            myRigid.AddForce(moveDirection * (moveSpeed*1.5f) * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

    private void RotateCharacter()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 moveDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));


        Quaternion currentRotation = transform.rotation;
        Quaternion targetQuaternion = Quaternion.Euler(new Vector3(0f, 0f, angle));


        transform.rotation = Quaternion.Lerp(currentRotation, targetQuaternion, rotationSpeed * Time.deltaTime);

    }

    private void MakeCloud()
    {
        //SJ_ 
        GameObject player_trail = GameManager.Instance.poolManager.
            SpawnFromPool(RDefine.PLAYER_TRAIL, wbottlePos.position, Quaternion.identity);
        // { player_trail scale ���� 
        float randScale = Random.Range(0.1f, 0.4f);
        player_trail.transform.localScale =
            new Vector3(randScale, randScale);
        //  player_trail scale ���� }
    }

    private void ShotMinigun()
    {
        if (shooTimer < Time.time)
        {
            //GameObject bullet = Instantiate(bulletPrefab, bulletPos.position, transform.rotation);
            GameObject bullet = GameManager.Instance.poolManager.
                SpawnFromPool(RDefine.PLAYER_BULLET, bulletPos.position, transform.rotation );
            bullet.transform.localScale *= 2f;
            bullet.GetComponent<Rigidbody2D>().AddForce(this.transform.right * bulletSpeed, ForceMode2D.Impulse);
            shooTimer = Time.time + 0.11f;
            myAudio.clip = fireClip;
            myAudio.PlayOneShot(myAudio.clip);

            //myAudio.Play();
        }
    }
    private void LimitVelocity(float maxVelocity)
    {
        Vector2 clampedVelocity = myRigid.velocity;
        clampedVelocity.x = Mathf.Clamp(clampedVelocity.x, -maxVelocity, maxVelocity);
        clampedVelocity.y = Mathf.Clamp(clampedVelocity.y, -maxVelocity, maxVelocity);
        myRigid.velocity = clampedVelocity;
    }

    private IEnumerator SwitchCollidersCoroutine()
    {
        // LEGACY : 
        //// ��� �ݶ��̴� ������Ʈ�� Ȱ��/��Ȱ�� ��ȯ
        //foreach (Collider2D collider in colliders)
        //{
        //    collider.enabled = !collider.enabled;
        //}
        isDodge = true;
        
        // ������ �ð� �Ŀ� �ٽ� �ݶ��̴� ����
        yield return new WaitForSeconds(colliderSwitchDuration);

        // LEGACY : 
        //foreach (Collider2D collider in colliders)
        //{
        //    collider.enabled = !collider.enabled;
        //}

        isDodge = false;

        isColliderSwitching = false; // �ݶ��̴� ��ȯ ���� ����
    }

    private void OnCollisionEnter2D(Collision2D otherCollision)
    {
        Debug.Log("������");
        // Damage : 1 
        OnDamage(1);
    }
    
    
    private void AliveChild(string child)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform findChild = transform.GetChild(i);

            if (findChild.name == child)
            {
                findChild.gameObject.SetActive(true);
            }
        }
    }

    private void KillChild(string child)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform findChild = transform.GetChild(i);

            if (findChild.name == child)
            {
                findChild.gameObject.SetActive(false);
            }
        }
    }

    //private void Die()
    //{
    //    // TODO : Die �Լ� ä��� 
    //    // ���⿡�� GameOver�� ���õ� ������ ������ ���� ��?
    //    // + ���� ����Ʈ 
    //    // ���� ����Ʈ
    //    playerCanvas.SetActive(false);
    //    defeatUi.SetActive(true);
    //    myRigid.constraints = RigidbodyConstraints2D.FreezeAll;

    //    GameManager.Instance.poolManager.SpawnFromPool(RDefine.ENEMY_EXPLOSION, this.transform.position, Quaternion.identity);
    //    GameManager.Instance.DefeatGame();

    //}

    // SJ_ 
    // Hp ��� �Լ� , IDamageable���� ������
    public void OnDamage(int damage)
    {
        // TEST :
        if (isDodge == false)
        {
            if (health > damage)
            {
                
                health -= damage;
            }       // if : damage���� Ŭ���� ����
            else
            {
                // TEST : 0�Ǹ� Destroy()
                //Destroy(this.gameObject);
                // TEST : 0�Ǹ� off
                //Die();
                StartCoroutine(DiePlayer());
                StopCoroutine(DiePlayer());

            }       // else : damage���� ���� �� Die() �Լ� ȣ��
        
        }       // if : Dodge ���� �� ���� ���� ����
        else { /* Do Nothing */ }

    }       // OnDamage()

    IEnumerator DiePlayer()
    {
        GameManager.Instance.poolManager.SpawnFromPool(RDefine.ENEMY_EXPLOSION, this.transform.position, Quaternion.identity);
        playerCanvas.SetActive(false); //UI ���ֱ�
        myRigid.constraints = RigidbodyConstraints2D.FreezeAll;  //���� ���ϰ� �����ְ� 
        yield return new WaitForSeconds(1.5f);
        //�й� ���â ����ݴϴ�.
        GameManager.Instance.DefeatGame();
        //defeatUi.SetActive(true);
        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);
        yield break;
    }

}
