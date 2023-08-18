using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInfo : MonoBehaviour
{
    private GameObject childBlur;
    // Start is called before the first frame update
    void Start()
    {
        childBlur = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject; 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L)) 
        {
            childBlur.SetActive(true);
        }
    }
}
