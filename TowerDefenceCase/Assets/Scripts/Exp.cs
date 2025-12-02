using UnityEngine;

public class Exp : MonoBehaviour {

    public float moveSpeed = 8f;
    public int value;

    RectTransform uiTarget;
    Camera cam;
    bool follow;

    float viewZ;

    private void Start()
    {
        uiTarget = MenuManager.instance.ExpTarget;
        cam = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        StartMoveToUI();
    }

    void StartMoveToUI()
    {
        viewZ = cam.WorldToScreenPoint(transform.position).z;
        if (viewZ < 0.1f) viewZ = 1f;

        follow = true;
    }

    private void LateUpdate()
    {
        if (!follow) return;

        // HER FRAME hedef pozisyonu güncelle
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, uiTarget.position);
        Vector3 targetWorldPos = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, viewZ));

        // --- LINEER HAREKET ---
        Vector3 dir = (targetWorldPos - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
        // -----------------------

        if (Vector3.Distance(transform.position, targetWorldPos) < 0.15f)
        {
            MenuManager menumanger = MenuManager.instance;
            PlayerStats playerstats = PlayerStats.Instance;

            follow = false;
            menumanger.LvlHolder.Replay();
            playerstats.PlayerXp += value;
            menumanger.UpdateLevel(playerstats.PlayerLevel, playerstats.PlayerXp);
            Destroy(gameObject);
        }
    }
}
