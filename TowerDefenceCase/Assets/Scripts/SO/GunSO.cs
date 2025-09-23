using UnityEngine;

public enum GunType
{
    Meelee,
    Ranged
};

[CreateAssetMenu(fileName = "GunData", menuName = "ScriptableObjects/Gun Data")]
public class GunSO : ScriptableObject
{
    public float FireCooldown;
    public float Damage;
    public GunType Type;
}
