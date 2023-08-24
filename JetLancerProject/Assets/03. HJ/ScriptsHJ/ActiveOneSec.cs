using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ActiveOneSec : MonoBehaviour
{
    GameObject myObj;
    Image myImg;

    public GameObject setOne, setTwo;
    public List<GameObject> selectTexts = default;
    //private float alphaNum;

    // GameObject.Find("Player").GetComponent<playerController>().health;

    // Start is called before the first frame update
    void Start()
    {
        myObj = GameObject.Find("whiteImg");
        myImg = myObj.GetComponent<Image>();
        StartCoroutine(DownAlphaColor());
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
        GameManager.Instance.isActiveText = true;
        for (int i = 0; i < selectTexts.Count; i++)
        {
            selectTexts[i].SetActive(false);
        }
    }

  
    IEnumerator DownAlphaColor()
    {
        
        yield return new WaitForSeconds(0.1f);
        if (myObj.activeSelf)
        {
            Color color = myImg.color;
            for(float i = 1.0f; i >= 0.0f;i-=0.01f)
            {
                color.a = i;
                myImg.color = color;
            }
        }

        //
    }

}
