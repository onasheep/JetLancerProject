using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour, IDeactive
{
    // Start is called before the first frame update
    private Rigidbody2D rigid;
    private float existTime = 10f;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        Invoke("Deactive", existTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            int damage = 1;
            collision.GetComponent<PlayerController>().OnDamage(damage);
        }
    }


    public void Deactive()
    {
        this.gameObject.SetActive(false);
    }       // Deactive()
}
