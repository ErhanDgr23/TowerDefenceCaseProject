using UnityEngine;

public class CardVisualizert : MonoBehaviour {

    [SerializeField] GameObject PanelObj;

    public void OpenPanel()
    {
        PanelObj.gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        PanelObj.gameObject.SetActive(false);
    }
}
