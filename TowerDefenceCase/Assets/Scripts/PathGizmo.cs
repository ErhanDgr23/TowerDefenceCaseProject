using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PathGizmo : MonoBehaviour
{
    public static PathGizmo Instance;

    public List<Transform> pathPoints = new List<Transform>();
    public Color pathColor = Color.red;
    public float pointRadius = 0.3f;

    private void Awake() => Instance = this;

    private void OnDrawGizmos()
    {
        if (pathPoints == null || pathPoints.Count == 0) return;

        Gizmos.color = pathColor;

        for (int i = 0; i < pathPoints.Count; i++)
        {
            Transform point = pathPoints[i];
            if (point == null) continue;

            Gizmos.DrawSphere(point.position, pointRadius);

            if (i < pathPoints.Count - 1 && pathPoints[i + 1] != null)
            {
                Gizmos.DrawLine(point.position, pathPoints[i + 1].position);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < pathPoints.Count; i++)
        {
            if (pathPoints[i] != null)
                Gizmos.DrawIcon(pathPoints[i].position, "sv_label_0", true);
        }
    }
}
