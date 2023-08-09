using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject bullet;

    //private float fireRate = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Fire();

        Debug.DrawRay(transform.position, this.transform.right,Color.red);
        if (Physics2D.Raycast(this.transform.position, this.transform.right, float.MaxValue,LayerMask.NameToLayer("Player")))
        {
            Debug.Log("들어가나?");
            Fire();
        }
    }

    public void Fire()
    {
        GameObject bulletObj = Instantiate(bullet,this.transform.position, Quaternion.identity);
        bulletObj.GetComponent<Rigidbody2D>().AddForce(this.transform.right * 2000f * Time.deltaTime, ForceMode2D.Impulse);

    }
}
