using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    public Transform targetPos; //�̵��� ��ġ�Դϴ�.

    public float speed = 0.5f;//�����̴� �ӵ��Դϴ�.
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
