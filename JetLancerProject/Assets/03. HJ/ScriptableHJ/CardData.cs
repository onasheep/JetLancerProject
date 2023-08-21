using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/CardData", fileName = "Card Data")]
public class CardData : ScriptableObject
{
    public float damage = 1; // ���ݷ�

    public float healAmount = 1; //ġ����

    public float bulletSpeed = 10; //�Ѿ� ���ǵ�

    public bool isSpecialOn = false; //Ư����� ��밡������
    
    public bool isShield = false;  //����
    public bool isGuidedMissile = false; //�����̻���
    public bool isBomb = false; //��ź

}
