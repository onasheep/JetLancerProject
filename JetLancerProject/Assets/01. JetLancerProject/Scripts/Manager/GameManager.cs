using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : GSingleton<GameManager>
{


    public PoolManager poolManager = default;
    public WaveManager waveManager = default;
    public PlayerController player = default;

    //============================================
    //���� ����
    public GameObject defeatUiObj = default;
    //============================================


    // bool ����
    //public bool isGameover { get; private set; } 
    public bool isGameOver = default;

    // �ؽ�Ʈ
    public TMP_Text scoreText = default;
    public TMP_Text bestScoreText = default;
    public TMP_Text timeText = default;
    public TMP_Text bestTimeText = default;
    public TMP_Text waveText = default;
    public TMP_Text bestWaveText = default;
    public TMP_Text killsText = default;
    public TMP_Text expTextNum = default;
    public TMP_Text nextExpTextNum = default;
    public TMP_Text[] tmpTextObjects = new TMP_Text[9];

    // ����
    public int score = default;
    public int bestScore = default;
    public int wave = default;
    public int bestWave = default;  
    public float time = default;
    public float bestTime = default;
    public int kills = default;
    

    //public float accuracy = default;
    //public float perfectEvasions = default;
    //public float overChargedTime = default;
    //public float rocketShotdown = default;
    //public float unguidedAirShots = default;

    
    public override void Awake()
    {
        isGameOver = false;    
        Init();
    }
    //public override void Start()
    //{
    //} 
    //TEST  start �Դϴ�.

    public void ActiveText()
    {
        scoreText.gameObject.SetActive(true);
        bestScoreText.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true);
        bestTimeText.gameObject.SetActive(true);
        waveText.gameObject.SetActive(true);
        bestWaveText.gameObject.SetActive(true);
        killsText.gameObject.SetActive(true);
        expTextNum.gameObject.SetActive(true);
        nextExpTextNum.gameObject.SetActive(true);
    }


    private void Init()
    {
        
        if (GFunc.GetRootObj("PoolManager").IsValid() == false) { return; }
        poolManager = GFunc.GetRootObj("PoolManager").GetComponent<PoolManager>();
        if (GFunc.GetRootObj("WaveManager").IsValid() == false) { return; }
        waveManager = GFunc.GetRootObj("WaveManager").GetComponent<WaveManager>();
        if (GFunc.GetRootObj("Player").IsValid() == false) { return; }
        player = GFunc.GetRootObj("Player").GetComponent<PlayerController>();

        //------------------------------------------------------------KHJ �߰�
        GameObject defeatUi = GFunc.GetRootObj(RDefine.DEFEAT_RESULT);

        defeatUiObj = GFunc.GetRootObj(RDefine.DEFEAT_RESULT).transform.GetChild(0).gameObject;       
        tmpTextObjects = defeatUi.transform.GetChild(2).transform.GetComponentsInChildren<TMP_Text>() ;
        foreach(TMP_Text text in tmpTextObjects)
        { 
            if (text.name == "ScoreNum")
            {
                scoreText = text;
            }
            else if (text.name == "bestScoreNum")
            {
                bestScoreText = text;
            }
            else if (text.name == "TimeNum")
            {
                timeText = text;
            }
            else if (text.name == "bestTimeNum")
            {
                bestTimeText = text;
            }
            else if (text.name == "WaveNum")
            {
                waveText = text;
            }
            else if (text.name == "bestWaveNum")
            {
                bestWaveText = text;
            }
            else if (text.name == "killNum")
            {
                killsText = text;
            }
            else if (text.name == "expNum")
            {
                expTextNum = text;
            }
            else if (text.name == "nextExpNum")
            {
                nextExpTextNum = text;
            }
            //return;
        } 

        scoreText.gameObject.SetActive(false);
        bestScoreText.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);
        bestTimeText.gameObject.SetActive(false);
        waveText.gameObject.SetActive(false);
        bestWaveText.gameObject.SetActive(false);
        killsText.gameObject.SetActive(false);
        expTextNum.gameObject.SetActive(false);
        nextExpTextNum.gameObject.SetActive(false);
    }


   



    // =================================================================================HJK �߰��� �κ� ����
    public void AddScore( int newScore )
    {
        if(!isGameOver)
        {
            score += newScore;
            scoreText.text = string.Format("{0}", newScore);
        }
        
    }

    public float AddTime(ref float newTime )
    {
        newTime += Time.deltaTime;
        return newTime;
    }

    public void DefeatGame()
    {   //TODO �Ҹ� ���� �۾� ������մϴ� + �� �� �ٸ� �׾��µ� �ǰ��� �Ǽ� ����Ʈ�� ���´ٴ��� �� ó��
        isGameOver = true;  //�̰� true�� �Ǹ� �ϴ��� ������� �ٿ��ݴϴ�
        defeatUiObj.SetActive(true);
    }
}
