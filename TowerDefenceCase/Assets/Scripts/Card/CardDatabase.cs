using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

[Serializable]
public class CardDicInfos
{
    public int CurrentLevel = 0;
    public CardSO CardInfoSO;
}

public class CardDatabase : MonoBehaviour
{
    public static CardDatabase instance;

    public Dictionary<int, CardDicInfos> CardDic;
    public CardSO[] Card;

    List<CardDicInfos> cardPool = new List<CardDicInfos>();

    private void Awake() => instance = this;

    private void Start()
    {
        CardDic = new Dictionary<int, CardDicInfos>();

        for (int i = 0; i < Card.Length; i++)
            CardDic.Add(Card[i].Id, new CardDicInfos 
            {
                CardInfoSO = Card[i], 
                CurrentLevel = 0 
            });

        CardDic.OrderBy(x => x.Value.CardInfoSO.Id);
    }

    public CardSO SelectRandomCard(CardSO[] currentSelectedCards)
    {
        cardPool.Clear();

        foreach (var item in CardDic)
        {
            if (item.Value.CurrentLevel >= item.Value.CardInfoSO.CardLevels.Length)
                continue;

            if (currentSelectedCards.Length > 0 && currentSelectedCards.Any(x => x != null && x.Id == item.Key))
                continue;

            if (item.Value.CardInfoSO.ConnectedCard != null)
            {
                if (item.Value.CurrentLevel <= 0)
                    continue;
            }

            cardPool.Add(item.Value);
        }

        if (cardPool.Count <= 0) return null;

        int sumRartiy = cardPool.Sum(x => x.CardInfoSO.Rartiy);
        int RandomValue = Random.Range(0, sumRartiy);

        int cumulative = 0;
        foreach (var item in cardPool)
        {
            cumulative += item.CardInfoSO.Rartiy;
            if (RandomValue < cumulative)
            {
                return item.CardInfoSO;
            }
        }

        return null;
    }

    public void LevelUpCard(CardSO cardInfo, int value)
    {
       var info = CardDic.FirstOrDefault(x => x.Value.CardInfoSO == cardInfo);
       info.Value.CurrentLevel += value;
    }

    public int GetCurrentCardLevel(CardSO cardInfo) => CardDic[cardInfo.Id].CurrentLevel;
}
