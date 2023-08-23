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
    //형준 변경
    public GameObject defeatUiObj = default;
    //============================================


    // bool 변수
    //public bool isGameover { get; private set; } 
    public bool isGameOver = default;

    // 텍스트
    public TMP_Text scoreText = default;
    public TMP_Text bestScoreText = default;
    public TMP_Text timeText = default;
    public TMP_Text waveText = default;
    public TMP_Text bestWaveText = default;
    public TMP_Text[] tmpTextObjects = new TMP_Text[15];

    // 변수
    public int score = default;
    public int bestScore = default;
    public int wave = default;
    public int bestWave = default;  
    public int kills = default;
    public float time = default;

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

   
    private void Init()
    {
        
        if (GFunc.GetRootObj("PoolManager").IsValid() == false) { return; }
        poolManager = GFunc.GetRootObj("PoolManager").GetComponent<PoolManager>();
        if (GFunc.GetRootObj("WaveManager").IsValid() == false) { return; }
        waveManager = GFunc.GetRootObj("WaveManager").GetComponent<WaveManager>();
        if (GFunc.GetRootObj("Player").IsValid() == false) { return; }
        player = GFunc.GetRootObj("Player").GetComponent<PlayerController>();

        //------------------------------------------------------------KHJ 추가
        GameObject defeatUi = GFunc.GetRootObj(RDefine.DEFEAT_RESULT);

        defeatUiObj = GFunc.GetRootObj(RDefine.DEFEAT_RESULT).transform.GetChild(0).gameObject;       
        tmpTextObjects = defeatUi.GetComponentsInChildren<TMP_Text>() ;

        Debug.Log(defeatUi);

        Debug.Log(tmpTextObjects.Length);
        foreach(TMP_Text text in tmpTextObjects)
        {
            Debug.LogFormat("{0}", text.name);
            if (text.name == defeatUiObj.name)
                return;
        }
        //GFunc.GetRootObj(RDefine.DEFEAT_RESULT).FindChildObj<TMP_Text>();

        //scoreText = default;

        //bestScoreText = default;
        //timeText = default;
        //waveText = default;
        //bestWaveText = default;
    }


   
    

    // =================================================================================HJK 추가한 부분 시작
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
    {   //TODO 소리 관련 작업 해줘야합니다 + 그 외 다른 죽었는데 피격이 되서 이펙트가 나온다던가 등 처리
        isGameOver = true;  //이게 true가 되면 일단은 배경음을 줄여줍니다
        defeatUiObj.SetActive(true);
    }
}
