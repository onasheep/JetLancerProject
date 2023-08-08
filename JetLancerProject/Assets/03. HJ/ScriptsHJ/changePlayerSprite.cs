using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changePlayerSprite : MonoBehaviour
{
    public Sprite[] sprites; // 스프라이트 이미지 배열

    private float minValue = 0f;
    private float maxValue = 0.5f;
    private SpriteRenderer mySprite; // 스프라이트 렌더러 컴포넌트

    private void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
        DetectSpriteError(mySprite);
    }

    private void Update()
    {
        // 값을 기준으로 어떤 스프라이트 이미지를 보여줄지 결정
        if (transform.rotation.z > minValue && transform.rotation.z < maxValue)
        {
            mySprite.sprite = sprites[0];
        }
        else //if (transform.rotation.z > minValue && transform.rotation.z < maxValue)
        {
            mySprite.sprite = sprites[1]; 
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
