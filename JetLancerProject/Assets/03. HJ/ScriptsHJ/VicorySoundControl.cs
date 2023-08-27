using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VicorySoundControl : MonoBehaviour
{
    public AudioClip victorySound;

    public AudioSource myAudio = default;

    // Start is called before the first frame update
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        StartCoroutine(Delay());
        StopCoroutine(Delay());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2.5f);
        myAudio.clip = victorySound;
        myAudio.PlayOneShot(victorySound);
    }
}
