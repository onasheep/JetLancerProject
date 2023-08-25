using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostEffect : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform[] boostEffect;
    private PlayerController player;
    private float scrollSpeed = 10f;

    void Start()
    {
        if(player.IsValid() == false)
        {
            player = GFunc.GetRootObj("Player").GetComponent<PlayerController>();
        }

        boostEffect = this.gameObject.GetComponentsInChildren<Transform>();      
    }

    public void ScrollEffect()
    {
        for (int i = 1; i < boostEffect.Length ;i++)
        {
            boostEffect[i].position -= player.transform.right * Time.deltaTime * scrollSpeed;
            CheckEffectPos();
        }
    }

    private void CheckEffectPos()
    {

        for (int i = 1; i < boostEffect.Length; i++)
        {
            if ((player.transform.position - boostEffect[i].transform.position).magnitude < 5f)
            {
                boostEffect[i].position = player.transform.right * 5f;
            }
        }
    }
}
