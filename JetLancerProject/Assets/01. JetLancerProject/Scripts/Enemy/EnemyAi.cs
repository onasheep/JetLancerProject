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
        }       // if : 타겟이 null 이거나 default인 경우 타겟을 가져옴
        
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
