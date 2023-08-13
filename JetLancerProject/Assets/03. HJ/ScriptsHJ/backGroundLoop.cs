using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundLoop : MonoBehaviour
{
    public Transform targetPos;
    private float distance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(targetPos.position, transform.position);
        if(distance < 5f)
        {
            Reposition();
        }
    }

    private void Reposition()
    {
        transform.position = new Vector2(0, 0);
    }
}
