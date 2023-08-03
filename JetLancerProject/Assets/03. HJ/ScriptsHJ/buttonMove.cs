using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class buttonMove : MonoBehaviour
{
    public Text[] buttonText; 
    public Transform btnSprite; // 버튼 스프라이트 입니다
    private int btnChoice; //버튼이 위치하는 값을 의미합니다.
    void Start()
    {
        btnChoice = 0;
        Vector3 destination = buttonText[btnChoice].transform.position;
        btnSprite.transform.position = Vector3.MoveTowards(btnSprite.transform.position, destination, 1);
    }


    void Update()
    {
        Vector3 destination = buttonText[btnChoice].transform.position;

        if (Input.GetKeyDown(KeyCode.S)  && btnChoice >= 0 && btnChoice < 2)
        {
            btnChoice += 1;
            destination = buttonText[btnChoice].transform.position;
            btnSprite.transform.position = Vector3.MoveTowards(btnSprite.transform.position, destination, 1);
            //Debug.Log(btnChoice);
        }
        if (Input.GetKeyDown(KeyCode.W)  && btnChoice <= 2&& btnChoice >0)
        {
            btnChoice -= 1;
            destination = buttonText[btnChoice].transform.position;
            btnSprite.transform.position = Vector3.MoveTowards(btnSprite.transform.position, destination, 1);
            //Debug.Log(btnChoice);
        }
        if (Input.GetKeyDown(KeyCode.Return)  && btnChoice == 0)
        {
            //Todo 캐릭터 선택씬 생성시 이곳에 넣어놔야합니다.
            //SceneManager.LoadScene("CharacterSelect"); 
        }
        else if (Input.GetKeyDown(KeyCode.Return)  && btnChoice == 2)
        {
            UnityEditor.EditorApplication.isPlaying = false; //플레이 모드 종료합니다 Todo 밑에 교체해야합니다.
            //	Application.Quit(); //어플 종료 넣어야합니다
        }
    }
}
