using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    //[CreateAssetMenu(menuName = "Scriptable/CardInfo", fileName = "Card Data")]
public class CardInfo : MonoBehaviour
{
    public enum State 
    {
        Ready,
        Apply,
        Plus
    }
    public State state { get; set; } //현재 카드 상태

    public GameObject playerObj;
    public CardData cardData;

    public bool isSpecial;
    public bool isEnter;
    // Start is called before the first frame update
    void Start()
    {
        //childBlur = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject; 
        playerObj = GFunc.GetRootObj(RDefine.PLAYER);
        isSpecial = cardData.isSpecialOn;
        isEnter = false;
        state = State.Ready;
        
    }

    //Update is called once per frame
    //void Update()
    //{
    //    if (isEnter)
    //    {
    //        if (state == State.Ready)
    //        {
    //            Debug.Log(playerObj.GetComponent<PlayerController>().health += cardData.healAmount);
    //            Debug.Log(playerObj.GetComponent<PlayerController>().bulletSpeed += cardData.damage);
    //            Debug.Log(playerObj.GetComponent<PlayerController>().bulletSpeed += cardData.bulletSpeed);
    //        }
    //        else if (state == State.Plus)
    //        {
    //            playerObj.GetComponent<PlayerController>().health += cardData.healAmount;
    //            playerObj.GetComponent<PlayerController>().bulletSpeed += cardData.damage;
    //            playerObj.GetComponent<PlayerController>().bulletSpeed += cardData.bulletSpeed;
    //        }
    //        else if (state == State.Apply && isSpecial == true)
    //        {
    //            //TODO 기술 업해주는걸 추가해야합니다.
    //            if (cardData.isShield)
    //            {

    //            }
    //            else if (cardData.isBomb)
    //            {

    //            }
    //            else if (cardData.isGuidedMissile)
    //            {

    //            }
    //        }
    //        else { }//DO NOTHING
    //    }
    //}

    public void ChooseCard()
    {
        if (state == State.Ready)
        {
            //Debug.Log(playerObj.GetComponent<PlayerController>().health += cardData.healAmount);
            //Debug.Log(playerObj.GetComponent<PlayerController>().bulletSpeed += cardData.damage);
            //Debug.Log(playerObj.GetComponent<PlayerController>().bulletSpeed += cardData.bulletSpeed);
        }
        else if (state == State.Plus)
        {
            // SJ_
            if (playerObj.GetComponent<PlayerController>().health < playerObj.GetComponent<PlayerController>().maxHealth)
            {
                playerObj.GetComponent<PlayerController>().health += cardData.healAmount;

            }       // if : 최대체력과 체력이 같으면 회복 되지 않도록
            playerObj.GetComponent<PlayerController>().damage += cardData.damage;
            playerObj.GetComponent<PlayerController>().bulletSpeed += cardData.bulletSpeed;
        }
        else if (state == State.Apply && isSpecial == true)
        {
            //TODO 기술 업해주는걸 추가해야합니다.
            if (cardData.isShield)
            {

            }
            else if (cardData.isBomb)
            {

            }
            else if (cardData.isGuidedMissile)
            {

            }
        }
        else { }//DO NOTHING
    }

}
