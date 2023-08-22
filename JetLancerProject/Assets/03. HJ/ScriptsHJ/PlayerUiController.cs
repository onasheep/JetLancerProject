using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUiController : MonoBehaviour
{

    //[SerializeField]
    //private 
    public WaitForSeconds waitDotOneSec = new WaitForSeconds(0.1f);
    //켜줬다 꺼줄 오브젝트
    public GameObject hpBarRed;
    public GameObject leftFlipRed;
    public GameObject rightFlitRed;

    //참조할 부스터 게이지 오브젝트
    public GameObject player;

    private float playerGas;
    public bool isPlayerOverhit;
    public bool isHit = false;
    

    // Start is called before the first frame update
    void Start()
    {
       
        playerGas = player.GetComponent<PlayerController>().gas;
        //StartCoroutine(ActiveDie());

    }

    // Update is called once per frame
    void Update()
    {
        isPlayerOverhit = player.GetComponent<PlayerController>().isOverhitBoost;
        if (isPlayerOverhit && isHit == false)
        {
            isHit = true;
            StartCoroutine(ActiveDie());
            StopCoroutine(ActiveDie());
        }
    }
    IEnumerator ActiveDie()
    {
        while (isPlayerOverhit)
        {
            yield return waitDotOneSec;
            hpBarRed.SetActive(true);
            rightFlitRed.SetActive(true);
            leftFlipRed.SetActive(true);
            yield return waitDotOneSec;
            hpBarRed.SetActive(false);
            rightFlitRed.SetActive(false);
            leftFlipRed.SetActive(false);
        }
        isHit = false;
        if(!isPlayerOverhit)
            yield break;
    }
    void CheckGasMoveFlip()
    {
        if (isPlayerOverhit)
        {
            if (playerGas >= 0f && playerGas < 7f)
            {

            }
            else if (playerGas >= 7f && playerGas < 14f)
            {
            }
            else if (playerGas >= 14f && playerGas < 21f)
            {
            }
            else if (playerGas >= 21f && playerGas < 28f)
            {
            }
            else if (playerGas >= 28f && playerGas < 35f)
            {
            }
            else if (playerGas >= 35f && playerGas < 42f)
            {
            }
            else if (playerGas >= 42f && playerGas < 49f)
            {
            }
            else if (playerGas >= 49f && playerGas < 56f)
            {
            }
            else if (playerGas >= 56f && playerGas < 63f)
            {
            }
            else if (playerGas >= 63f && playerGas < 70f)
            {
            }
            else if (playerGas >= 70f && playerGas < 77f)
            {
            }
            else if (playerGas >= 77f && playerGas < 84f)
            {
            }
            else if (playerGas >= 84f && playerGas < 91f)
            {
            }
            else if (playerGas >= 91f && playerGas < 98f)
            {
            }
            else
            {
            }
        }
        else
        {
            if (playerGas >= 0f && playerGas < 7f)
            {

            }
            else if (playerGas >= 7f && playerGas < 14f)
            {
            }
            else if (playerGas >= 14f && playerGas < 21f)
            {
            }
            else if (playerGas >= 21f && playerGas < 28f)
            {
            }
            else if (playerGas >= 28f && playerGas < 35f)
            {
            }
            else if (playerGas >= 35f && playerGas < 42f)
            {
            }
            else if (playerGas >= 42f && playerGas < 49f)
            {
            }
            else if (playerGas >= 49f && playerGas < 56f)
            {
            }
            else if (playerGas >= 56f && playerGas < 63f)
            {
            }
            else if (playerGas >= 63f && playerGas < 70f)
            {
            }
            else if (playerGas >= 70f && playerGas < 77f)
            {
            }
            else if (playerGas >= 77f && playerGas < 84f)
            {
            }
            else if (playerGas >= 84f && playerGas < 91f)
            {
            }
            else if (playerGas >= 91f && playerGas < 98f)
            {
            }
            else
            {
            }
        }
    }
}
