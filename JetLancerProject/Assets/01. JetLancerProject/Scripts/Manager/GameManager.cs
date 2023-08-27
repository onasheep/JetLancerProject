using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public bool isVictory = false;
    public bool isVictoryText = false;

    public bool isActiveText = false;
    public bool isEngague = true;


    // 텍스트
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

    // 변수
    public int score = default;
    public int bestScore = default;
    public int wave = default;
    public int bestWave = default;  
    public float time = default;
    public float bestTime = default;
    public int kills = default;
    

   
    
    public override void Awake()
    {
        isGameOver = false;    
        Init();
    }
 
    public override void Update()
    {
        if(isGameOver && !Input.anyKey) //계속 실행 시켜줍니다.
        {
            if (bestScore < score) 
            {
                bestScore = score;
                Debug.LogFormat("이거 스코어 갱신 됨? {0}",bestScore);
                bestScoreText.text = string.Format("{0}", bestScore);
            }
            if ( bestTime < time) 
            {
                TimeSpan currentTime = TimeSpan.FromSeconds(time);
                bestTime = time;
                bestTimeText.text = string.Format("{0}:{1}:{2}", currentTime.Hours, currentTime.Minutes, currentTime.Seconds);
            }
            if(bestWave < wave)
            {
                bestWave = wave;
                bestWaveText.text = string.Format("{0}", bestWave);
            }
            Invoke("ActiveText",2f);
            
        }
        else if (isGameOver && Input.anyKey)
        {
            float pushAnykey = 0f;
            pushAnykey +=  Time.time;
            Debug.LogFormat("{0}",pushAnykey);
            if (pushAnykey > 3f)
            {
                SceneManager.LoadScene("CharacterSelect");
            }
        }    
        else
        {
            UpdateTime();
            UpdateWave();
        }
        
    }

    public void ActiveText()
    {
        if (isActiveText)
        {
            scoreText.gameObject.SetActive(true);
            bestScoreText.gameObject.SetActive(true);
            expTextNum.gameObject.SetActive(true);
            nextExpTextNum.gameObject.SetActive(true);


            timeText.gameObject.SetActive(true);
            bestTimeText.gameObject.SetActive(true);

            waveText.gameObject.SetActive(true);
            bestWaveText.gameObject.SetActive(true);


            killsText.gameObject.SetActive(true);
        }
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


  



    // =================================================================================HJK 추가한 부분 시작
    public void AddScore( int newScore )
    {
        if(!isGameOver)
        {
            score += newScore;
            kills += 1;
            scoreText.text = string.Format("{0}", score);
            killsText.text = string.Format("{0}", kills);
            bestScoreText.text = string.Format("{0}", bestScore);
        }
    }

    public void UpdateTime()
    {
        if (!isActiveText)
        {
            time += Time.deltaTime;
        }
        TimeSpan currentTime = TimeSpan.FromSeconds(time);
        timeText.text = string.Format("{0}:{1}:{2}", currentTime.Hours,currentTime.Minutes, currentTime.Seconds);
    }
    public void UpdateWave()
    {
        wave = waveManager.curWave;
        waveText.text = string.Format("{0}",wave);

    }

    public void DefeatGame()
    {   //TODO 소리 관련 작업 해줘야합니다 + 그 외 다른 죽었는데 피격이 되서 이펙트가 나온다던가 등 처리
        isGameOver = true;  //이게 true가 되면 일단은 배경음을 줄여줍니다
        defeatUiObj.SetActive(true);   // >>>>>>> defeat bottom 오브젝트를 켜줍니다 . 그러면 애니메이션이 시작됩니다.
    }
    
}
