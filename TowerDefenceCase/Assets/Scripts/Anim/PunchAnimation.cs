using UnityEngine;
using System.Collections;

public class PunchAnimation : MonoBehaviour
{
    [Header("Punch Settings")]
    public float punchScale = 1.2f;
    public float punchTime = 0.15f;

    Vector3 originalScale;
    Coroutine currentRoutine;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    /// <summary>
    /// Eğer animasyon oynamıyorsa oynatır.
    /// Oynuyorsa yok sayar.
    /// </summary>
    public void Play()
    {
        if (currentRoutine == null)
        {
            currentRoutine = StartCoroutine(PunchRoutine());
        }
    }

    /// <summary>
    /// Her zaman animasyonu en baştan oynatır.
    /// Devam eden animasyon varsa iptal eder.
    /// </summary>
    public void Replay()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        transform.localScale = originalScale;
        currentRoutine = StartCoroutine(PunchRoutine());
    }

    IEnumerator PunchRoutine()
    {
        float half = punchTime / 2f;
        float t = 0f;

        // --- Büyüme ---
        while (t < half)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, originalScale * punchScale, t / half);
            yield return null;
        }

        // --- Küçülme ---
        t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale * punchScale, originalScale, t / half);
            yield return null;
        }

        transform.localScale = originalScale;
        currentRoutine = null;
    }
}
