using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private PlayerController playercontroller;
    //TODO �� �ݶ��̴� ��ġ �޾ƿͼ� ī�޶� �̵� ���� ��������� �մϴ�.
    

    private float minOffsetX;
    private float minOffsetY;  
    private float maxOffsetX;
    private float maxOffsetY;

    // SJ_
    // ī�޶� ��鸲�� ���� ����
    private float time = default;
    private float shakeTimer = 0.5f;
    private float randPos = default;
    private float posMin = 0.2f;
    private float posMax = 0.5f;
    //

    private float offsetZ = -10;
    // Start is called before the first frame update


    // SJ_
    // �÷��̾ ã��
    void Start()
    {
        playercontroller = GFunc.GetRootObj("Player").GetComponent<PlayerController>(); 

        minOffsetX = -1.5f;
        maxOffsetX = 140f;
        minOffsetY = -16f;
        maxOffsetY = 21f;
    }

    //private void FixedUpdate()
    //{
    //    transform.position = new Vector3(playercontroller.transform.position.x, playercontroller.transform.position.y, -10f);

    //}
    // Update is called once per frame
    void Update()
    {
        // ī�޶�� �÷��̾��� z ������ �������� ������ ��������ϴ�.
        //transform.position = playerController.transform.position;   
        transform.position = new Vector3(playercontroller.transform.position.x, playercontroller.transform.position.y, -10f);
    }
    private void LateUpdate()
    {
        transform.position = new Vector3(
            Mathf.Clamp(Camera.main.transform.position.x, minOffsetX, maxOffsetX),
            Mathf.Clamp(Camera.main.transform.position.y, minOffsetY, maxOffsetY),
            Mathf.Clamp(Camera.main.transform.position.z, offsetZ, offsetZ));
    }

    // SJ_ 
    // PlayerController�� OnDamage() �� ī�޶� ������
    public IEnumerator ShakeCamera()
    {
        
        while(time < shakeTimer)
        {
            time += Time.deltaTime;
            randPos = Random.Range(posMin, posMax);
            this.transform.position += new Vector3(randPos, randPos);
            yield return null;            
        }
        time = 0f;
    }       // ShakeCamera()
}
