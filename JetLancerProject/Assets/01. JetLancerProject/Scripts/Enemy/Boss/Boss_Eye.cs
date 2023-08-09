using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Eye : MonoBehaviour, IDamageable
{
    // target 확인용 임시 
    public GameObject target = default;
    public GameObject berral = default;

    float distToTarget = default;
    float targetAngle = default;
    Vector2 dirToTarget = default;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckTarget();

    }

    private  void CheckTarget()
    {
        // { 타겟, 거리, 방향 정보값, 발사에 필요한 정보가 담겨 있으므로, 위치가 변경 될 수 있음
        // 변경 될 수 있음
        Transform targetPos = target.transform;


        distToTarget = (targetPos.position - this.transform.position).magnitude;
        dirToTarget = (targetPos.position - this.transform.position).normalized;
        targetAngle = Mathf.Atan(dirToTarget.y / dirToTarget.x) * Mathf.Rad2Deg;
        //  타겟, 거리, 방향 정보값, 발사에 필요한 정보가 담겨 있으므로, 위치가 변경 될 수 있음 }
    }       // CheckTarget()

    private void RotateBerral()
    {

    }
    public void OnDamage(int damage)
    {
        // TODO : 죽었을 떄 할 무언가 

    }
}
