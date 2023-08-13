using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerSprite : MonoBehaviour
{
    public Sprite[] sprites; // 스프라이트 이미지 배열

    private float gasGauge;


    private SpriteRenderer mySprite; // 스프라이트 렌더러 컴포넌트

    private void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
        DetectSpriteError(mySprite);

        gasGauge = GameObject.Find("Player").GetComponent<PlayerController>().gas;

    }

    private void Update()
    {
        //Debug.Log(gasGauge);
        ChangeBoosterGauge();


    }

    void ChangeBoosterGauge()
    {
        gasGauge = GameObject.Find("Player").GetComponent<PlayerController>().gas;


        // 값을 기준으로 어떤 스프라이트 이미지를 보여줄지 결정
        if (gasGauge >= 0f && gasGauge < 7f)
        {
            mySprite.sprite = sprites[14];
        }
        else if (gasGauge >= 7f && gasGauge < 14f)
        {
            mySprite.sprite = sprites[13];
        }
        else if (gasGauge >= 14f && gasGauge < 21f)
        {
            mySprite.sprite = sprites[12];
        }
        else if (gasGauge >= 21f && gasGauge < 28f)
        {
            mySprite.sprite = sprites[11];
        }
        else if (gasGauge >= 28f && gasGauge < 35f)
        {
            mySprite.sprite = sprites[10];
        }
        else if (gasGauge >= 35f && gasGauge < 42f)
        {
            mySprite.sprite = sprites[9];
        }
        else if (gasGauge >= 42f && gasGauge < 49f)
        {
            mySprite.sprite = sprites[8];
        }
        else if (gasGauge >= 49f && gasGauge < 56f)
        {
            mySprite.sprite = sprites[7];
        }
        else if (gasGauge >= 56f && gasGauge < 63f)
        {
            mySprite.sprite = sprites[6];
        }
        else if (gasGauge >= 63f && gasGauge < 70f)
        {
            mySprite.sprite = sprites[5];
        }
        else if (gasGauge >= 70f && gasGauge < 77f)
        {
            mySprite.sprite = sprites[4];
        }
        else if (gasGauge >= 77f && gasGauge < 84f)
        {
            mySprite.sprite = sprites[3];
        }
        else if (gasGauge >= 84f && gasGauge < 91f)
        {
            mySprite.sprite = sprites[2];
        }
        else if (gasGauge >= 91f && gasGauge < 98f)
        {
            mySprite.sprite = sprites[1];
        }
        else
        {
            mySprite.sprite = sprites[0];

        }
    }

    void DetectSpriteError(SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("스프라이트 못가져왔음");
        }
    }
}
