using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudInstantiate : MonoBehaviour
{
    public GameObject[] cloudPrefab;
    public Transform cloudPos;

    private int cloudCount;
    private float cloudTimer;
    // Start is called before the first frame update
    void Start()
    {
        cloudTimer = Time.time + 0.5f;
    }
    private void FixedUpdate()
    {
        if (cloudTimer < Time.time)
        {
            GameObject cloud = Instantiate(cloudPrefab[cloudCount], cloudPos.position, transform.rotation);
            cloudCount += 1;
            cloudTimer = Time.time + 0.1f;
        }
        if (cloudCount == 4)
        {
            cloudCount = 0;
        }
    }
   
}
