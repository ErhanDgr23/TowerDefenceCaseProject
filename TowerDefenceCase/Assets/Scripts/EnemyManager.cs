using Random = UnityEngine.Random;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;
using Unity.VisualScripting;

public interface IEnemy
{
    public void Damage(int val);
}

public class EnemyManager : MonoBehaviour, IEnemy {

    public event Action<GameObject, bool> OnDead;
    public EnemyTypeSO Type;
    public bool IsDead, SpawnedFromBoss = false;

    [SerializeField] Image HealthBarSlider;

    SpriteRenderer _spriteRenderer;
    CustomAnimator _enemyAnim;
    EnemyMove _enemyMove;
    PathGizmo _pathGizmo;
    ObjectPooler _pooler;

    CustomAnimator _bleedAnim;
    int health, maxHealth;

    private Coroutine bossCoroutine;

    UnityEngine.Events.UnityAction _runEndHandler;

    private void Awake()
    {
        _bleedAnim = transform.GetChild(1).GetComponent<CustomAnimator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyAnim = GetComponent<CustomAnimator>();
        _enemyMove = GetComponent<EnemyMove>();

        transform.AddComponent<EnemyXPDropper>();

        InitEnemy();
    }

    public void InitEnemy()
    {
        _enemyAnim.enabled = true;
        transform.tag = "Enemy";
        IsDead = false;
        health = (int)Type.Health;
        maxHealth = (int)Type.Health;
        HealthBarSlider.fillAmount = (float)health / (float)maxHealth;
        HealthBarSlider.transform.parent.gameObject.SetActive(true);

        transform.rotation = Quaternion.Euler(45f, -90f, 0f);
        transform.localScale = new Vector3(2f, 2f, 2f);
        _enemyAnim.animations.Clear();

        for (int i = 0; i < Type.RunAnimationSprites.Length; i++)
            _enemyAnim.animations.Add(Type.RunAnimationSprites[i]);

        _enemyMove.MoveSpeed = Type.Speed;

        _spriteRenderer.sprite = Type.EnemySprite;

        if (_runEndHandler != null)
            _enemyAnim.animations[3].onAnimationEnd.RemoveListener(_runEndHandler);

        _runEndHandler = OnRunAnimationEnd;
        _enemyAnim.animations[3].onAnimationEnd.AddListener(this.OnRunAnimationEnd);

        if (Type is EnemyTypeSpawnerBossSO bossType && bossType.IsBoss)
        {
            if (bossCoroutine != null) StopCoroutine(bossCoroutine);
            bossCoroutine = StartCoroutine(BossSpawnRoutine(bossType));
        }
    }

    private IEnumerator BossSpawnRoutine(EnemyTypeSpawnerBossSO bossType)
    {
        while (!IsDead)
        {
            yield return new WaitForSeconds(bossType.SpawnInterval);

            for (int i = 0; i < bossType.SpawnCount; i++)
            {
                Vector3 offset = new Vector3(
                    Random.Range(-1f, 1f),
                    0f,
                    Random.Range(-1f, 1f)
                );

                GameObject minion = _pooler.SpawnFromPool(bossType.MinionPrefab, transform.position + offset, Quaternion.identity);
                EnemyManager minionManager = minion.GetComponent<EnemyManager>();
                minionManager.SpawnedFromBoss = true;
                minionManager._enemyMove.TakeThisPath(_enemyMove._currentPathIndex, _enemyMove._pathPoints);
                minionManager.Type = bossType.MinionType;
                if (minionManager != null)
                {
                    minionManager.InitEnemy();
                }
            }
        }
    }

    private void OnRunAnimationEnd()
    {
        _enemyAnim.Play("Run");
        _enemyMove.Stop = false;
    }

    void Start()
    {
        _pooler = ObjectPooler.Instance;
        _pathGizmo = PathGizmo.Instance;

        if (_pathGizmo != null && !SpawnedFromBoss)
            _enemyMove.SetPath(_pathGizmo.pathPoints.ToArray());

        _enemyMove.PathComplete += EnemyHitPlayerBase;
    }

    void EnemyHitPlayerBase()
    {
        PlayerBase.instance.OnDamage?.Invoke(Type.Damage);
        health = 0;
        CheckHealth();
    }

    public void Damage(int val)
    {
        if (IsDead) return;

        _bleedAnim.StopAndPlay("Bleed");
        _enemyAnim.StopAndPlay("Hit");
        _enemyMove.Stop = true;
        health -= val;
        HealthBarSlider.fillAmount = (float)health / (float)maxHealth;
        CheckHealth();
    }

    void CheckHealth()
    {
        if (IsDead) return;

        if (health <= 0f)
        {
            _enemyAnim.StopAndPlay("Dead");
            _enemyAnim.canPlayAnimation = false;

            if (_runEndHandler != null)
                _enemyAnim.animations[3].onAnimationEnd.RemoveListener(_runEndHandler);

            if (bossCoroutine != null) StopCoroutine(bossCoroutine);

            HealthBarSlider.transform.parent.gameObject.SetActive(false);
            _enemyMove.Stop = true;
            IsDead = true;
            transform.tag = "Untagged";
            //_enemyAnim.enabled = false;
            OnDead?.Invoke(this.gameObject, SpawnedFromBoss);

            if (SpawnedFromBoss)
                _pooler.ReturnToPool(this.gameObject, 1f);
        }
    }
}
