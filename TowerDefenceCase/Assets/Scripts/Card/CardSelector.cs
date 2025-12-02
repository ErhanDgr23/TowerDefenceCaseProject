using System.Collections.Generic;
using UnityEngine;

public class CardSelector : MonoBehaviour {

    [SerializeField] CardVisualizert VardVisualizer;
    [SerializeField] CardDatabase Database;

    [SerializeField] CardInCanvas[] CardsInCanvas;

    public List<CardSO> CurrentSelectedCard = new List<CardSO>();

    private void Start() => Database = GetComponent<CardDatabase>();

    public void OpenPanel()
    {
        Time.timeScale = 0f;
        VardVisualizer.OpenPanel();
        PickThreeCard();
    }

    public void ClosePanel()
    {
        Time.timeScale = 1f;
        ClearCardAndBuffs();
        VardVisualizer.ClosePanel();
    }

    void ClearCardAndBuffs()
    {
        CurrentSelectedCard.Clear();

        foreach (var item in CardsInCanvas)
            item.ClearCard();
    }

    void PickThreeCard()
    {
        CurrentSelectedCard.Clear();

        foreach (var item in CardsInCanvas)
        {
            item.InitCard(Database.SelectRandomCard(CurrentSelectedCard.ToArray()), ClosePanel);
            CurrentSelectedCard.Add(item.CardInfoSO);
        }
    }
}