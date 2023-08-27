using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefabs;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictinoary;

    void Start()
    {
        poolDictinoary = new Dictionary<string, Queue<GameObject>>();
        
        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            GameObject poolConstaincer=  new GameObject(pool.tag +"Pool");
            poolConstaincer.transform.SetParent(this.transform);
            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefabs, poolConstaincer.transform);

                obj.SetActive(false);
                
                objectPool.Enqueue(obj);
            }
            poolDictinoary.Add(pool.tag, objectPool);
        }

    }


    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (poolDictinoary.ContainsKey(tag) == false)
        {
            Debug.LogWarning("Pool with tag " + tag + "doesn't exist.");
            return null;
        }
        
        
        GameObject objectToSpawn = poolDictinoary[tag].Dequeue();

        
        if (objectToSpawn.GetComponent<Rigidbody2D>() != null)
        {
            objectToSpawn.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }       // if : rigidBody를 가지고 있는 객체의 속도 초기화

        // TODO : 필요성 확인 후 삭제 
        if(objectToSpawn.GetComponent<Animator>() != null)
        {
            objectToSpawn.GetComponent<Animator>().enabled = true;
        }       // if : Animator를 가지고 있는 객체의 애니메이터 초기화

        
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.transform.localScale = Vector3.one;

        poolDictinoary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
    
    

}
