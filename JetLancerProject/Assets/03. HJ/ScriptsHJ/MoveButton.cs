using UnityEngine.SceneManagement;
//using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class MoveButton : MonoBehaviour
{
    public GameObject escObj;
    private bool isPaused;

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
        // �Ʒ� �� �Լ��� ȣ���Ͽ� ���� S�� W Ű �Է��� ó���մϴ�.
        HandleButtonInput(KeyCode.S, 1); // S Ű�� ������ �� ��ư ������ 1 ������ŵ�ϴ�.
        HandleButtonInput(KeyCode.W, -1); // W Ű�� ������ �� ��ư ������ 1 ���ҽ�ŵ�ϴ�.

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))//&& SceneManager.GetActiveScene().buildIndex == 2)
        {
            PlayEnterKey();
        }
    }

    // ��ư �Է� ó�� �Լ�
    private void HandleButtonInput(KeyCode key, int change)
    {
        // key�� �ش��ϴ� Ű�� ��������, ����� ��ư ������ ��ȿ�� ���� ���� ���� ��� ����˴ϴ�.
        if (Input.GetKeyDown(key) && btnChoice + change >= 0 && btnChoice + change < buttonText.Length)
        {
            btnChoice += change; // ��ư ������ �����մϴ�.
            UpdateButtonPosition(); // ��ư ��������Ʈ ��ġ�� ������Ʈ�մϴ�.
            //PlaySoundEffect(soundEffect); // ȿ������ ����մϴ�.
        }
    }

    // ��ư ��������Ʈ ��ġ�� ������Ʈ�ϴ� �Լ�
    private void UpdateButtonPosition()
    {
        btnSprite.transform.position = Vector3.MoveTowards(btnSprite.transform.position, buttonText[btnChoice].transform.position, 1);
    }

    void PauseGame()
    {
        escObj.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    void ResumeGame()
    {
        escObj.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    // Enter Ű�� ó���ϴ� �Լ�
    private void PlayEnterKey()
    {
        if (btnChoice == 0)
        {
            //PlaySoundEffect(enterSoundEffect); // Enter ȿ������ ����մϴ�.
            ResumeGame();
            escObj.SetActive(false);
        }
        else if (btnChoice == 1)
        {
            ResumeGame();
            SceneManager.LoadScene("PlayScene");

        }
        else if (btnChoice == 2)
        {
            SceneManager.LoadScene("TitleScene");
            // Application.Quit(); // ���� ����
        }
    }

    // ȿ������ ����ϴ� �Լ�
    //private void PlaySoundEffect(AudioSource audioSource)
    //{
    //    if (audioSource != null)
    //        audioSource.Play();
    //}
}
