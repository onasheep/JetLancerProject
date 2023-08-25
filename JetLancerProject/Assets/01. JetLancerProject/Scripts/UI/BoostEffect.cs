using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostEffect : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform[] boostEffect;
    private PlayerController player;
    private float scrollSpeed = 15f;

    void Start()
   {
      

        boostEffect = this.gameObject.GetComponentsInChildren<Transform>();      
    }


    private void Update()
    {
        ScrollEffect();
    }
    public void ScrollEffect()
    {
        for (int i = 1; i < boostEffect.Length ;i++)
        {
            boostEffect[i].position -= new Vector3(Time.deltaTime * scrollSpeed,0f) ;
        }
    }

}
