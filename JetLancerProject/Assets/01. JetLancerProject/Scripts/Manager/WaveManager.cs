using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<WaveData> waves;
    
    private int waveCount;
    private int enem_1_Num;
    private int enem_2_Num;
    private int enem_3_Num;
    private int enem_4_Num;

    [SerializeField]
    public List<GameObject> enemyList;
    [SerializeField]
    private int curWave = 1;
    [SerializeField]
    private int remainToSpawn = default;
    private float minSpawnTime = 0.5f;
    private float maxSpawnTime = 5f;

    private bool isSpawn = default;
    [SerializeField]
    private bool isClear = default;
    
    private PoolManager pool;

    void Start()
    {
        pool = GameManager.Instance.poolManager;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (isSpawn == false && isClear == false)
        {
            isSpawn = true;
            SetWave(curWave);

            StartCoroutine(SpawnEnemy());
        }


        if (CheckActive() == false)
        {
            enemyList.Clear();            
            isClear = false;
            curWave += 1;
        }


    }
    void SetWave(int waveIdx)
    {
        waveCount = waves[waveIdx - 1].waveCount;
        enem_1_Num = waves[waveIdx - 1].enem_1_Num;
        enem_2_Num = waves[waveIdx - 1].enem_2_Num;
        enem_3_Num = waves[waveIdx - 1].enem_3_Num;
        enem_4_Num = waves[waveIdx - 1].enem_4_Num;

        remainToSpawn = enem_1_Num + enem_2_Num + enem_3_Num + enem_4_Num;
    }

    bool CheckActive()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i].gameObject.activeSelf == true)
            {
                return true;
            }
        }
        return false;
    }
    IEnumerator SpawnEnemy()
    {
        // 구조 이상 변경 필요
        float spawnTime = default;
        while (isSpawn == true)
        {
            if (remainToSpawn <= 0)
            {
                Debug.LogFormat("before : {0}", isClear);
                isClear = true;
                Debug.LogFormat("after : {0}", isClear);
                isSpawn = false;
                yield break;
            }

            float randNum = Random.Range(1, 5);
            spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
            switch (randNum)
            {
                case 1:
                    SpawnWaveEnemy(RDefine.ENEMY_JET_01);
                    break;
                case 2:
                    SpawnWaveEnemy(RDefine.ENEMY_JET_02);
                    break;
                case 3:
                    SpawnWaveEnemy(RDefine.ENEMY_JET_03);
                    break;
                case 4:
                    SpawnWaveEnemy(RDefine.ENEMY_JET_04);
                    break;
            }
            remainToSpawn--;


            yield return new WaitForSeconds(spawnTime);
        }
    }

    void SpawnWaveEnemy(string name_)
    {
        enemyList.Add(pool.SpawnFromPool(name_, this.transform.position, Quaternion.identity));
    }       // SpawnWaveEnemy()


}
