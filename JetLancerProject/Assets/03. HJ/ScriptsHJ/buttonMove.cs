using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class buttonMove : MonoBehaviour
{
    public Text[] buttonText; 
    public Transform btnSprite; // ��ư ��������Ʈ �Դϴ�
    private int btnChoice; //��ư�� ��ġ�ϴ� ���� �ǹ��մϴ�.
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
            //Todo ĳ���� ���þ� ������ �̰��� �־�����մϴ�.
            //SceneManager.LoadScene("CharacterSelect"); 
        }
        else if (Input.GetKeyDown(KeyCode.Return)  && btnChoice == 2)
        {
            UnityEditor.EditorApplication.isPlaying = false; //�÷��� ��� �����մϴ� Todo �ؿ� ��ü�ؾ��մϴ�.
            //	Application.Quit(); //���� ���� �־���մϴ�
        }
    }
}
