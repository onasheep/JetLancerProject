using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/CardData", fileName = "Card Data")]
public class CardData : ScriptableObject
{
    public float damage = 1; // 공격력

    public float healAmount = 1; //치유량

    public float bulletSpeed = 10; //총알 스피드

    public bool isSpecialOn = false; //특수기술 사용가능한지
    
    public bool isShield = false;  //방패
    public bool isGuidedMissile = false; //유도미사일
    public bool isBomb = false; //폭탄

}
