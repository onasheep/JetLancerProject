using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteWbottle : MonoBehaviour
{
    private float deleteTimer;
    // Start is called before the first frame update
    void Start()
    {
        deleteTimer = Time.time + 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (deleteTimer < Time.time)
        {
            Destroy(gameObject);
        }
    }
}
