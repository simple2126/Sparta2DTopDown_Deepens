using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    // 리스트는 자동적으로 시리얼라이즈 되고 딕셔너리는 안됨
    public List<Pool> pools = new List<Pool>();
    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    private void Awake()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(var pool in pools)
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }

            PoolDictionary.Add(pool.tag, queue);
        }
    }

    public GameObject SpawnFromPool(string tag)
    {
        if (!PoolDictionary.ContainsKey(tag))
        {
            return null;
        }

        GameObject obj = PoolDictionary[tag].Dequeue();
        PoolDictionary[tag].Enqueue(obj);

        obj.SetActive(true);
        return obj;
    }
}