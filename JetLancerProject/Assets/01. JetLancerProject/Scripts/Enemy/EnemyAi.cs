using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public GameObject target;
    // Start is called before the first frame update

    private void Awake()
    {
        if(target.IsValid() == false)
        {
            target = FindObjectOfType<Player>().gameObject;
        }       // if : Ÿ���� null �̰ų� default�� ��� Ÿ���� ������
        
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
