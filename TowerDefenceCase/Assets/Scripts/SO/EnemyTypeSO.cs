using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType", menuName = "ScriptableObjects/EnemyTypeObject")]
public class EnemyTypeSO : ScriptableObject
{
    [Header("Animation And Graphics")]
    public Sprite EnemySprite;
    public SpriteAnimation[] RunAnimationSprites;

    [Space(10)]

    [Header("Stats")]
    public float Health;
    public float Speed;
    public int Damage;
}
