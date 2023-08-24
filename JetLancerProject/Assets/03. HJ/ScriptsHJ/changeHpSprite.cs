using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHpSprite : MonoBehaviour
{
    public Sprite[] sprites; // 스프라이트 이미지 배열

    private float playerHp;
    private SpriteRenderer mySprite; // 스프라이트 렌더러 컴포넌트

    // Start is called before the first frame update
    void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
        DetectSpriteError(mySprite);        
        playerHp = GameObject.Find("Player").GetComponent<PlayerController>().health;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeHPSprite();
    }

    void ChangeHPSprite()
    {
        playerHp = GameObject.Find("Player").GetComponent<PlayerController>().health;
        if (playerHp < 0.5f)
        {
            mySprite.sprite = sprites[0];
        }
        else if (playerHp >= 0.5f && playerHp < 1f)
        {
            mySprite.sprite = sprites[1];
        }
        else if (playerHp >= 1f && playerHp < 1.5f)
        {
            mySprite.sprite = sprites[2];
        }
        else if (playerHp >= 1.5f && playerHp < 2f)
        {
            mySprite.sprite = sprites[3];
        }
        else if (playerHp >= 2f && playerHp < 2.5f)
        {
            mySprite.sprite = sprites[4];
        }
        else if (playerHp >= 2.5f && playerHp < 3f)
        {
            mySprite.sprite = sprites[5];
        }
        else
        {
            mySprite.sprite = sprites[6];
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
