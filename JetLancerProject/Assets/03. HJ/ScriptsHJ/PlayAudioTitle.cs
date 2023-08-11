using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioTitle : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip musicClip;
    
    public float delay = 4.0f;

    private float timer = 0.0f;
    private bool isPlay = false;

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
