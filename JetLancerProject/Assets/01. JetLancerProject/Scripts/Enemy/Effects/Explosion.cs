using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour, IDeactive
{    
    
        
    // TODO : 추후 animation time 끝날 떄 디엑티브 하고 싶음
    private float existTime = 0.8f;
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();        
    }
    private void OnEnable()
    {
        anim.SetTrigger("isExplosion");
        Invoke("Deactive", existTime);
    }
    public void Deactive()
    {
        // Interface 내용
        this.gameObject.SetActive(false);

    }       // Deactive()
}
