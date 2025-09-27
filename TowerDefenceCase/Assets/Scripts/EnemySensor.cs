using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySensor : MonoBehaviour
{
    public event Action<List<GameObject>> EnemyReach;

    public List<GameObject> enemys = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemys.Add(other.gameObject);
            other.gameObject.GetComponent<EnemyManager>().OnDead += ZombieDead;
            EnemyReach?.Invoke(enemys);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemys.Remove(other.gameObject);
            other.gameObject.GetComponent<EnemyManager>().OnDead -= ZombieDead;
            EnemyReach?.Invoke(enemys);
        }
    }

    public void ZombieDead(GameObject enemyObj, bool spawnedFromboss) => enemys.Remove(enemyObj);

    public void FireEvent() => EnemyReach?.Invoke(enemys);
}
