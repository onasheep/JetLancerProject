using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "WaveData", order = int.MinValue)]
public class WaveData : ScriptableObject
{
    public int waveCount;
    public int enem_1_Num;
    public int enem_2_Num;
    public int enem_3_Num;
    public int enem_4_Num;
    public string boss_Type;
}
