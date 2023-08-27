using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StageUiControl : MonoBehaviour
{
    public GameObject waveClearUi; //웨이브 클리어시 나오는 애니메이션 오브젝트입니다
    public Transform[] wavePos; //웨이브 아이콘들이 있는 TRANSFORM입니다. 위치를 참조하기 위해서 썼습니다.
    public GameObject allWaveIcon; //웨이브 아이콘 오브젝트들을 들고있는 부모 오브젝트입니다

    public GameObject rewardTransition; //보상 카드를 선택하기 위한 트랜지션이 나오는 오브젝트입니다.
    public GameObject rewardUi; //카드 보상 배경ㅇ 오브젝트입니다.

    [Header("This is card pos object")]
    public GameObject firstCardPos;  // 첫번쨰 카드 등장 애니메이션입니다.
    public GameObject secondCardPos; // 두번째 카드 등장 애니메이션입니다.
    public GameObject thirdCardPos;  // 세번째 카드 등장 애니메이션입니다.

    [Header("This is card object")]
    //TODO 카드 이름을 NORMALCARD ---->>>> firstCard 로 바꿔줘야합니다.
    public GameObject normalCard;           // 첫번째 카드
    public GameObject uncommonCard;         // 두번째 카드
    public GameObject rareCard;             // 세번째 카드 
    public List<GameObject> rewardCardPool;     //실질적으로 쓰일 카드들이 들어있는 오브젝트입니다.
    private GameObject cardBlur;

    [Header("This is card Arrow")]
    public GameObject firstArrowBlur;  //첫번쨰 카드 위치의 화살표 오브젝트가 들어있는 부모 오브젝트입니다.
    public GameObject secondArrowBlur; // 위와 동일하지만 두번째 카드 위치 입니다
    public GameObject thirdArrowBlur; // 첫번쨰와 동일하지만 세번째 카드 위치 입니다

    public GameObject firstArrow;   // 첫번째 카드 위치의 화살표 오브젝트입니다.
    public GameObject secondArrow;  // 위치만 다르고 동일
    public GameObject thirdArrow;   // 위치만 다르고 동일

    [Header("victory page")]
    public GameObject victoryObj;//승리 애니메이션 출력
    public TMP_Text scoreText;
    public TMP_Text bestScoreText;

    [Header("sound page")]
    public AudioSource waveAudio;
    public AudioClip enterSound;
    public AudioClip moveSound;
    // SJ_
    // 애니메이션 시작을 위함 
    private bool isAnimation;
    // 코루틴 시작을 위함
    private bool isStartCoroutine;
    // SJ_ 
    // background 변경위한 
    private GameObject bgRoot = default;
    private List<GameObject> bgList = default;
    private int randIdx = default;

    private bool isRewardUi; //보상 ui 가 켜졌는지 확인하는 bool값입니다.
    private bool isRewardAnimation; //보상 애니메이션 출력됐는지 확인하는 bool값
    private bool isUpgradeComplete; //보상을 플레이어가 선택했는지 확인하는 bool값

    private bool isWaveClear; //웨이브 클리어 UI 애니메이션 관리하기위한 값입니다.

    private int choiceNum;
    // Start is called before the first frame update
    void Start()
    {
        // SJ_
        bgRoot = GFunc.GetRootObj("BackGrounds");
        bgList = new List<GameObject>();
        for (int i = 0; i < bgRoot.transform.childCount; i++)
        {
            bgList.Add(bgRoot.transform.GetChild(i).gameObject);
        }

        choiceNum = 1; // 카드 선택시 쓰일 기준 넘버입니다.
        isRewardAnimation = false;  //보상 애니메이션 false 값으로 놔둡니다.
        isAnimation = false;
        isStartCoroutine = true;

        waveAudio.clip = enterSound;
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.Instance.waveManager.isClear == true)
        {
            if (isStartCoroutine == true && GameManager.Instance.waveManager.curWave < 10)
            {
                StartCoroutine(VictoryStage());
                StopCoroutine(VictoryStage());
            }

        }
        if (GameManager.Instance.isVictory)  // >>>>> victoryObj를 추가합니다.
        {
            victoryObj.SetActive(true);

            scoreText.text = GameManager.Instance.scoreText.text;
            bestScoreText.text = GameManager.Instance.bestScoreText.text;
            Invoke("OffVictoryAni", 8f); // 승리 페이지 애니메이션 끝나면 글자 초기화 해줄려고 씁니다.
        }

        if (isAnimation && !GameManager.Instance.isVictory) //KH 승리 되면 실행 못하게
        {
            isAnimation = false;

            waveClearUi.SetActive(true);

            isWaveClear = true;
            waveClearUi.GetComponent<Animator>().SetBool("WaveUi", isWaveClear);
            ChangeWaveAni(); //스테이지 별로 애니메이션을 다르게 출력합니다.
        }

        if (isUpgradeComplete != true && isRewardAnimation)
        {
            SelectMoveCard();
            if (choiceNum == 0 && isRewardUi)
            {
                secondArrowBlur.SetActive(false);
                thirdArrowBlur.SetActive(false);
                firstArrowBlur.SetActive(true);


                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
                {
                    waveAudio.PlayOneShot(moveSound);
                    #region 다른 카드들은 제자리에 있도록 하는 스크립트들
                    secondArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
                    uncommonCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(uncommonCard.GetComponent<RectTransform>().anchoredPosition.x, 800f);
                    thirdArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
                    rareCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(rareCard.GetComponent<RectTransform>().anchoredPosition.x, 800f);
                    uncommonCard.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    rareCard.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    #endregion
                    StartCoroutine(MoveArrow(firstArrow));
                    StartCoroutine(UpCardPosition(normalCard));
                    StopCoroutine(MoveArrow(firstArrow));
                    StopCoroutine(UpCardPosition(normalCard));
                    normalCard.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);

                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    //rareCard.GetComponent<CardInfo>().isEnter = true;
                    waveAudio.PlayOneShot(enterSound);
                    CardInfo cardInfo = normalCard.GetComponent<CardInfo>();
                    if (cardInfo.isSpecial)//rareCard.GetComponent<CardInfo>().isSpecial)
                    {
                        cardInfo.state = CardInfo.State.Apply;
                    }
                    else
                    {
                        cardInfo.state = CardInfo.State.Plus;
                    }
                    cardInfo.ChooseCard();
                    //카드 날려주기
                    uncommonCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(-9999f, -9999f);
                    rareCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(-9999f, -9999f);
                    StartCoroutine(SelectCardAndSetOff(normalCard)); //카드 올려주고 reward ui 꺼주는 코루틴입니다.
                    StopCoroutine(SelectCardAndSetOff(normalCard));


                    rewardCardPool.Add(uncommonCard);
                    rewardCardPool.Add(rareCard);
                    isUpgradeComplete = true;

                    GameManager.Instance.waveManager.isClear = false;
                    isStartCoroutine = true;

                }       // if : Enter키를 눌러서 1번 카드를 선택하는 순간
            }
            else if (choiceNum == 1 && isRewardUi)
            {
                firstArrowBlur.SetActive(false);
                thirdArrowBlur.SetActive(false);
                secondArrowBlur.SetActive(true);

                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
                {
                    waveAudio.PlayOneShot(moveSound);
                    firstArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                    normalCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(normalCard.GetComponent<RectTransform>().anchoredPosition.x, 800f);
                    thirdArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
                    uncommonCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(uncommonCard.GetComponent<RectTransform>().anchoredPosition.x, 800f);
                    normalCard.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    uncommonCard.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);

                    StartCoroutine(MoveArrow(secondArrow));
                    StartCoroutine(UpCardPosition(rareCard));
                    StopCoroutine(MoveArrow(secondArrow));
                    StopCoroutine(UpCardPosition(rareCard));
                    rareCard.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    waveAudio.PlayOneShot(enterSound);

                    CardInfo cardInfo = rareCard.GetComponent<CardInfo>();
                    //rareCard.GetComponent<CardInfo>().isEnter = true;

                    if (cardInfo.isSpecial)//rareCard.GetComponent<CardInfo>().isSpecial)
                    {
                        cardInfo.state = CardInfo.State.Apply;
                    }
                    else
                    {
                        cardInfo.state = CardInfo.State.Plus;
                    }
                    cardInfo.ChooseCard();
                    normalCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(-9999f, -9999f);
                    uncommonCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(-9999f, -9999f);
                    StartCoroutine(SelectCardAndSetOff(rareCard)); //카드 올려주고 reward ui 꺼주는 코루틴입니다.
                    StopCoroutine(SelectCardAndSetOff(rareCard));

                    rewardCardPool.Add(uncommonCard);// Add(uncommonCard);
                    rewardCardPool.Add(normalCard);
                    isUpgradeComplete = true;


                    GameManager.Instance.waveManager.isClear = false;
                    isStartCoroutine = true;
                }       // if : Enter키를 눌러서 2번 카드를 선택하는 순간
            }
            else if (choiceNum == 2 && isRewardUi)
            {
                firstArrowBlur.SetActive(false);
                secondArrowBlur.SetActive(false);
                thirdArrowBlur.SetActive(true);
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
                {
                    waveAudio.PlayOneShot(moveSound);
                    firstArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
                    normalCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(normalCard.GetComponent<RectTransform>().anchoredPosition.x, 800f);
                    secondArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
                    rareCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(rareCard.GetComponent<RectTransform>().anchoredPosition.x, 800f);
                    normalCard.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    rareCard.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);

                    StartCoroutine(MoveArrow(thirdArrow));
                    StartCoroutine(UpCardPosition(uncommonCard));
                    StopCoroutine(MoveArrow(thirdArrow));
                    StopCoroutine(UpCardPosition(uncommonCard));
                    uncommonCard.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    waveAudio.PlayOneShot(enterSound);

                    CardInfo cardInfo = rareCard.GetComponent<CardInfo>();
                    if (cardInfo.isSpecial)//rareCard.GetComponent<CardInfo>().isSpecial)
                    {
                        cardInfo.state = CardInfo.State.Apply;
                    }
                    else
                    {
                        cardInfo.state = CardInfo.State.Plus;
                    }
                    cardInfo.ChooseCard();
                    StartCoroutine(SelectCardAndSetOff(uncommonCard)); //카드 올려주고 reward ui 꺼주는 코루틴입니다.
                    StopCoroutine(SelectCardAndSetOff(uncommonCard));


                    rewardCardPool.Add(normalCard);
                    rewardCardPool.Add(rareCard);
                    normalCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(-9999f, -9999f);
                    rareCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(-9999f, -9999f);
                    //normalCard = null;
                    //rareCard = null; //어디가 오류 났는지 찾기위해 설정해놨었습니다.
                    isUpgradeComplete = true;

                    GameManager.Instance.waveManager.isClear = false;
                    isStartCoroutine = true;

                }       // if : Enter키를 눌러서 3번 카드를 선택하는 순간
            }
            else { } //DO NOTHING
        }
        else { } //DO NOTHING

    }
    //TODO : 싹다 바꿔야함 조금씩 바꿔줘야 할 것 같습니다.
    IEnumerator VictoryStage()
    {

        isAnimation = true;
        isStartCoroutine = false;
        yield return new WaitForSeconds(1f);

        yield return new WaitForSeconds(1f);
        //waveClearUi.GetComponent<Animator>().enabled = false; //이게 웨이브 클리어시 나오는 애니메이션
        //yield return new WaitForSeconds(0.5f);
        MoveAllIcon();   //TODO: 실질적으로 그 아이콘까지 이동하는걸 하고 싶었는데 그냥 애니메이션으로 할까합니다.
        yield return new WaitForSeconds(1.5f);
        if (GameManager.Instance.waveManager.curWave == 1 || GameManager.Instance.waveManager.curWave == 2 || GameManager.Instance.waveManager.curWave == 3 || GameManager.Instance.waveManager.curWave == 6 || GameManager.Instance.waveManager.curWave == 8)
        {
            rewardTransition.SetActive(true);
            yield return new WaitForSeconds(1f);

            ChangeBackground();

            isRewardUi = true;
            choiceNum = 1;
            rewardUi.SetActive(true);
            StartCoroutine(OpenCard());
            StopCoroutine(OpenCard());
        }
        // SJ_ stage 1 추가 
        else if (GameManager.Instance.waveManager.curWave == 4 || GameManager.Instance.waveManager.curWave == 5 || GameManager.Instance.waveManager.curWave == 7 || GameManager.Instance.waveManager.curWave == 9)
        {
            GameManager.Instance.waveManager.isClear = false;
            isStartCoroutine = true;
            yield return new WaitForSeconds(1f);
        }
        //waveClearUi.SetActive(false);   //애니메이션으로 바꿔줘야하기에 끄지 않을려고합니다.
        isWaveClear = false; //waveClearUI 애니메이션 관리합니다. 결국 스테이지 클리어 전까지는 투명상태를 유지하기위해
        waveClearUi.GetComponent<Animator>().SetBool("WaveUi", isWaveClear);


        isAnimation = false;

        //StartCoroutine(OpenCard());
        //StopCoroutine(OpenCard());

        //=======================================================

    }

    // SJ_
    private void ChangeBackground()
    {
        Debug.Log("In?");

        randIdx = Random.Range(0, bgList.Count);
        foreach (GameObject gameObject in bgList)
        {
            gameObject.SetActive(false);
        }

        bgList[randIdx].SetActive(true);

    }       // ChangeBackground()


    IEnumerator OpenCard()
    {
        // 카드 선택 상태에서 플레이어 움직임 정지 
        GameManager.Instance.player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        isUpgradeComplete = false;  //업그레이드 완료했는지 확인하는 bool값입니다
        secondArrow.SetActive(false); // 카드 선택에서 2번째 카드 위치의 화살표를 꺼줍니다. 왜냐하면 처음에 카드 애니메이션 나올때 혼자만 덩그러니 나와있기 떄문에 일단 처음에 꺼줄려 했습니다.
        yield return new WaitForSeconds(0.5f);
        firstCardPos.SetActive(true); // 첫번째 카드 애니메이션을 켜줍니다.
        yield return new WaitForSeconds(0.3f);
        firstCardPos.SetActive(false);  // 하얀색 잔상으로 남아있기에 꺼줍니다.
        MoveFirstCard();                // 애니메이션 위치로 첫번째 뽑은 카드 오브젝트를 이동시켜줍니다.
        yield return new WaitForSeconds(0.5f);
        thirdCardPos.SetActive(true);   //첫번쨰 카드와 동일합니다.
        yield return new WaitForSeconds(0.3f);
        thirdCardPos.SetActive(false);
        MoveThirdCard();
        yield return new WaitForSeconds(0.5f);
        secondCardPos.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        secondCardPos.SetActive(false);
        MoveSecondCard();
        secondArrow.SetActive(true);
        yield return new WaitForSeconds(0.1f);  //모든 과정이 끝나면 
        StartCoroutine(MoveArrow(secondArrow)); // 두번쨰 화살표와 카드가 선택되어 있는걸 보여주기위해 인위적으로 움직여줍니다.
        StartCoroutine(UpCardPosition(rareCard));
        StopCoroutine(MoveArrow(secondArrow));
        StopCoroutine(UpCardPosition(rareCard));
        isRewardAnimation = true;

    }
    void SelectMoveCard()  // {선택을 하기 위한 기준 choiceNum 을 변경시켜주는 함수입니다.
    {
        if (Input.GetKeyDown(KeyCode.A) && isUpgradeComplete != true)
        {
            if (choiceNum <= 0)
            {
                choiceNum = 2;
            }
            else
            {
                choiceNum -= 1;
            }

        }
        if (Input.GetKeyDown(KeyCode.D) && isUpgradeComplete != true)
        {
            if (choiceNum >= 2)
            {
                choiceNum = 0;
            }
            else
            {
                choiceNum += 1;
            }
        }
    }
    //} 선택을 하기 위한 기준 choiceNum 을 변경시켜주는 함수 끝

    void MoveAllIcon()
    {
        if (GameManager.Instance.waveManager.curWave == 5 || GameManager.Instance.waveManager.curWave == 6 || GameManager.Instance.waveManager.curWave == 10)
        {
            float distanceBossPos = wavePos[5].position.x - wavePos[4].position.x;
            allWaveIcon.transform.position = new Vector2(allWaveIcon.transform.position.x - distanceBossPos, allWaveIcon.transform.position.y);

        }
        else
        {
            float distancPos = wavePos[1].position.x - allWaveIcon.transform.position.x;
            allWaveIcon.transform.position = new Vector2(allWaveIcon.transform.position.x - distancPos, allWaveIcon.transform.position.y);
        }

    }
    void MoveFirstCard()
    {
        if (GameManager.Instance.waveManager.curWave == 1 || GameManager.Instance.waveManager.curWave == 2 || GameManager.Instance.waveManager.curWave == 3 || GameManager.Instance.waveManager.curWave == 6 || GameManager.Instance.waveManager.curWave == 8)
        {
            SelectActiveCard(ref normalCard);
        }
        normalCard.transform.position = firstCardPos.transform.position; //선택된 카드를 첫번쨰 카드 애니메이션 위치로 포지션 바꿔줍니다
    }
    void MoveSecondCard()
    {
        if (GameManager.Instance.waveManager.curWave == 1 || GameManager.Instance.waveManager.curWave == 2 || GameManager.Instance.waveManager.curWave == 3 || GameManager.Instance.waveManager.curWave == 6 || GameManager.Instance.waveManager.curWave == 8)
        {
            SelectActiveCard(ref rareCard);
        }
        rareCard.transform.position = secondCardPos.transform.position; //선택된 카드를 두번째 카드 애니메이션 위치로 포지션 바꿔줍니다.
    }
    void MoveThirdCard()
    {
        if (GameManager.Instance.waveManager.curWave == 1 || GameManager.Instance.waveManager.curWave == 2 || GameManager.Instance.waveManager.curWave == 3 || GameManager.Instance.waveManager.curWave == 6 || GameManager.Instance.waveManager.curWave == 8)
        {
            SelectActiveCard(ref uncommonCard);
        }
        uncommonCard.transform.position = thirdCardPos.transform.position; // 선택된 카드를 세번째 카드 애니메이션 위치로 포지션 바꿔줍니다.
    }



    IEnumerator MoveArrow(GameObject arrow) //화살표 움직여주는 함수입니다.
    {
        float max = -90; //내려가는 최대 위치입니다.
        float height = arrow.GetComponent<RectTransform>().anchoredPosition.y;
        for (int i = 0; i < 20; i++)
        {
            if (height <= max) yield break;

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
            if (height >= max) yield break;

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

    IEnumerator SelectCardAndSetOff(GameObject card)
    {
        yield return new WaitForSeconds(0.000001f * Time.deltaTime);
        card.GetComponent<CardInfo>().isEnter = false;
        float max = 1500;
        float height = card.GetComponent<RectTransform>().anchoredPosition.y;
        for (int i = 0; i < 20; i++)
        {
            if (height >= max) yield break;

            height += 32f;
            card.GetComponent<RectTransform>().anchoredPosition = new Vector2(card.GetComponent<RectTransform>().anchoredPosition.x, height);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.5f);
        card.SetActive(false);
        rewardUi.SetActive(false);
        rewardTransition.GetComponent<Animator>().SetTrigger("OffCicle");
        yield return new WaitForSeconds(1f);
        // 플레이어 자유롭게 풀어주기
        GameManager.Instance.player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        rewardTransition.SetActive(false);
    }

    void ChangeWaveAni()
    {
        switch (GameManager.Instance.waveManager.curWave /*- 1*/)
        {
            case 2:
                waveClearUi.GetComponent<Animator>().SetTrigger("WaveTwoClear");
                break;
            case 3:
                waveClearUi.GetComponent<Animator>().SetTrigger("WaveThreeClear");
                break;
            case 4:
                waveClearUi.GetComponent<Animator>().SetTrigger("WaveFourClear");
                break;
            case 5:
                waveClearUi.GetComponent<Animator>().SetTrigger("WaveFiveClear");
                break;
            case 6:
                waveClearUi.GetComponent<Animator>().SetTrigger("WaveSixClear");
                break;
            case 7:
                waveClearUi.GetComponent<Animator>().SetTrigger("WaveSevenClear");
                break;
            case 8:
                waveClearUi.GetComponent<Animator>().SetTrigger("WaveEightClear");
                break;
            case 9:
                waveClearUi.GetComponent<Animator>().SetTrigger("WaveNineClear");
                break;
            case 10:
                break;
        }
    } //stageNum 에 의해서 애니메이션이 변경되는데 이게 순서를 조금 잘못 생각해서 스테이지 3일떄 2clear 애니메이션이 나와줘야 정상적으로 됩니다.

    GameObject SelectActiveCard(ref GameObject card) //ref로 값을 변경되게 해줬습니다. 안그러면 값 참조를 못해서 바꾸질 못했어서 
    {
        int cardIndex = Random.Range(0, rewardCardPool.Count);
        card = rewardCardPool[cardIndex];
        rewardCardPool.RemoveAt(cardIndex);
        return card;
    }

    void OffVictoryAni()
    {
       
        victoryObj.GetComponent<Animator>().enabled = false;
    }
}
