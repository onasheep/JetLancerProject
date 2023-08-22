using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneStartSound : MonoBehaviour
{
    public AudioClip startSound;
    public AudioClip threeCountSound;
    public AudioClip oneTwoCountSound;
    public AudioClip engaugeSound;
    public AudioClip backGroundSound;

    private AudioSource myAudio;

    void Start()
    {
        myAudio = GetComponent<AudioSource>();
        StartCoroutine(PlaySound());
        StopCoroutine(PlaySound());

    }


    IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(0.3f);
        myAudio.clip = startSound;
        myAudio.Play();
        yield return new WaitForSeconds(1f);
        myAudio.Stop();
        myAudio.PlayOneShot(threeCountSound);
        yield return new WaitForSeconds(1f);
        myAudio.PlayOneShot(oneTwoCountSound);
        yield return new WaitForSeconds(1f);
        myAudio.PlayOneShot(oneTwoCountSound);
        yield return new WaitForSeconds(1f);
        myAudio.PlayOneShot(engaugeSound);
        yield return new WaitForSeconds(1f);
        myAudio.clip = backGroundSound;
        myAudio.Play();
        myAudio.volume = 0.4f;
    }

}
