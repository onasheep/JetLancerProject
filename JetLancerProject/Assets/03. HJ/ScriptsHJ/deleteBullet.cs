using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteBullet : MonoBehaviour
{
    private float deleteTimer;
    // Start is called before the first frame update
    void Start()
    {
        deleteTimer = Time.time + 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (deleteTimer < Time.time)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // SJ_
        //{ PlayerController에 있는 OnDamage() 를 호출함        
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            // TODO : 추후 데미지 추가되면 임시변수가 아닌 가져와서 쓸것
            int damge = 10;
            collision.GetComponent<EnemyBase>().OnDamage(damge);
        }       // if : layer를 통해서 playerController를 가져오고 데미지를 줌
        else { /* Do Nothing */ }
        // PlayerController에 있는 OnDamage() 를 호출함 }


        Destroy(gameObject);

    }
}
