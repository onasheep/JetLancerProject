using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WaveManager : MonoBehaviour
{
    public List<WaveData> waves;
    
    private int waveCount;
    private int enem_1_Num;
    private int enem_2_Num;
    private int enem_3_Num;
    private int enem_4_Num;

    
    public List<GameObject> enemyList;

    public int curWave = 1;
    [SerializeField]
    private int remainToSpawn = default;
    private float minSpawnTime = 0.5f;
    private float maxSpawnTime = 5f;

    private Vector3 spawnOffset;

    // 형준이 Intro 시간 4.3f
    // 나중에 수정 필요
    private float introTime = 6.0f;

    private bool isSpawn = default;
    public bool isClear = default;
    
    private PoolManager pool;

    private void Awake()
    {
        isClear = false;
        isSpawn = false;
    }
    void Start()
    {
        pool = GameManager.Instance.poolManager;
        enemyList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // 인트로 타임 이후부터 스폰 시작!
        if (introTime > Time.time) { return; }

        if (isSpawn == false && isClear == false)
        {
            isSpawn = true;
            SetWave(curWave);

            curWave += 1;
            StartCoroutine(SpawnEnemy());
        }

        if (CheckActive() == false)
        {

            enemyList.Clear();            
            isClear = true;
            isSpawn = false;
        }
        else
        {
            isSpawn = true;
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
            if (enemyList[i].activeSelf == true)
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

        Vector3 spawnPos = GameManager.Instance.player.transform.position + new Vector3(2 * Camera.main.orthographicSize * Camera.main.aspect, 2 * Camera.main.orthographicSize);

        enemyList.Add(pool.SpawnFromPool(name_, spawnPos/* this.transform.position*/, Quaternion.identity));
    }       // SpawnWaveEnemy()


}
