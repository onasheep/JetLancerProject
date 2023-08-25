using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAudioTitle : MonoBehaviour
{

    //GameObject backMusic;
    

    public AudioSource audioSource;
    public AudioClip musicClip;
   
    
    public float delay = 4.0f;

    private float timer = 0.0f;
    private bool isPlay = false;

    private void Awake()
    {
        
        var backMusic = FindObjectsOfType<PlayAudioTitle>();
            //GameObject.Find("introCanvas");
        //audioSource = backMusic.GetComponent<AudioSource>();
        if (backMusic.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        //if(isPlay)
        //{
        //    return;
        //}
        //else 
        //{
        //    audioSource.Play(); //�Ҹ����
        //    DontDestroyOnLoad(backMusic); //��������ʰԲ�

        //}

    }

    private void Update()
    {
        // Ÿ�̸� ������Ʈ
        timer += Time.deltaTime;

        // ������ ���� �ð��� ������ ���� ������ ������� �ʾҴٸ�
        if (timer >= delay && !isPlay)
        {
            isPlay = true; // ������ �� ���� ����ǵ��� ����
            PlayMusic(); // ���� ��� �Լ� ȣ��
        }
        if (Input.anyKeyDown && isPlay != true)
        {

            isPlay = true;
            audioSource.clip = musicClip;
            audioSource.time = 10.2f;
            audioSource.Play();
       
        }
        
        if (SceneManager.GetActiveScene().buildIndex  >= 2) //Ȱ��ȭ���� �� �ѹ��� �÷��̾��̸�
        {
            //gameObject.SetActive(false);
            audioSource.Stop();  //���� ��� ����
        }
    }

    private void PlayMusic()
    {
        if (audioSource != null && musicClip != null)
        {
            audioSource.clip = musicClip;
            audioSource.Play();
        }
    }
}
