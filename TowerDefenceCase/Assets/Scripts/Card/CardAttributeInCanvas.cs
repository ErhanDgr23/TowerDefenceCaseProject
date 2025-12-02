using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CardAttributeInCanvas : MonoBehaviour
{
    public Image IconImage;
    public TextMeshProUGUI ValueText;

    public void InitAttribute(Sprite icon, int val, bool percent = false)
    {
        IconImage.sprite = icon;
        ValueText.text = percent == false ? (val > 0 ? "<color=green>+%" : "<color=red>-%") + val : (val > 0 ? "<color=green>+" : "<color=red>-") + val + "</color>";
    }
}
