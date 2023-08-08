using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonMove : MonoBehaviour
{
    public Text[] buttonText;
    public Transform btnSprite;
    private int btnChoice;
    public AudioSource soundEffect;
    public AudioSource enterSoundEffect;

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

        // Enter Ű �Է��� ó���մϴ�.
        if (Input.GetKeyDown(KeyCode.Return))
            EnterKey();
    }

    // ��ư �Է� ó�� �Լ�
    private void HandleButtonInput(KeyCode key, int change)
    {
        // key�� �ش��ϴ� Ű�� ��������, ����� ��ư ������ ��ȿ�� ���� ���� ���� ��� ����˴ϴ�.
        if (Input.GetKeyDown(key) && btnChoice + change >= 0 && btnChoice + change < buttonText.Length)
        {
            btnChoice += change; // ��ư ������ �����մϴ�.
            UpdateButtonPosition(); // ��ư ��������Ʈ ��ġ�� ������Ʈ�մϴ�.
            PlaySoundEffect(soundEffect); // ȿ������ ����մϴ�.
        }
    }

    // ��ư ��������Ʈ ��ġ�� ������Ʈ�ϴ� �Լ�
    private void UpdateButtonPosition()
    {
        btnSprite.transform.position = Vector3.MoveTowards(btnSprite.transform.position, buttonText[btnChoice].transform.position, 1);
    }

    // Enter Ű�� ó���ϴ� �Լ�
    private void EnterKey()
    {
        if (btnChoice == 0)
        {
            PlaySoundEffect(enterSoundEffect); // Enter ȿ������ ����մϴ�.
           
            SceneManager.LoadScene("CharacterSelect");
        }
        else if (btnChoice == 2)
        {
            UnityEditor.EditorApplication.isPlaying = false; // �÷��� ��� ����
            // Application.Quit(); // ���� ����
        }
    }

    // ȿ������ ����ϴ� �Լ�
    private void PlaySoundEffect(AudioSource audioSource)
    {
        if (audioSource != null)
            audioSource.Play();
    }
}
