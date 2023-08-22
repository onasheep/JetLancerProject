using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public PlayerController playercontroller;
    //TODO 각 콜라이더 위치 받아와서 카메라 이동 범위 제한해줘야 합니다.
    

    private float minOffsetX;
    private float minOffsetY;  
    private float maxOffsetX;
    private float maxOffsetY;


  
    private float offsetZ = -10;
    // Start is called before the first frame update


    // SJ_
    // 플레이어를 찾음
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
        // 카메라와 플레이어의 z 간격이 같아져서 문제가 생겼었습니다.
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
