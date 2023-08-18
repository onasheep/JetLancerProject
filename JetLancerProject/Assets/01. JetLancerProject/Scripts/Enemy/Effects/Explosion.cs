using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour, IDeactive
{    
    
        
    // TODO : ���� animation time ���� �� ��Ƽ�� �ϰ� ����
    private float existTime = 1.0f;
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
        // Interface ����
        this.gameObject.SetActive(false);

    }       // Deactive()
}
