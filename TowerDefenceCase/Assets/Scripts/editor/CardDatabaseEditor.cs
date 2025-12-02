using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(CardDatabase))]
public class CardDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CardDatabase db = (CardDatabase)target;

        if (GUILayout.Button("Show Dictionary"))
        {
            if (db.CardDic.Count == 0)
            {
                Debug.Log("CardDic boş!");
            }
            else
            {
                foreach (KeyValuePair<int, CardDicInfos> kvp in db.CardDic)
                {
                    string cardName = kvp.Value.CardInfoSO != null ? kvp.Value.CardInfoSO.name : "null";
                    Debug.Log($"Key: {kvp.Key}, CardName: {cardName}, Level: {kvp.Value.CurrentLevel}");
                }
            }
        }
    }
}
