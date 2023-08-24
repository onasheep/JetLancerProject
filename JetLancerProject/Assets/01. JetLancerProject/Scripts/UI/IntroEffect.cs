using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroEffect : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform[] speedEffects;
    private Vector3 spawnPos;
    private PlayerController player;
    void Start()
    {
        if(player.IsValid() == false)
        {
            player = GFunc.GetRootObj("Player").GetComponent<PlayerController>();
        }
        spawnPos = Vector3.zero;
        speedEffects = new Transform[5];
        this.transform.GetComponentInChildren<Transform>();
    }


    public void SpawnEffect()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
}
