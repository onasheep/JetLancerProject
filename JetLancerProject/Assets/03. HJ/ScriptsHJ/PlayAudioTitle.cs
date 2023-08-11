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
        // Ÿ�̸� ������Ʈ
        timer += Time.deltaTime;

        // ������ ���� �ð��� ������ ���� ������ ������� �ʾҴٸ�
        if (timer >= delay && !isPlay)
        {
            isPlay = true; // ������ �� ���� ����ǵ��� ����
            PlayMusic(); // ���� ��� �Լ� ȣ��
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
