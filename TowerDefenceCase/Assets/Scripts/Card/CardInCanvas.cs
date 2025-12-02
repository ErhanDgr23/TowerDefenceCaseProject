using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class CardInCanvas : MonoBehaviour
{
    public CardSO CardInfoSO;

    [SerializeField] Image IconImage;
    [SerializeField] Button CardButton;
    [SerializeField] TextMeshProUGUI TitleText;
    [SerializeField] TextMeshProUGUI DescriptionText;
    [SerializeField] DOTweenAnimation CardAnimation;

    [SerializeField] Transform BuffParent;
    [SerializeField] GameObject BuffPrefab;

    public CardDatabase _cardDatabase;
    Action _clickFunc;

    private void OnEnable() => CardAnimation.DORestart();

    public void InitCard(CardSO info, Action CloseCardPanelFunc)
    {
        if (_cardDatabase == null) _cardDatabase = CardDatabase.instance;

        CardInfoSO = info;

        if (CardInfoSO == null) return;

        IconImage.sprite = CardInfoSO.Icon;

        DescriptionText.text = CardInfoSO.Explanation;
        TitleText.text = CardInfoSO.Name;

        CardButton.onClick.RemoveAllListeners();
        CardButton.onClick.AddListener(() => ButtonClick());
        CardButton.onClick.AddListener(() => CardDatabase.instance.LevelUpCard(CardInfoSO, 1));
        CardButton.onClick.AddListener(() => CloseCardPanelFunc());

        for (int i = 0; i < CardInfoSO.CardAttributes.Length; i++)
        {
            GameObject buffPre = Instantiate(BuffPrefab, BuffParent);
            CardAttributeInCanvas buffPreAttribute = buffPre.GetComponent<CardAttributeInCanvas>();
            //buffPreAttribute.InitAttribute(CardInfoSO.CardAttributes[i].AttributeIcon, CardInfoSO.CardAttributes[i].AttributeValue, CardInfoSO.CardAttributes[i].AttributeType == AttributeType.Value ? false : true);

            print(CardInfoSO.CardLevels[Mathf.Min(_cardDatabase.GetCurrentCardLevel(CardInfoSO), CardInfoSO.CardLevels.Length - 1)].ValueChange);

            buffPreAttribute.InitAttribute(
    CardInfoSO.CardAttributes[i].AttributeIcon,
    CardInfoSO.CardLevels[
        Mathf.Min(_cardDatabase.GetCurrentCardLevel(CardInfoSO), CardInfoSO.CardLevels.Length - 1)
    ].ValueChange,
    CardInfoSO.CardAttributes[i].AttributeType == AttributeType.Value ? false : true
);

        }
    }

    public void ClearCard()
    {
        CardInfoSO = null;

        DescriptionText.text = "";
        TitleText.text = "";

        CardButton.onClick.RemoveAllListeners();

        for (int i = 0; i < BuffParent.childCount; i++)
            Destroy(BuffParent.GetChild(i).gameObject);
    }

    public void ButtonClick() => PlayerStats.Instance.ChangeStatsWithId(CardInfoSO, CardDatabase.instance.GetCurrentCardLevel(CardInfoSO));
}
