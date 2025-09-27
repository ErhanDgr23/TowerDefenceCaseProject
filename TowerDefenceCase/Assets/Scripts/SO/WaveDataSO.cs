using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyWaveItem
{
    public EnemyTypeSO enemyType;
    public int count;
}

[CreateAssetMenu(fileName = "WaveData", menuName = "ScriptableObjects/Wave Data")]
public class WaveDataSO : ScriptableObject
{
    public bool IsBossWave;
    public List<EnemyWaveItem> enemies = new List<EnemyWaveItem>();
    public float waveCooldown;
    public float waveTimeMax;
    public float waveTimeMin;
}