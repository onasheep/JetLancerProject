using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    public SpriteRenderer[] spritePos;
    public Transform btnSprite;
    private int selectNum;
    // Start is called before the first frame update
    void Start()
    {
        //Todo 나중에 폴리싱 작업때 타이틀 버튼 이동하는 것처럼 구현 해놔야함
        selectNum = 0;
        if( btnSprite != null )
        {
            btnSprite.transform.position =new Vector2(spritePos[0].transform.position.x-1, spritePos[0].transform.position.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && selectNum >= 0) 
        {
            SceneManager.LoadScene("PlayScene");
        }
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Invoke("LoadTitle", 0.7f);
        }


    }

    void LoadTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
