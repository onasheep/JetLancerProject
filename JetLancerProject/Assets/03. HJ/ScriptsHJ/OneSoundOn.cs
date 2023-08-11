using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSoundOn : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip musicClip;

    private bool isPress = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && isPress != true)
        {

            isPress = true;
            audioSource.clip = musicClip;
            
            audioSource.PlayOneShot(musicClip);

        }
    }
}
