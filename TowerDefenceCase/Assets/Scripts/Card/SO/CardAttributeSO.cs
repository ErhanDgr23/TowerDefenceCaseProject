using UnityEngine;

public enum AttributeType
{
    Value,
    Percent
}

[CreateAssetMenu(fileName = "CardAttribute", menuName = "ScriptableObjects/CardAttribute")]
public class CardAttributeSO : ScriptableObject {

    public Sprite AttributeIcon;
    public string AttributeName;

    public AttributeType AttributeType;
}