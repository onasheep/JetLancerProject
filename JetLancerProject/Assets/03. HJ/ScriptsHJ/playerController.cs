using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, IDamageable
{
    //오브젝트들 관련 변수
    public float moveSpeed = 5f;
    public float rotationSpeed;
    public Transform wbottlePos;
    public Transform bulletPos;
    public GameObject wPrefab;
    public GameObject bulletPrefab;
   
    public GameObject playerCanvas;
    //public GameObject defeatUi;

    public GameObject boosterSound;

    //음악 관련 변수
    public AudioClip dodgeClip;
    public AudioClip deathClip;
    public AudioClip fireClip;
    public AudioClip boosterClip;
    public Texture2D cursorIcon;

    //플레이어 스테이터스 관련 변수
    
    //SJ_
    public float health = default;
    public float maxHealth = 3f;
    private DangerPanel dangerPanel;


    //
    public float gas = 100f;
    private float maxGas = 100f;
    public float bulletSpeed;
    public float damage;

    private float shooTimer;
    private Rigidbody2D myRigid;
    private Animator myAnimator;
    private AudioSource myAudio;
    private bool isBoost;
    public bool isOverhitBoost;
    private bool isDead;

    


    // SJ_
    // TODO : 변수 이름 변경해야 함 ColliderSwitch 방식이 아님 
    public bool isDodge = default; // public : OnDamge에서 misiile을 닷지했을 떄 사용 
    private float colliderSwitchDuration; // dodge 지속시간

    // LEGACY : Collider 전환 형 닷지 
    private Collider2D[] colliders; // 모든 콜라이더 컴포넌트
    private bool isColliderSwitching = false; // 콜라이더 전환 중인지 여부

    private void Start()
    {
        Cursor.SetCursor(cursorIcon, Vector2.zero, CursorMode.Auto);
        myRigid = GetComponent<Rigidbody2D>();

        // SJ_
        // 초기화 시점
        health = maxHealth;
        damage = 1;


        playerCanvas = GFunc.GetRootObj("playCanvas");
        dangerPanel = playerCanvas.FindChildObj("danger_panel").GetComponent<DangerPanel>();

        // SJ_
        
        isOverhitBoost = false; 
        isBoost = false;
        //isFire = false;
        isDead = false;
        shooTimer = Time.time + 0.01f;//0.1f;


        bulletSpeed = 20f;

        colliderSwitchDuration = 1.0f; // 0.5f

        // 콜라이더 컴포넌트들을 모두 가져와서 배열에 저장
        colliders = GetComponents<Collider2D>();
        myAnimator = GetComponent<Animator>();
        myAudio = GetComponent<AudioSource>();


    }

    // TODO : Collider 변경점 
    private void Update()
    {
        if (isDead)
        {
            //myAnimator.SetTrigger("Die", isDead);
            //TODO 항상 실행되는 밑에 구문 바꿔야 합니다.
            //이렇게 하면 바로 굳어버리는 문제가 있어서 고민을 해봐야 할 것 같습니다.
            return;
        }
        if (!GameManager.Instance.isEngague)
        {
            MoveCharacter();
            RotateCharacter();
            //부스터 게이지 관련 시작입니다
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


            //부스터 게이지 관련 끝입니다

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
        }

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
            //transform.position += moveDirection * moveSpeed * Time.deltaTime; //단순히 이동하는
            myRigid.AddForce(moveDirection * moveSpeed * Time.deltaTime, ForceMode2D.Impulse);
            AliveChild("wBrnEffect");


            MakeCloud();

            LimitVelocity(6f);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            KillChild("wBrnEffect");
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            myAudio.PlayOneShot(boosterClip);
            //boosterSound.GetComponent<PlaySceneStartSound>().isOverHeat = false;
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
        // { player_trail scale 변동 
        float randScale = Random.Range(0.1f, 0.4f);
        player_trail.transform.localScale =
            new Vector3(randScale, randScale);
        //  player_trail scale 변동 }
    }

    private void ShotMinigun()
    {
        if (shooTimer < Time.time)
        {
            //GameObject bullet = Instantiate(bulletPrefab, bulletPos.position, transform.rotation);
            GameObject bullet = GameManager.Instance.poolManager.
                SpawnFromPool(RDefine.PLAYER_BULLET, bulletPos.position, transform.rotation );
            bullet.GetComponent<DeleteBullet>().damage = this.damage;
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
        //// 모든 콜라이더 컴포넌트의 활성/비활성 전환
        //foreach (Collider2D collider in colliders)
        //{
        //    collider.enabled = !collider.enabled;
        //}
        isDodge = true;
        
        // 지정된 시간 후에 다시 콜라이더 복구
        yield return new WaitForSeconds(colliderSwitchDuration);

        // LEGACY : 
        //foreach (Collider2D collider in colliders)
        //{
        //    collider.enabled = !collider.enabled;
        //}

        isDodge = false;

        isColliderSwitching = false; // 콜라이더 전환 상태 해제
    }

    private void OnCollisionEnter2D(Collision2D otherCollision)
    {
        Debug.Log("접촉함");
        // Damage : 1 
        OnDamage(1);
    }

    //TODO 벽 트리거 공간에 닿으면 실행시켜줄 구문 추가 예정
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
    //    {

    //    }
    //}
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
    //    {
    //        Debug.Log("찍히나?0");  
    //            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //            Vector3 moveDirection = (mousePosition - transform.position).normalized;
    //            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
    //            Quaternion currentRotation = transform.rotation;
    //            Quaternion targetQuaternion = Quaternion.Euler(new Vector3(0f, 0f, -angle));
    //            transform.rotation = Quaternion.Lerp(currentRotation, targetQuaternion, rotationSpeed * Time.deltaTime);
           
    //    }
    //}
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
    //    {

    //    }
    //}

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
    //    // TODO : Die 함수 채우기 
    //    // 여기에는 GameOver와 관련된 로직이 있으면 좋을 듯?
    //    // + 죽음 이펙트 
    //    // 죽음 이펙트
    //    playerCanvas.SetActive(false);
    //    defeatUi.SetActive(true);
    //    myRigid.constraints = RigidbodyConstraints2D.FreezeAll;

    //    GameManager.Instance.poolManager.SpawnFromPool(RDefine.ENEMY_EXPLOSION, this.transform.position, Quaternion.identity);
    //    GameManager.Instance.DefeatGame();

    //}

    // SJ_ 
    // Hp 깎는 함수 , IDamageable에서 가져옴
    public void OnDamage(int damage)
    {
        // TEST :
        if (isDodge == false)
        {
            if (health > damage)
            {
                StartCoroutine(dangerPanel.SwapImage());
                StartCoroutine(Camera.main.GetComponent<CameraFollow>().ShakeCamera());
                health -= damage;
            }       // if : damage보다 클때만 동작
            else
            {
                // 승리창 켜지면 패배 창 안나오게 설정   
                if (!GameManager.Instance.isVictory)
                {
                    StartCoroutine(DiePlayer());
                    StopCoroutine(DiePlayer());
                }
            }       // else : damage보다 작을 떄 Die() 함수 호출
        
        }       // if : Dodge 중일 때 피해 입지 않음
        else { /* Do Nothing */ }

    }       // OnDamage()

    


    IEnumerator DiePlayer()
    {
        GameManager.Instance.poolManager.SpawnFromPool(RDefine.ENEMY_EXPLOSION, this.transform.position, Quaternion.identity);
        playerCanvas.SetActive(false); //UI 꺼주기
        myRigid.constraints = RigidbodyConstraints2D.FreezeAll;  //조작 못하게 멈춰주고 
        yield return new WaitForSeconds(1.5f);
        //패배 결과창 띄어줍니다.
        GameManager.Instance.DefeatGame();
        //defeatUi.SetActive(true);
        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);
        yield break;
    }

}
