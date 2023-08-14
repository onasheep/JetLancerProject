using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteWbottle : MonoBehaviour, IDeactive
{
    // 이름 변경 deleteTimer => existTime;
    private float existTime = 3f;
    // Start is called before the first frame update

    private void OnEnable()
    {        
        Invoke("Deactive", existTime);
    }
    // Update is called once per frame

    public void Deactive()
    {
        // Interface 내용
        this.gameObject.SetActive(false);

    }       // Deactive()

}
