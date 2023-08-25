using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ButtonMove : MonoBehaviour
{
    public TMP_Text[] buttonText;
    public Transform btnSprite;
    private int btnChoice;
    //public AudioSource soundEffect;
    //public AudioSource enterSoundEffect;

    private void Start()
    {
        btnChoice = 0;

        UpdateButtonPosition();
    }

    private void Update()
    {
        // 아래 두 함수를 호출하여 각각 S와 W 키 입력을 처리합니다.
        HandleButtonInput(KeyCode.S, 1); // S 키를 눌렀을 때 버튼 선택을 1 증가시킵니다.
        HandleButtonInput(KeyCode.W, -1); // W 키를 눌렀을 때 버튼 선택을 1 감소시킵니다.

        // Enter 키 입력을 처리합니다.
        if (Input.GetKeyDown(KeyCode.Return))
        {
            EnterKey();
            //Invoke("EnterKey", 0.7f);
        }
        //EnterKey();
    }



    // 버튼 입력 처리 함수
    private void HandleButtonInput(KeyCode key, int change)
    {
        // key에 해당하는 키가 눌렸으며, 변경된 버튼 선택이 유효한 범위 내에 있을 경우 실행됩니다.
        if (Input.GetKeyDown(key) && btnChoice + change >= 0 && btnChoice + change < buttonText.Length)
        {
            btnChoice += change; // 버튼 선택을 변경합니다.
            UpdateButtonPosition(); // 버튼 스프라이트 위치를 업데이트합니다.
            //PlaySoundEffect(soundEffect); // 효과음을 재생합니다.
        }
    }

    // 버튼 스프라이트 위치를 업데이트하는 함수
    private void UpdateButtonPosition()
    {
        btnSprite.transform.position = Vector3.MoveTowards(btnSprite.transform.position, buttonText[btnChoice].transform.position, 1);
    }

    // Enter 키를 처리하는 함수
    private void EnterKey()
    {
        if (btnChoice == 0)
        {
            //PlaySoundEffect(enterSoundEffect); // Enter 효과음을 재생합니다.
            SceneManager.LoadScene("CharacterSelect");
        }
        else if (btnChoice == 2)
        {            
             Application.Quit(); // 어플 종료

        }


    }

    // 효과음을 재생하는 함수
    //private void PlaySoundEffect(AudioSource audioSource)
    //{
    //    if (audioSource != null)
    //        audioSource.Play();
    //}
}
