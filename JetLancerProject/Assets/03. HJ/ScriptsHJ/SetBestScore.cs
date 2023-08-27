using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SetBestScore : MonoBehaviour
{
    //public GameObject textObj;
    public TMP_Text bestScore = default;
    public TMP_Text topWave = default;

    private float score;
    private float wave;
    // Start is called before the first frame update
    void Start()
    {
        //bestScore = textObj.transform.GetChild(0).transform.GetComponent<TextMeshPro>(); 
        // topWave = textObj.transform.GetChild(1).transform.GetComponent<TextMeshPro>();
        bestScore.text = string.Format("BEST SCORE\n{0}", score);
        topWave.text = string.Format("TOP WAVE\n{0}", wave);
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            score = GameManager.Instance.bestScore;
            wave = GameManager.Instance.bestWave;
        }
           
      
    }
}
