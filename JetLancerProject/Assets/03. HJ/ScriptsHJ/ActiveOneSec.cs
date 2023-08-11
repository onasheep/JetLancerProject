using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveOneSec : MonoBehaviour
{
    public GameObject setOne, setTwo;
    

    // GameObject.Find("Player").GetComponent<playerController>().health;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("ActivateObject1", 1f);
        Invoke("ActivateObject2", 2f);

    }

 
    
    void ActivateObject1()
    {
        //GameObject.Find("whiteImg").transform.FindChild("timeSprite").gameObject.SetActive(true);
        setOne.SetActive(true);
    }
    void ActivateObject2() 
    {
        //GameObject.Find("whiteImg").transform.FindChild("waveSprite").gameObject.SetActive(true);
        setTwo.SetActive(true);
    }
}
