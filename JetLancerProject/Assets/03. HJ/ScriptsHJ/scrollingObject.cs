using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    public Transform targetPos; //이동할 위치입니다.

    public float speed = 0.5f;//움직이는 속도입니다.
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(targetPos.position * speed * Time.deltaTime);
    }
}
