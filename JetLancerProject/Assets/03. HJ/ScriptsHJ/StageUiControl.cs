using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageUiControl : MonoBehaviour
{
    public GameObject waveClearUi;
    public Transform[] wavePos;
    public GameObject allWaveIcon;

    public GameObject rewardTransition;
    public GameObject rewardUi;

    private bool isClear;
    private int stageNum;
    // Start is called before the first frame update
    void Start()
    {
        stageNum = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && stageNum < 10)
        {
            StartCoroutine(VictoryStage());
            StopCoroutine(VictoryStage());
        }
        Debug.LogFormat("현재 스테이지는 = {0}",stageNum);
        if(isClear)
        {
            isClear = false;
            stageNum +=1;
            waveClearUi.SetActive(true);
        }
    }
    
    IEnumerator VictoryStage()
    {
        yield return new WaitForSeconds(1f);
        isClear = true;
        yield return new WaitForSeconds(1f);
        MoveAllIcon();
        yield return new WaitForSeconds(1f);
        waveClearUi.SetActive(false);
    }
    void MoveAllIcon()
    {
        if (stageNum == 5 || stageNum == 6 || stageNum == 10)
        {
            float distanceBossPos = wavePos[5].position.x - wavePos[4].position.x;
            allWaveIcon.transform.position = new Vector2(allWaveIcon.transform.position.x-distanceBossPos, allWaveIcon.transform.position.y);
        }
        else 
        {
            float distancPos = wavePos[1].position.x - allWaveIcon.transform.position.x;
            allWaveIcon.transform.position = new Vector2(allWaveIcon.transform.position.x-distancPos, allWaveIcon.transform.position.y);
        }
    }
}
