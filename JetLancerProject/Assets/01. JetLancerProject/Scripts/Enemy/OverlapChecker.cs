using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapChecker : MonoBehaviour
{
    
    public Queue<EnemyBase> enemyQue;

    // Start is called before the first frame update
    void Start()
    {
        enemyQue = new Queue<EnemyBase> ();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            enemyQue.Enqueue(collision.gameObject.GetComponent<EnemyBase>());
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
    //    {
    //        enemyQue.Dequeue();
    //    }
    //}
}
