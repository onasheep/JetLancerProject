using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffPlayOnResult : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject playCanvasObject;
    public GameObject defeatResultObject;
    private float playerHp;
    
    // Start is called before the first frame update
    void Start()
    {
        playerHp = GameObject.Find("Player").GetComponent<playerController>().health;

    }

    // Update is called once per frame
    void Update()
    {

        playerHp = GameObject.Find("Player").GetComponent<playerController>().health;
        if (playerHp <= 0)
        {
            // Player 오브젝트 비활성화
            playerObject.SetActive(false);

            // PlayCanvas 오브젝트 비활성화
            playCanvasObject.SetActive(false);

            // DefeatResult 오브젝트 활성화
            defeatResultObject.SetActive(true);
        }
    }
}
