using UnityEngine;

public class Billboard : MonoBehaviour
{
    [Header("Billboard Ayarları")]
    [SerializeField] private bool oneTime = false;
    [SerializeField] private bool onlyYRotation = false; // ✅ Sadece sağ-sol dönüş için

    private Camera _cam;

    void Start()
    {
        _cam = Camera.main;

        if (oneTime)
        {
            UpdateRotation();
            enabled = false;
        }
    }

    void LateUpdate()
    {
        if (_cam == null) return;
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        if (!onlyYRotation)
        {
            // 🔹 Tam billboard (kamera yönüne bakar)
            transform.forward = _cam.transform.forward;
        }
        else
        {
            // 🔹 Sadece Y ekseninde (sağ-sol) döner
            Vector3 camPos = _cam.transform.position;
            Vector3 lookDir = camPos - transform.position;
            lookDir.y = 0f; // Y ekseni sabit kalır
            if (lookDir.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.LookRotation(-lookDir.normalized, Vector3.up);
        }
    }
}
