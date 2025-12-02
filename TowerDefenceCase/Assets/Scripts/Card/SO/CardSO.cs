using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CardLevelIndicator
{
    public int Levels;
    public int ValueChange;
}

[CreateAssetMenu(fileName = "CardInfo", menuName = "ScriptableObjects/CardInfo")]
public class CardSO : ScriptableObject
{
    public int Id;
    public string Name;
    public string Explanation;

    public CardLevelIndicator[] CardLevels;
    public CardSO ConnectedCard;

    [Range(1, 99)]
    public int Rartiy;

    [Space(5f)]

    public Sprite Icon;
    public CardAttributeSO[] CardAttributes;
}