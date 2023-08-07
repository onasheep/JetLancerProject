using UnityEngine;

public class playerController : MonoBehaviour
{
    
    public float moveSpeed = 5f;
    public float rotationSpeed;
    public Transform wbottlePos;

    public GameObject wPrefab;
    public Texture2D cursorIcon;
    public float gas = 100f;
    private Rigidbody2D myRigid;
    private bool isBoost;

    private void Start()
    {
        Cursor.SetCursor(cursorIcon, Vector2.zero, CursorMode.Auto);
        myRigid = GetComponent<Rigidbody2D>();

        isBoost = false;
    }
    private void FixedUpdate()
    {
        
        if (isBoost && gas > 0f)
        {
            gas -= 0.5f;
            BoostPlayer();
        }
        else
        {
            if (gas < 100f)
            {
                gas += 0.5f;
            }
            isBoost = false;
        }
        Debug.Log(gas);
    }
    private void Update()
    {
        MoveCharacter();
        RotateCharacter();

        //LimitVelocity(5f);
        Debug.Log(myRigid.velocity.magnitude);
        //if (Mathf.Abs(myRigid.velocity.x) > 8)
        //{
        //    myRigid.velocity = new Vector2(Mathf.Sign(myRigid.velocity.x) * 6, myRigid.velocity.y);
        //}
        //else if (Mathf.Abs(myRigid.velocity.y) > 8)
        //{
        //    myRigid.velocity = new Vector2(myRigid.velocity.x, Mathf.Sign(myRigid.velocity.y)* 6 );
        //}
    }

    private void MoveCharacter()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z;
            Vector3 moveDirection = (mousePosition - transform.position).normalized;
            //transform.position += moveDirection * moveSpeed * Time.deltaTime;
            myRigid.AddForce(moveDirection *moveSpeed * Time.deltaTime, ForceMode2D.Impulse);
            MakeCloud();
            LimitVelocity(5f);
            //MoveTowardsMousePointer();
        }
        
        if (Input.GetKey(KeyCode.Space))
        {
            isBoost = true;
            LimitVelocity(10f);
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
        Instantiate(wPrefab,wbottlePos.position,transform.rotation);
    }

    private void LimitVelocity(float maxVelocity)
    {
        Vector2 clampedVelocity = myRigid.velocity;
        clampedVelocity.x = Mathf.Clamp(clampedVelocity.x, -maxVelocity, maxVelocity);
        clampedVelocity.y = Mathf.Clamp(clampedVelocity.y, -maxVelocity, maxVelocity);
        myRigid.velocity = clampedVelocity;
    }

}
