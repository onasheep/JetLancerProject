using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private PlayerController playercontroller;
    //TODO 각 콜라이더 위치 받아와서 카메라 이동 범위 제한해줘야 합니다.
    

    private float minOffsetX;
    private float minOffsetY;  
    private float maxOffsetX;
    private float maxOffsetY;

    // SJ_
    // 카메라 흔들림에 사용될 변수
    private float time = default;
    private float shakeTimer = 0.5f;
    private float randPos = default;
    private float posMin = 0.2f;
    private float posMax = 0.5f;
    //

    private float offsetZ = -10;
    // Start is called before the first frame update


    // SJ_
    // 플레이어를 찾음
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

    // SJ_ 
    // PlayerController의 OnDamage() 시 카메라를 흔들어줌
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
