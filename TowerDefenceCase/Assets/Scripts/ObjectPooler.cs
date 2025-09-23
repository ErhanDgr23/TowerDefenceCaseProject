using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class PoolItem
{
    public GameObject prefab;
    public int size;
}

public class ObjectPooler : MonoBehaviour
{
    public List<PoolItem> itemsToPool = new List<PoolItem>();
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary;
    private Dictionary<GameObject, GameObject> objectToPrefabMap;

    void Awake()
    {
        poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();
        objectToPrefabMap = new Dictionary<GameObject, GameObject>();

        foreach (var item in itemsToPool)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < item.size; i++)
            {
                GameObject obj = Instantiate(item.prefab);
                obj.SetActive(false);
                obj.transform.parent = transform;
                objectPool.Enqueue(obj);

                objectToPrefabMap[obj] = item.prefab;
            }

            poolDictionary.Add(item.prefab, objectPool);
        }
    }

    public GameObject SpawnFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            Debug.LogWarning("Pool bulunamadı: " + prefab.name);
            return null;
        }

        GameObject objectToSpawn = poolDictionary[prefab].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        return objectToSpawn;
    }

    public void ReturnToPool(GameObject obj, float delay = 0f)
    {
        if (!objectToPrefabMap.ContainsKey(obj))
        {
            Debug.LogWarning("Bu obje herhangi bir poola ait değil: " + obj.name);
            Destroy(obj);
            return;
        }

        if (delay > 0f)
            StartCoroutine(ReturnWithDelay(obj, delay));
        else
            ReturnImmediate(obj);
    }

    private IEnumerator ReturnWithDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnImmediate(obj);
    }

    private void ReturnImmediate(GameObject obj)
    {
        GameObject prefab = objectToPrefabMap[obj];

        obj.SetActive(false);
        obj.transform.parent = transform;
        poolDictionary[prefab].Enqueue(obj);
    }
}
