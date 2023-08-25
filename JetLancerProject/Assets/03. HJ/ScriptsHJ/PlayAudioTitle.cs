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
        //    audioSource.Play(); //소리재생
        //    DontDestroyOnLoad(backMusic); //사라지지않게끔

        //}

    }

    private void Update()
    {
        // 타이머 업데이트
        timer += Time.deltaTime;

        // 지정된 지연 시간이 지나고 아직 음악이 재생되지 않았다면
        if (timer >= delay && !isPlay)
        {
            isPlay = true; // 음악이 한 번만 재생되도록 설정
            PlayMusic(); // 음악 재생 함수 호출
        }
        if (Input.anyKeyDown && isPlay != true)
        {

            isPlay = true;
            audioSource.clip = musicClip;
            audioSource.time = 10.2f;
            audioSource.Play();
       
        }
        
        if (SceneManager.GetActiveScene().buildIndex  >= 2) //활성화중인 씬 넘버거 플레이씬이면
        {
            //gameObject.SetActive(false);
            audioSource.Stop();  //음악 재생 중지
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
