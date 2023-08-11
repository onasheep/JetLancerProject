using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDelayOff : MonoBehaviour
{
    
    

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DeactiveObject", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void DeactiveObject()
    {
        gameObject.SetActive(false);
    }
}
