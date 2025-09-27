using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType", menuName = "ScriptableObjects/EnemyTypeObject(SpawnerBoss)")]
public class EnemyTypeSpawnerBossSO : EnemyTypeSO
{
    public bool IsBoss;
    public GameObject MinionPrefab;
    public EnemyTypeSO MinionType;
    public float SpawnInterval = 5f;
    public int SpawnCount = 2;
}