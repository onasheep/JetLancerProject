using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageUiControl : MonoBehaviour
{
    public GameObject waveClearUi; //���̺� Ŭ����� ������ �ִϸ��̼� ������Ʈ�Դϴ�
    public Transform[] wavePos; //���̺� �����ܵ��� �ִ� TRANSFORM�Դϴ�. ��ġ�� �����ϱ� ���ؼ� ����ϴ�.
    public GameObject allWaveIcon; //���̺� ������ ������Ʈ���� ����ִ� �θ� ������Ʈ�Դϴ�

    public GameObject rewardTransition; //���� ī�带 �����ϱ� ���� Ʈ�������� ������ ������Ʈ�Դϴ�.
    public GameObject rewardUi; //ī�� ���� ��椷 ������Ʈ�Դϴ�.

    [Header("This is card pos object")]
    public GameObject firstCardPos;  // ù���� ī�� ���� �ִϸ��̼��Դϴ�.
    public GameObject secondCardPos; // �ι�° ī�� ���� �ִϸ��̼��Դϴ�.
    public GameObject thirdCardPos;  // ����° ī�� ���� �ִϸ��̼��Դϴ�.

    [Header("This is card object")]
    //TODO ī�� �̸��� NORMALCARD ---->>>> firstCard �� �ٲ�����մϴ�.
    public GameObject normalCard;           // ù��° ī��
    public GameObject uncommonCard;         // �ι�° ī��
    public GameObject rareCard;             // ����° ī�� 
    public List<GameObject> rewardCardPool;     //���������� ���� ī����� ����ִ� ������Ʈ�Դϴ�.
    private GameObject cardBlur;

    [Header("This is card Arrow")]
    public GameObject firstArrowBlur;  //ù���� ī�� ��ġ�� ȭ��ǥ ������Ʈ�� ����ִ� �θ� ������Ʈ�Դϴ�.
    public GameObject secondArrowBlur; // ���� ���������� �ι�° ī�� ��ġ �Դϴ�
    public GameObject thirdArrowBlur; // ù������ ���������� ����° ī�� ��ġ �Դϴ�

    public GameObject firstArrow;   // ù��° ī�� ��ġ�� ȭ��ǥ ������Ʈ�Դϴ�.
    public GameObject secondArrow;  // ��ġ�� �ٸ��� ����
    public GameObject thirdArrow;   // ��ġ�� �ٸ��� ����

    private bool isClear;  //���̺긦 Ŭ���� �ߴ��� 
    private int stageNum;  //���� ���̺� ��ȣ�Դϴ�.

    private bool isRewardUi; //���� ui �� �������� Ȯ���ϴ� bool���Դϴ�.
    private bool isRewardAnimation; //���� �ִϸ��̼� ��µƴ��� Ȯ���ϴ� bool��
    private bool isUpgradeComplete; //������ �÷��̾ �����ߴ��� Ȯ���ϴ� bool��

    private bool isWaveClear; //���̺� Ŭ���� UI �ִϸ��̼� �����ϱ����� ���Դϴ�.

    private int choiceNum;
    // Start is called before the first frame update
    void Start()
    {
        stageNum = 1; //���� �������� ��ȣ�Դϴ�.
        choiceNum = 1; // ī�� ���ý� ���� ���� �ѹ��Դϴ�.
        isRewardAnimation = false;  //���� �ִϸ��̼� false ������ ���Ӵϴ�.
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && stageNum < 10)
        {
            StartCoroutine(VictoryStage());
            StopCoroutine(VictoryStage());
        }
        Debug.LogFormat("���� ���������� = {0}", stageNum);
        if (isClear)
        {
            isClear = false;
            waveClearUi.SetActive(true);
            isWaveClear = true;
            waveClearUi.GetComponent<Animator>().SetBool("WaveUi", isWaveClear);
            ChangeWaveAni(); //�������� ���� �ִϸ��̼��� �ٸ��� ����մϴ�.
            stageNum +=1;

            //waveClearUi.GetComponent<Animator>().enabled = false;
            //TODO Ŭ���� ���������� ���� ī�� �̰� �س����մϴ�.
            if (stageNum == 2 || stageNum == 3 || stageNum == 4 || stageNum == 7 || stageNum == 9)
            {
                SelectActiveCard(ref normalCard);
                SelectActiveCard(ref rareCard);
                SelectActiveCard(ref uncommonCard);
            }
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
                    #region �ٸ� ī����� ���ڸ��� �ֵ��� �ϴ� ��ũ��Ʈ��
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
                    //ī�� �����ֱ�
                    uncommonCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(-9999f, -9999f);
                    rareCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(-9999f, -9999f);
                    StartCoroutine(SelectCardAndSetOff(normalCard)); //ī�� �÷��ְ� reward ui ���ִ� �ڷ�ƾ�Դϴ�.
                    StopCoroutine(SelectCardAndSetOff(normalCard));
                    
                  
                    rewardCardPool.Add(uncommonCard);
                    rewardCardPool.Add(rareCard);
                    isUpgradeComplete = true;
                }
            }
            else if (choiceNum == 1 && isRewardUi)
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
                    StartCoroutine(SelectCardAndSetOff(rareCard)); //ī�� �÷��ְ� reward ui ���ִ� �ڷ�ƾ�Դϴ�.
                    StopCoroutine(SelectCardAndSetOff(rareCard));
                   
                    rewardCardPool.Add(uncommonCard);// Add(uncommonCard);
                    rewardCardPool.Add(normalCard);
                    isUpgradeComplete = true;

                }
            }
            else if (choiceNum == 2 && isRewardUi)
            {
                firstArrowBlur.SetActive(false);
                secondArrowBlur.SetActive(false);
                thirdArrowBlur.SetActive(true);
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
                {
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
                    StartCoroutine(SelectCardAndSetOff(uncommonCard)); //ī�� �÷��ְ� reward ui ���ִ� �ڷ�ƾ�Դϴ�.
                    StopCoroutine(SelectCardAndSetOff(uncommonCard));

                    
                    rewardCardPool.Add(normalCard);
                    rewardCardPool.Add(rareCard);
                    normalCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(-9999f, -9999f);
                    rareCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(-9999f, -9999f);
                    //normalCard = null;
                    //rareCard = null; //��� ���� ������ ã������ �����س������ϴ�.
                    isUpgradeComplete = true;

                }
            }
            else { } //DO NOTHING
        }
        else { } //DO NOTHING

    }
    //TODO : �ϴ� �ٲ���� ���ݾ� �ٲ���� �� �� �����ϴ�.
    IEnumerator VictoryStage()
    {
        yield return new WaitForSeconds(1f);
        isClear = true;
        yield return new WaitForSeconds(1f);
        //waveClearUi.GetComponent<Animator>().enabled = false; //�̰� ���̺� Ŭ����� ������ �ִϸ��̼�
        //yield return new WaitForSeconds(0.5f);
        MoveAllIcon();   //TODO: ���������� �� �����ܱ��� �̵��ϴ°� �ϰ� �;��µ� �׳� �ִϸ��̼����� �ұ��մϴ�.
        yield return new WaitForSeconds(1.5f);
        if (stageNum == 2 || stageNum == 3 || stageNum == 4 || stageNum == 7 || stageNum == 9)
        {
            rewardTransition.SetActive(true);
            yield return new WaitForSeconds(1f);
            isRewardUi = true;
            choiceNum = 1;
            rewardUi.SetActive(true);
            StartCoroutine(OpenCard());
            StopCoroutine(OpenCard());
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }
        //waveClearUi.SetActive(false);   //�ִϸ��̼����� �ٲ�����ϱ⿡ ���� ���������մϴ�.
        isWaveClear = false; //waveClearUI �ִϸ��̼� �����մϴ�. �ᱹ �������� Ŭ���� �������� �������¸� �����ϱ�����
        waveClearUi.GetComponent<Animator>().SetBool("WaveUi", isWaveClear);
        //StartCoroutine(OpenCard());
        //StopCoroutine(OpenCard());

        //=======================================================

    }

    IEnumerator OpenCard()
    {
        isUpgradeComplete = false;  //���׷��̵� �Ϸ��ߴ��� Ȯ���ϴ� bool���Դϴ�
        secondArrow.SetActive(false); // ī�� ���ÿ��� 2��° ī�� ��ġ�� ȭ��ǥ�� ���ݴϴ�. �ֳ��ϸ� ó���� ī�� �ִϸ��̼� ���ö� ȥ�ڸ� ���׷��� �����ֱ� ������ �ϴ� ó���� ���ٷ� �߽��ϴ�.
        yield return new WaitForSeconds(0.5f);
        firstCardPos.SetActive(true); // ù��° ī�� �ִϸ��̼��� ���ݴϴ�.
        yield return new WaitForSeconds(0.3f);
        firstCardPos.SetActive(false);  // �Ͼ�� �ܻ����� �����ֱ⿡ ���ݴϴ�.
        MoveFirstCard();                // �ִϸ��̼� ��ġ�� ù��° ���� ī�� ������Ʈ�� �̵������ݴϴ�.
        yield return new WaitForSeconds(0.5f);
        thirdCardPos.SetActive(true);   //ù���� ī��� �����մϴ�.
        yield return new WaitForSeconds(0.3f);
        thirdCardPos.SetActive(false);
        MoveThirdCard();
        yield return new WaitForSeconds(0.5f);
        secondCardPos.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        secondCardPos.SetActive(false);
        MoveSecondCard();
        secondArrow.SetActive(true);
        yield return new WaitForSeconds(0.1f);  //��� ������ ������ 
        StartCoroutine(MoveArrow(secondArrow)); // �ι��� ȭ��ǥ�� ī�尡 ���õǾ� �ִ°� �����ֱ����� ���������� �������ݴϴ�.
        StartCoroutine(UpCardPosition(rareCard));
        StopCoroutine(MoveArrow(secondArrow));
        StopCoroutine(UpCardPosition(rareCard));
        isRewardAnimation = true;

    }
    void SelectMoveCard()  // {������ �ϱ� ���� ���� choiceNum �� ��������ִ� �Լ��Դϴ�.
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
    //} ������ �ϱ� ���� ���� choiceNum �� ��������ִ� �Լ� ��
    
    void MoveAllIcon()
    {
        if (stageNum == 5 || stageNum == 6 || stageNum == 10)
        {
            float distanceBossPos = wavePos[5].position.x - wavePos[4].position.x;
            allWaveIcon.transform.position = new Vector2(allWaveIcon.transform.position.x-distanceBossPos, allWaveIcon.transform.position.y);
        }
        else 
        {
            Debug.Log("���� ��?");
            float distancPos = wavePos[1].position.x - allWaveIcon.transform.position.x;
            allWaveIcon.transform.position = new Vector2(allWaveIcon.transform.position.x-distancPos, allWaveIcon.transform.position.y);
        }
        
    }
    void MoveFirstCard()
    {
        normalCard.transform.position = firstCardPos.transform.position; //���õ� ī�带 ù���� ī�� �ִϸ��̼� ��ġ�� ������ �ٲ��ݴϴ�
    }
    void MoveSecondCard()
    {
        rareCard.transform.position = secondCardPos.transform.position; //���õ� ī�带 �ι�° ī�� �ִϸ��̼� ��ġ�� ������ �ٲ��ݴϴ�.
    }
    void MoveThirdCard()
    {
        uncommonCard.transform.position = thirdCardPos.transform.position; // ���õ� ī�带 ����° ī�� �ִϸ��̼� ��ġ�� ������ �ٲ��ݴϴ�.
    }



    IEnumerator MoveArrow(GameObject arrow) //ȭ��ǥ �������ִ� �Լ��Դϴ�.
    {
        float max = -90; //�������� �ִ� ��ġ�Դϴ�.
        float height = arrow.GetComponent<RectTransform>().anchoredPosition.y;
        for (int i = 0; i < 20; i ++)
        {
            if (height<=max) yield break;

            height -= 4.5f;
            arrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(arrow.GetComponent<RectTransform>().anchoredPosition.x, height);
            yield return new WaitForSeconds(0.01f);
            //TODO �ö���� ����ó�� ������մϴ�.
            //TODO : ���� ���¸� �����صΰ� ��� �� ���� ���·� ���ƿ��Բ�)
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
            //TODO �������� ����ó�� ������մϴ�.
            //TODO : ���� ���¸� �����صΰ� ��� �� ���� ���·� ���ƿ��Բ�)
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
            if (height>=max) yield break;

            height += 32f;
            card.GetComponent<RectTransform>().anchoredPosition = new Vector2(card.GetComponent<RectTransform>().anchoredPosition.x, height);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.5f);
        card.SetActive(false); 
        rewardUi.SetActive(false);
        rewardTransition.GetComponent<Animator>().SetTrigger("OffCicle");
        yield return new WaitForSeconds(1f);
        rewardTransition.SetActive(false);
    }

    void ChangeWaveAni()
    {
        switch (stageNum)
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
    } //stageNum �� ���ؼ� �ִϸ��̼��� ����Ǵµ� �̰� ������ ���� �߸� �����ؼ� �������� 3�ϋ� 2clear �ִϸ��̼��� ������� ���������� �˴ϴ�.

    GameObject SelectActiveCard(ref GameObject card) //ref�� ���� ����ǰ� ������ϴ�. �ȱ׷��� �� ������ ���ؼ� �ٲ��� ���߾ 
    {
        int cardIndex = Random.Range(0, rewardCardPool.Count);
        card = rewardCardPool[cardIndex];
        rewardCardPool.RemoveAt(cardIndex);
        return card;
    }
}