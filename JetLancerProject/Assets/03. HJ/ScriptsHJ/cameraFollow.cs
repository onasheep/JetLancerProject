using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public playerController playercontroller;
    //TODO 각 콜라이더 위치 받아와서 카메라 이동 범위 제한해줘야 합니다.
    //public float offsetX = 10;
    //public float offsetY = 10;
    //public float offsetZ = -10;
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

        // 카메라와 플레이어의 z 간격이 같아져서 문제가 생겼었습니다.
        //transform.position = playerController.transform.position;   
        transform.position = new Vector3(playercontroller.transform.position.x, playercontroller.transform.position.y, -10f);
    }
    //private void LateUpdate()
    //{
    //    transform.position = new Vector3(
    //        Mathf.Clamp(Camera.main.transform.position.x, -offsetX, offsetX),
    //        Mathf.Clamp(Camera.main.transform.position.y, -offsetY, offsetY),
    //        Mathf.Clamp(Camera.main.transform.position.z, offsetZ, offsetZ));
    //}
}
