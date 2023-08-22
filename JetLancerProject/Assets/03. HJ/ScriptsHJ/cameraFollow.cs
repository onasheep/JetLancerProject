using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public PlayerController playercontroller;
    //TODO �� �ݶ��̴� ��ġ �޾ƿͼ� ī�޶� �̵� ���� ��������� �մϴ�.
    

    private float minOffsetX;
    private float minOffsetY;  
    private float maxOffsetX;
    private float maxOffsetY;


  
    private float offsetZ = -10;
    // Start is called before the first frame update


    // SJ_
    // �÷��̾ ã��
    void Start()
    {
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
}
