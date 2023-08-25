using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatAniScript : MonoBehaviour
{
    public GameObject setObject;

    public Transform targetPos;
    private float moveSpeed;
    private float delayTime;
    //private float setTime = 3f;
    //private Animator defeatAni;

    // Start is called before the first frame update
    void Start()
    {
        delayTime = 1.5f;
        moveSpeed = 0.8f;
        StartCoroutine(ActiveObj());
    }

    // Update is called once per frame
    void Update()
    {
        if (delayTime >= Time.deltaTime)
        {
            moveObject();
        }
        //if (setTime >= Time.deltaTime) 
        //{
        //    setObject.SetActive(true);
        //    //defeatAni.SetBool("isIng", isMove);
        //}
       
      
    }

    void moveObject()
    {
        transform.position =  Vector2.Lerp(transform.position, targetPos.position, moveSpeed * Time.deltaTime);

    }

    IEnumerator ActiveObj()
    {
        //yield return new WaitForSeconds(delayTime);

        yield return new WaitForSeconds(3f);
        setObject.SetActive(true);


        //
    }

}
