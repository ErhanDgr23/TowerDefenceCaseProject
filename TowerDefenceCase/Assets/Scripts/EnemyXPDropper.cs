using System.Collections;
using UnityEngine;

public class EnemyXPDropper : MonoBehaviour {

    private GameObject xpPrefab;
    private int minXp = 1;
    private int maxXp = 4;

    private float scatterRadius = 1.5f;
    private float scatterSpeed = 4f;

    EnemyManager enemy;

    void Awake() => enemy = GetComponent<EnemyManager>();

    private void Start() => xpPrefab = enemy.Type.XpPrefab;

    void OnEnable() => enemy.OnDead += HandleDead;

    void OnDisable() => enemy.OnDead -= HandleDead;

    void HandleDead(GameObject deadEnemy, bool fromBoss) => SpawnXP();

    void SpawnXP()
    {
        int xpCount = Random.Range(minXp, maxXp + 1);

        for (int i = 0; i < xpCount; i++)
        {
            GameObject xp = Instantiate(xpPrefab, transform.position, Quaternion.identity);

            Vector3 target = transform.position + new Vector3(
                Random.Range(-scatterRadius, scatterRadius),
                0,
                Random.Range(-scatterRadius, scatterRadius)
            );

            StartCoroutine(ScatterAnimation(xp, target));
        }
    }

    IEnumerator ScatterAnimation(GameObject xp, Vector3 target)
    {
        float t = 0f;
        float duration = 0.25f;
        Vector3 start = xp.transform.position;

        while (t < 1f)
        {
            t += Time.deltaTime * scatterSpeed;

            xp.transform.position = Vector3.Lerp(start, target, t);

            yield return null;
        }
    }
}
