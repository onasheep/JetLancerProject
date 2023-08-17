using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<WaveData> waves;

    [System.Serializable]
    public class Wave
    {
        public int waveCount;
        public int enem_1_Num;
        public int enem_2_Num;
        public int enem_3_Num;
        public int enem_4_Num;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
