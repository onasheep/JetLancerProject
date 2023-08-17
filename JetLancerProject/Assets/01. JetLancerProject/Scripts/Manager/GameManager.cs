using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : GSingleton<GameManager>
{
    public PoolManager poolManager = default;
    
    // 
    private bool isGameOver = default;

    // 텍스트
    public GameObject scoreText = default;
    public GameObject timeText = default;
    public GameObject waveText = default;

    // 변수
    public int score = default;
    public int kills = default;
    public float time = default;
    public float accuracy = default;
    public float perfectEvasions = default;
    public float overChargedTime = default;
    public float rocketShotdown = default;
    public float unguidedAirShots = default;





    public override void Start()
    {
        isGameOver = false;
        //Init();
    }


    private void Init()
    {
        poolManager = GFunc.GetRootObj("PoolManager").GetComponent<PoolManager>();        
    }


}
