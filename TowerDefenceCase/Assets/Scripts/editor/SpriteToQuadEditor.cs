using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpriteToQuad))]
public class SpriteToQuadEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SpriteToQuad script = (SpriteToQuad)target;

        if (GUILayout.Button("Convert To Quad"))
        {
            if (script.sprite == null)
            {
                Debug.LogWarning("Sprite atanmamış!");
                return;
            }

            script.ApplySprite(script.sprite);

            // Mesh verilerini editörde kalıcı hale getir
            Mesh meshCopy = Object.Instantiate(script.GetComponent<MeshFilter>().sharedMesh);
            meshCopy.name = script.sprite.name + "_QuadMesh";

            string path = "Assets/" + meshCopy.name + ".asset";
            AssetDatabase.CreateAsset(meshCopy, path);
            AssetDatabase.SaveAssets();

            script.GetComponent<MeshFilter>().sharedMesh = meshCopy;

            // MeshRenderer ayarları
            MeshRenderer mr = script.GetComponent<MeshRenderer>();
            if (mr.sharedMaterial == null)
            {
                Shader shader = Shader.Find("Universal Render Pipeline/Lit");
                Material mat = new Material(shader);
                mat.mainTexture = script.sprite.texture;

                string matPath = "Assets/" + script.sprite.name + "_Mat.mat";
                AssetDatabase.CreateAsset(mat, matPath);
                AssetDatabase.SaveAssets();

                mr.sharedMaterial = mat;
            }

            EditorUtility.SetDirty(script);
            Debug.Log($"✅ {script.sprite.name} sprite'ı quad'a dönüştürüldü ve gölge alabilir hale getirildi!");
        }
    }
}
