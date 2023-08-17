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

    private void Awake()
    {
        GameManager.Instance.poolManager = this;
    }
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

        
        
        
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;


        
        
        poolDictinoary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
    
}
