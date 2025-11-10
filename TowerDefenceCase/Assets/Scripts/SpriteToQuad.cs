using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SpriteToQuad : MonoBehaviour
{
    public Sprite sprite; // Multiple sprite sheet’ten tek frame
    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        mesh = new Mesh();
        meshFilter.mesh = mesh;

        if (sprite != null)
            ApplySprite(sprite);
    }

    public void ApplySprite(Sprite s)
    {
        if (s == null) return;

        meshRenderer.sharedMaterial.mainTexture = s.texture;

        Rect rect = s.textureRect;
        Vector2[] uv = new Vector2[4];
        float xMin = rect.x / s.texture.width;
        float xMax = (rect.x + rect.width) / s.texture.width;
        float yMin = rect.y / s.texture.height;
        float yMax = (rect.y + rect.height) / s.texture.height;

        uv[0] = new Vector2(xMin, yMin);
        uv[1] = new Vector2(xMax, yMin);
        uv[2] = new Vector2(xMin, yMax);
        uv[3] = new Vector2(xMax, yMax);

        mesh.vertices = new Vector3[]
        {
            new Vector3(-0.5f, -0.5f, 0),
            new Vector3(0.5f, -0.5f, 0),
            new Vector3(-0.5f, 0.5f, 0),
            new Vector3(0.5f, 0.5f, 0)
        };

        mesh.uv = uv;
        mesh.triangles = new int[] { 0, 2, 1, 2, 3, 1 };
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
}
