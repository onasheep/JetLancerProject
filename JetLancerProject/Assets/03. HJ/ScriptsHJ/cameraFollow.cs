using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public playerController playercontroller;
    // Start is called before the first frame update
    void Start()
    {
        
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
}
