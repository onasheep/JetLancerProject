using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OneSoundOn : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip unlockClip;
    public AudioClip confirmClip;
    public AudioClip cancelClip;
    public AudioClip moveClip;
    public bool isPress = false;

   
    
   
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && isPress != true)
        {

            audioSource.clip = unlockClip;
            
            audioSource.PlayOneShot(unlockClip);
            isPress = true;
        }
        else if(Input.GetKeyDown(KeyCode.W) && isPress && SceneManager.GetActiveScene().buildIndex  < 2)
        {
            audioSource.clip = moveClip;
            audioSource.PlayOneShot(moveClip);
        }
        else if(Input.GetKeyDown(KeyCode.S) && isPress && SceneManager.GetActiveScene().buildIndex  < 2)
        {
            audioSource.clip = moveClip;
            audioSource.PlayOneShot(moveClip);
        }
        else if (Input.GetKeyDown(KeyCode.Return )&& isPress)
        {
            audioSource.clip = confirmClip;
            audioSource.PlayOneShot(confirmClip);
            //transition.SetTrigger("Enter");
            
            
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isPress)
        {
            audioSource.clip = cancelClip;
            audioSource.PlayOneShot(cancelClip);
            //transition.SetTrigger("Esc");
          
        }
    }
   

    
}
