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

    [Header("This is card pos object")]
    public GameObject firstCardPos;
    public GameObject secondCardPos;
    public GameObject thirdCardPos;

    [Header("This is card object")]
    public GameObject normalCard;
    public GameObject uncommonCard;
    public GameObject rareCard;
    public GameObject legendaryCard;

    [Header("This is card Arrow")]
    public GameObject firstArrowBlur;
    public GameObject secondArrowBlur;
    public GameObject thirdArrowBlur;

    public GameObject firstArrow;
    public GameObject secondArrow;
    public GameObject thirdArrow;

    private bool isClear;
    private int stageNum;

    private bool isRewardUi;
    private bool isRewardAnimation;
    private bool isUpgradeComplete;

    private int choiceNum;
    // Start is called before the first frame update
    void Start()
    {
        stageNum = 1;
        choiceNum = 1;
        isRewardAnimation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && stageNum < 10 )
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
            //waveClearUi.GetComponent<Animator>().enabled = false;

        }

        if (isUpgradeComplete != true && isRewardAnimation )
        {
            
            SelectMoveCard();
            if(choiceNum == 0 && isRewardUi)
            {
                secondArrowBlur.SetActive(false);
                thirdArrowBlur.SetActive(false);
                firstArrowBlur.SetActive(true);


                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
                {
                    secondArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
                    uncommonCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(uncommonCard.GetComponent<RectTransform>().anchoredPosition.x, 800f);
                    thirdArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
                    rareCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(rareCard.GetComponent<RectTransform>().anchoredPosition.x, 800f);

                    StartCoroutine(MoveArrow(firstArrow));
                    StartCoroutine(UpCardPosition(normalCard));

                    StopCoroutine(MoveArrow(firstArrow));
                    StopCoroutine(UpCardPosition(normalCard));
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Debug.Log("첫번째 카드 선택");
                }
            }
            if(choiceNum == 1 && isRewardUi) 
            {
                firstArrowBlur.SetActive(false);
                thirdArrowBlur.SetActive(false);
                secondArrowBlur.SetActive(true);

                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
                {
                    firstArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                    normalCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(normalCard.GetComponent<RectTransform>().anchoredPosition.x, 800f);
                    thirdArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
                    uncommonCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(uncommonCard.GetComponent<RectTransform>().anchoredPosition.x, 800f);

                    StartCoroutine(MoveArrow(secondArrow));
                    StartCoroutine(UpCardPosition(rareCard));
                    StopCoroutine(MoveArrow(secondArrow));
                    StopCoroutine(UpCardPosition(rareCard));
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Debug.Log("두번째 카드 선택");
                }
            }
            if (choiceNum == 2 && isRewardUi)
            {
                firstArrowBlur.SetActive(false);
                secondArrowBlur.SetActive(false);
                thirdArrowBlur.SetActive(true) ;
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
                {
                    firstArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
                    normalCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(normalCard.GetComponent<RectTransform>().anchoredPosition.x, 800f);
                    secondArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
                    rareCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(rareCard.GetComponent<RectTransform>().anchoredPosition.x, 800f);

                    StartCoroutine(MoveArrow(thirdArrow));
                    StartCoroutine(UpCardPosition(uncommonCard));
                    StopCoroutine(MoveArrow(thirdArrow));
                    StopCoroutine(UpCardPosition(uncommonCard));
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Debug.Log("세번째 카드 선택");
                }
            }
            else
            {
                //DO NOTHING
            }
        }
    }
    //TODO : 싹다 바꿔야함 
    IEnumerator VictoryStage()
    {
        yield return new WaitForSeconds(1f);
        isClear = true;
        yield return new WaitForSeconds(1.5f);
        waveClearUi.GetComponent<Animator>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        MoveAllIcon();
        yield return new WaitForSeconds(1.5f);
        rewardTransition.SetActive(true);
        yield return new WaitForSeconds(1f);
        if (stageNum == 2 || stageNum == 3 || stageNum == 4 || stageNum == 7 || stageNum == 9)
        {
            isRewardUi = true;
        }

        rewardUi.SetActive(true);
        waveClearUi.SetActive(false);
        StartCoroutine(OpenCard());
        StopCoroutine(OpenCard());
        //=======================================================

    }

    IEnumerator OpenCard()
    {
        isUpgradeComplete = false;
        secondArrow.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        firstCardPos.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        firstCardPos.SetActive(false);
        MoveFirstCard();
        yield return new WaitForSeconds(0.5f);
        thirdCardPos.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        thirdCardPos.SetActive(false);
        MoveThirdCard();
        yield return new WaitForSeconds(0.5f);
        secondCardPos.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        secondCardPos.SetActive(false);
        MoveSecondCard();
        secondArrow.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(MoveArrow(secondArrow));
        StartCoroutine(UpCardPosition(rareCard));
        StopCoroutine(MoveArrow(secondArrow));
        StopCoroutine(UpCardPosition(rareCard));
        isRewardAnimation = true;

    }
    void SelectMoveCard()
    {
        if(Input.GetKeyDown(KeyCode.A) && isUpgradeComplete != true) 
        {
            if(choiceNum <= 0)
            {
                choiceNum = 2;
            }
            else 
            {
                choiceNum -= 1;
            }

        }
        if(Input.GetKeyDown(KeyCode.D) && isUpgradeComplete != true)
        {
            if(choiceNum >= 2)
            {
                choiceNum = 0;
            }
            else
            {
                choiceNum += 1;
            }
        }
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
            Debug.Log("실행 됨?");
            float distancPos = wavePos[1].position.x - allWaveIcon.transform.position.x;
            allWaveIcon.transform.position = new Vector2(allWaveIcon.transform.position.x-distancPos, allWaveIcon.transform.position.y);
        }
        
    }
    void MoveFirstCard()
    {
        normalCard.transform.position = firstCardPos.transform.position;
    }
    void MoveSecondCard()
    {
        rareCard.transform.position = secondCardPos.transform.position;
    }
    void MoveThirdCard()
    {
        uncommonCard.transform.position = thirdCardPos.transform.position;
    }



    IEnumerator MoveArrow(GameObject arrow)
    {
        float max = -90;
        float height = arrow.GetComponent<RectTransform>().anchoredPosition.y;
        for (int i = 0; i < 20; i ++)
        {
            if (height<=max) yield break;

            height -= 4.5f;
            arrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(arrow.GetComponent<RectTransform>().anchoredPosition.x, height);
            yield return new WaitForSeconds(0.01f);
            //TODO 올라오는 예외처리 해줘야합니다.
            //TODO : 현재 상태를 저장해두고 벗어날 시 현재 상태로 돌아오게끔)
            /*            if (!select)
                            yield return new WaitForSeconds(0.01f);
                        else
                            yield return new WaitForSeconds(0.001f);
            */
        }
    }

    IEnumerator UpCardPosition(GameObject card)
    {

        float max = 840;
        float height = card.GetComponent<RectTransform>().anchoredPosition.y;
        for (int i = 0; i < 20; i++)
        {
            if (height>=max) yield break;

            height += 2f;
            card.GetComponent<RectTransform>().anchoredPosition = new Vector2(card.GetComponent<RectTransform>().anchoredPosition.x, height);
            yield return new WaitForSeconds(0.01f);
            //TODO 내려오는 예외처리 해줘야합니다.
            //TODO : 현재 상태를 저장해두고 벗어날 시 현재 상태로 돌아오게끔)
            /*            if (!select)
                            yield return new WaitForSeconds(0.01f);
                        else
                            yield return new WaitForSeconds(0.001f);
            */

        }
    }


}
