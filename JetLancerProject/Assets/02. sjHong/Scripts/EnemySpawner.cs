using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject basicEnemy;
    public GameObject hommingEnemy;
    public GameObject bigEye;

    private Transform target;

    private float spawnTime = default;
    private float spwanMaxTime = 2;
    private float spwanMinTime = 1;

    private int spawnCount = 5;

    private int basicNum = 1;
    private int hommingNum = 1;
    private bool isWaveClear = false;

    private Queue waveQue = default;


    void Start()
    {
        

        target = FindObjectOfType<PlayerController>().transform;
        waveQue = new Queue();
    }

    // Update is called once per frame
    void Update()
    {
        if(isWaveClear== false)
        {
            EnemySpawn();

        }
    }

    private void EnemySpawn()
    {


        // TODO : 생성위치 관련해서 플레이어 위치 또는 카메라 범위 밖에서 생성되도록 
        spawnTime += Time.deltaTime;
        if (spawnTime > spwanMinTime)
        {
            float randPosY = Random.Range(-15f, 15f);
            float randPosX = Random.Range(15f, 30f);
            Vector3 randPos = new Vector3(randPosX, randPosY);
            spawnTime = 0f;
            if(basicNum > 0)
            {
                basicNum -= 1;
                waveQue.Enqueue(Instantiate(basicEnemy, target.transform.position + randPos, Quaternion.identity));
            }
            else
            {
                if(hommingNum > 0)
                {
                    hommingNum -= 1;
                    waveQue.Enqueue(Instantiate(hommingEnemy,target.transform.position + randPos,Quaternion.identity));
                
                    
                }
                else
                {
                    Instantiate(bigEye, target.transform.position, Quaternion.identity);
                    isWaveClear = true;

                }


            }


        }
    }
}
