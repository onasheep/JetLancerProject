using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteWbottle : MonoBehaviour, IDeactive
{
    // �̸� ���� deleteTimer => existTime;
    private float existTime = 2f;
    // Start is called before the first frame update

    private void OnEnable()
    {
        //this.transform.DOScale = new Vector3(1, 1, 1);
        Invoke("Deactive", Random.Range(1f, existTime));
    }
    // Update is called once per frame

    public void Deactive()
    {
        // Interface ����
        this.gameObject.SetActive(false);

    }       // Deactive()

}
