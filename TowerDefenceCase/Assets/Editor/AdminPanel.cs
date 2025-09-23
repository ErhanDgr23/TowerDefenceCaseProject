using UnityEngine;
using UnityEditor;
using System.Reflection;

public class AdminPanel : EditorWindow
{
    private GameObject targetObject;

    [MenuItem("Tools/Admin Panel")]
    public static void ShowWindow()
    {
        GetWindow<AdminPanel>("Admin Panel");
    }

    private void OnGUI()
    {
        GUILayout.Label("Admin Panel", EditorStyles.boldLabel);

        targetObject = (GameObject)EditorGUILayout.ObjectField("Target Object", targetObject, typeof(GameObject), true);

        if (targetObject == null)
        {
            GUILayout.Label("Select an object with scripts", EditorStyles.helpBox);
            return;
        }

        var components = targetObject.GetComponents<MonoBehaviour>();

        foreach (var comp in components)
        {
            if (comp == null) continue;

            GUILayout.Space(5);
            GUILayout.Label(comp.GetType().Name, EditorStyles.boldLabel);

            MethodInfo[] methods = comp.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            foreach (var method in methods)
            {
                // Sadece parametresiz metodlar
                if (method.GetParameters().Length == 0)
                {
                    if (GUILayout.Button(method.Name))
                    {
                        method.Invoke(comp, null);
                        Debug.Log($"Called {method.Name} on {comp.name}");
                    }
                }
            }
        }
    }
}
