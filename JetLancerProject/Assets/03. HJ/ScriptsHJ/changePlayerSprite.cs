using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changePlayerSprite : MonoBehaviour
{
    public Sprite[] sprites; // ��������Ʈ �̹��� �迭

    private float minValue = 0f;
    private float maxValue = 0.5f;
    private SpriteRenderer mySprite; // ��������Ʈ ������ ������Ʈ

    private void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
        DetectSpriteError(mySprite);
    }

    private void Update()
    {
        // ���� �������� � ��������Ʈ �̹����� �������� ����
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
            Debug.LogError("��������Ʈ ����������");
        }
    }
}
