using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WaveManager : MonoBehaviour
{
    public List<WaveData> waves;
    
    private int waveCount;
    private int enemNum;
    private int bossNum;

    
    public List<GameObject> enemyList;

    public int curWave = 1;
    [SerializeField]
    private int remainToSpawn = default;
    [SerializeField]
    private int remainToSpawn_Boss = default;
    private float minSpawnTime = 0.5f;
    private float maxSpawnTime = 5f;


    // 형준이 Intro 시간 4.3f
    // 나중에 수정 필요
    private float introTime = 6.0f;

    public bool isClear = default;
    private bool isSpawn = default;
    private bool isCheckable = default;

    private PoolManager pool;

    private void Awake()
    {
        isClear = false;
        isSpawn = false;
        isCheckable = false;
    }
    void Start()
    {
        pool = GameManager.Instance.poolManager;
        enemyList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

        Debug.LogFormat("{0}", 2 * Camera.main.orthographicSize * Camera.main.aspect);
        Debug.LogFormat("{0}", 2 * Camera.main.orthographicSize);

        // 인트로 타임 이후부터 스폰 시작!
        if (introTime > Time.time) { return; }

        if (isSpawn == false && isClear == false)
        {
            isSpawn = true;
            SetWave(curWave);

            StartCoroutine(SpawnEnemy());
            curWave += 1;
        }

        if (isCheckable == true && CheckActive() == false)
        {
            
            enemyList.Clear();
            isCheckable = false;
            isClear = true;
            isSpawn = false;
        }

    }
    void SetWave(int waveIdx)
    {

        waveCount = waves[waveIdx].waveCount;
        enemNum = waves[waveIdx].enemNum;
        bossNum = waves[waveIdx].bossNum;

        remainToSpawn = enemNum;
        remainToSpawn_Boss = bossNum;
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
        isSpawn = false;
        return false;
    }

    IEnumerator SpawnEnemy()
    {
        // 구조 이상 변경 필요
        float spawnTime = default;
        while (isSpawn == true)
        {
           

            if (remainToSpawn <= 0 && remainToSpawn_Boss <= 0)
            {
                isCheckable = true;
                yield break;
            }

            if (remainToSpawn_Boss > 0)
            {
                SpawnWaveEnemy(RDefine.BOSS_EYE);
                remainToSpawn_Boss -= 1;
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
        float randNum = Random.Range(0, 2);

        int mult = 1;
        switch (randNum)
        {

            case 0:
                mult = 1;
                break;
            case 1:
                mult = -1;
                break;
        }
        Vector3 spawnPos = 
            GameManager.Instance.player.transform.position +
            new Vector3(2 * Camera.main.orthographicSize * Camera.main.aspect * mult, 2 * Camera.main.orthographicSize * mult) ;
        
        enemyList.Add(pool.SpawnFromPool(name_, spawnPos, Quaternion.identity));

        
    }       // SpawnWaveEnemy()


}
