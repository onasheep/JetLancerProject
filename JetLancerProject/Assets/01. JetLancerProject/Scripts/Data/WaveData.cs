using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "WaveData", order = int.MinValue)]
public class WaveData : ScriptableObject
{
    public int waveCount;
    public int enemNum;
    public int bossNum;
    public string boss_Type;
}
