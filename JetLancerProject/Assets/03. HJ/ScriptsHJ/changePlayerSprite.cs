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
        if (gasGauge >= 0f && gasGauge < 4f)
        {
            mySprite.sprite = sprites[14];
        }
        else if (gasGauge >= 4f && gasGauge < 10f)
        {
            mySprite.sprite = sprites[13];
        }
        else if (gasGauge >= 10f && gasGauge < 16f)
        {
            mySprite.sprite = sprites[12];
        }
        else if (gasGauge >= 16f && gasGauge < 21f)
        {
            mySprite.sprite = sprites[11];
        }
        else if (gasGauge >= 21f && gasGauge < 28f)
        {
            mySprite.sprite = sprites[10];
        }
        else if (gasGauge >= 28f && gasGauge < 36f)
        {
            mySprite.sprite = sprites[9];
        }
        else if (gasGauge >= 36f && gasGauge < 44f)
        {
            mySprite.sprite = sprites[8];
        }
        else if (gasGauge >= 44f && gasGauge < 51f)
        {
            mySprite.sprite = sprites[7];
        }
        else if (gasGauge >= 51f && gasGauge < 58f)
        {
            mySprite.sprite = sprites[6];
        }
        else if (gasGauge >= 58f && gasGauge < 65f)
        {
            mySprite.sprite = sprites[5];
        }
        else if (gasGauge >= 65f && gasGauge < 72f)
        {
            mySprite.sprite = sprites[4];
        }
        else if (gasGauge >= 72f && gasGauge < 79f)
        {
            mySprite.sprite = sprites[3];
        }
        else if (gasGauge >= 79f && gasGauge < 86f)
        {
            mySprite.sprite = sprites[2];
        }
        else if (gasGauge >= 86f && gasGauge < 93f)
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
