using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : GSingleton<GameManager>
{
    public PoolManager poolManager = default;
    public WaveManager waveManager = default;
    public PlayerController player = default;

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





    public override void Awake()
    {
        isGameOver = false;
        Init();
    }


    private void Init()
    {
        if (GFunc.GetRootObj("PoolManager").IsValid() == false) { return; }
        poolManager = GFunc.GetRootObj("PoolManager").GetComponent<PoolManager>();
        if (GFunc.GetRootObj("WaveManager").IsValid() == false) { return; }
        waveManager = GFunc.GetRootObj("WaveManager").GetComponent<WaveManager>();
        if (GFunc.GetRootObj("Player").IsValid() == false) { return; }
        player = GFunc.GetRootObj("Player").GetComponent<PlayerController>();

    }


}
