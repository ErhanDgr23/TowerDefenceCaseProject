using UnityEngine;

public class CameraFollowFixedRotation : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // Takip edilecek oyuncu

    [Header("Follow Settings")]
    public Vector3 offset = new Vector3(0f, 6f, -8f); // Kamera pozisyon ofseti
    public float smoothSpeed = 5f; // Yumuşaklık oranı

    [Header("Clamp Settings (World Limits)")]
    public bool useLimits = true;
    public Vector2 minLimits = new Vector2(-10f, -10f);
    public Vector2 maxLimits = new Vector2(10f, 10f);

    private Vector3 desiredPosition;
    private Vector3 smoothedPosition;
    private Quaternion fixedRotation;

    private void Start()
    {
        // Başlangıçta kameranın mevcut rotasyonunu kaydet
        fixedRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        if (!target) return;

        // 1️⃣ Hedef pozisyon (offset ekleyerek)
        desiredPosition = target.position + offset;

        // 2️⃣ Kamera sınırlarını uygula (X-Z düzleminde)
        if (useLimits)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minLimits.x, maxLimits.x);
            desiredPosition.z = Mathf.Clamp(desiredPosition.z, minLimits.y, maxLimits.y);
        }

        // 3️⃣ Smooth hareket
        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // 4️⃣ Pozisyonu uygula
        transform.position = smoothedPosition;

        // 5️⃣ Rotasyonu sabit tut (kullanıcının istediği gibi)
        transform.rotation = fixedRotation;
    }

    // Sahnede sınırlama alanını göster
    private void OnDrawGizmosSelected()
    {
        if (!useLimits) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(minLimits.x, 0, minLimits.y), new Vector3(maxLimits.x, 0, minLimits.y));
        Gizmos.DrawLine(new Vector3(maxLimits.x, 0, minLimits.y), new Vector3(maxLimits.x, 0, maxLimits.y));
        Gizmos.DrawLine(new Vector3(maxLimits.x, 0, maxLimits.y), new Vector3(minLimits.x, 0, maxLimits.y));
        Gizmos.DrawLine(new Vector3(minLimits.x, 0, maxLimits.y), new Vector3(minLimits.x, 0, minLimits.y));
    }
}
