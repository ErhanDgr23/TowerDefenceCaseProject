using System.Collections;
using UnityEngine;

public class CameraFollowFixedRotation : MonoBehaviour
{
    public static CameraFollowFixedRotation instance;

    [Header("Target")]
    public Transform target;

    [Header("Follow Settings")]
    public Vector3 offset = new Vector3(0f, 6f, -8f);
    public float smoothSpeed = 5f;

    [Header("Clamp Settings (World Limits)")]
    public bool useLimits = true;
    public Vector2 minLimits = new Vector2(-10f, -10f);
    public Vector2 maxLimits = new Vector2(10f, 10f);

    private float smoothDuration = 0.5f;
    private Coroutine smoothCoroutine;
    private Vector3 desiredPosition;
    private Vector3 smoothedPosition;
    private Quaternion fixedRotation;

    // 🔹 Yeni değişkenler
    private float lastInputTime = 0f;
    private float inputCooldown = 1f; // 1 saniyede 1 güncelleme
    private float idleTime = 2f; // 2 saniye sonra 0'a dön
    private Coroutine returnToZeroCoroutine;
    private float lastRequestedZ = 0f;

    private void Awake() => instance = this;

    private void Start()
    {
        fixedRotation = transform.rotation;
    }

    public void OffsetZChange(float val)
    {
        float currentTime = Time.time;

        // Çok sık çağrılıyorsa engelle (örnek: 1 saniyede 1 kez izin ver)
        if (currentTime - lastInputTime < inputCooldown)
        {
            lastRequestedZ = val;
            return;
        }

        lastInputTime = currentTime;

        // Eğer sıfıra dönüş coroutine'i varsa durdur (çünkü yeni input geldi)
        if (returnToZeroCoroutine != null)
        {
            StopCoroutine(returnToZeroCoroutine);
            returnToZeroCoroutine = null;
        }

        //Debug.Log("<color=green> Camera </color> Offset Z Changed: " + val);

        // Eski coroutine'i durdur
        if (smoothCoroutine != null)
        {
            StopCoroutine(smoothCoroutine);
            smoothCoroutine = null;
        }

        // Smooth geçiş başlat
        smoothCoroutine = StartCoroutine(SmoothZ(val));

        // Yeni giriş geldikten sonra sıfıra dönüş için zamanlayıcı başlat
        if (returnToZeroCoroutine != null)
        {
            StopCoroutine(returnToZeroCoroutine);
        }
        returnToZeroCoroutine = StartCoroutine(ReturnToZeroAfterDelay());
    }

    private IEnumerator SmoothZ(float targetZ)
    {
        float startZ = offset.z;
        float elapsed = 0f;

        if (Mathf.Approximately(startZ, targetZ))
        {
            smoothCoroutine = null;
            yield break;
        }

        while (elapsed < smoothDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / smoothDuration);
            offset.z = Mathf.Lerp(startZ, targetZ, t);
            yield return null;
        }

        offset.z = targetZ;
        smoothCoroutine = null;
    }

    private IEnumerator ReturnToZeroAfterDelay()
    {
        // 2 saniye boyunca yeni giriş gelmezse sıfıra dön
        float startTime = Time.time;
        while (Time.time - lastInputTime < idleTime)
            yield return null;

        // Smooth sıfıra dönüş
        if (smoothCoroutine != null)
        {
            StopCoroutine(smoothCoroutine);
            smoothCoroutine = null;
        }
        smoothCoroutine = StartCoroutine(SmoothZ(0f));
        returnToZeroCoroutine = null;
    }

    private void LateUpdate()
    {
        if (!target) return;

        desiredPosition = target.position + offset;

        if (useLimits)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minLimits.x, maxLimits.x);
            desiredPosition.z = Mathf.Clamp(desiredPosition.z, minLimits.y, maxLimits.y);
        }

        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
        transform.rotation = fixedRotation;
    }

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
