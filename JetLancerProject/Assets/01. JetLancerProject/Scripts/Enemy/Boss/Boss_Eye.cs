using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Eye : MonoBehaviour, IDamageable
{
    // target Ȯ�ο� �ӽ� 
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
        // { Ÿ��, �Ÿ�, ���� ������, �߻翡 �ʿ��� ������ ��� �����Ƿ�, ��ġ�� ���� �� �� ����
        // ���� �� �� ����
        Transform targetPos = target.transform;


        distToTarget = (targetPos.position - this.transform.position).magnitude;
        dirToTarget = (targetPos.position - this.transform.position).normalized;
        targetAngle = Mathf.Atan(dirToTarget.y / dirToTarget.x) * Mathf.Rad2Deg;
        //  Ÿ��, �Ÿ�, ���� ������, �߻翡 �ʿ��� ������ ��� �����Ƿ�, ��ġ�� ���� �� �� ���� }
    }       // CheckTarget()

    private void RotateBerral()
    {

    }
    public void OnDamage(int damage)
    {
        // TODO : �׾��� �� �� ���� 

    }
}
