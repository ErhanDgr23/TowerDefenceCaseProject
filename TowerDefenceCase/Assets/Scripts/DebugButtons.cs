using UnityEngine;

public class DebugButtons : MonoBehaviour
{
#if UNITY_EDITOR
    public CardSelector cardSelector;

    private bool showGUI = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            showGUI = !showGUI;
        }
    }

    private void OnGUI()
    {
        if (!showGUI) return;

        GUI.backgroundColor = Color.green;

        if (GUI.Button(new Rect(10, 10, 150, 40), "Open Card Selector"))
        {
            cardSelector.OpenPanel();
        }

        if (GUI.Button(new Rect(160, 10, 150, 40), "Close Card Selector"))
        {
            cardSelector.ClosePanel();
        }
    }
#endif
}
