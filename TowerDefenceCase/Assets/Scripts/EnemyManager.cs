using UnityEngine.UI;
using UnityEngine;
using System;

public interface IEnemy
{
    public void Damage(int val);
}

public class EnemyManager : MonoBehaviour, IEnemy {

    public event Action<GameObject> OnDead;
    public EnemyTypeSO Type;
    public bool IsDead;

    [SerializeField] Image HealthBarSlider;

    SpriteRenderer _spriteRenderer;
    CustomAnimator _enemyAnim;
    EnemyMove _enemyMove;
    PathGizmo _pathGizmo;

    CustomAnimator _bleedAnim;
    int _enemyMoveIndex = -1;
    int Health;

    UnityEngine.Events.UnityAction _runEndHandler;

    private void Awake()
    {
        _bleedAnim = transform.GetChild(1).GetComponent<CustomAnimator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyAnim = GetComponent<CustomAnimator>();
        _enemyMove = GetComponent<EnemyMove>();

        InitEnemy();
    }

    public void InitEnemy()
    {
        transform.tag = "Enemy";
        IsDead = false;
        Health = (int)Type.Health;
        HealthBarSlider.fillAmount = Health / 100f;

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
        _enemyAnim.animations[3].onAnimationEnd.AddListener(_runEndHandler);
    }

    private void OnRunAnimationEnd()
    {
        _enemyAnim.Play("Run");
        _enemyMove.Stop = false;
    }

    void Start()
    {
        _pathGizmo = PathGizmo.Instance;

        if (_enemyMove.Targettr == null)
            ChangeEnemyTarget();

        _enemyMove.ReachTheEnd += ChangeEnemyTarget;
    }

    void ChangeEnemyTarget()
    {
        if (IsDead) return;

        _enemyMoveIndex++;

        if (_pathGizmo.pathPoints.Count > _enemyMoveIndex)
            _enemyMove.Targettr = _pathGizmo.pathPoints[_enemyMoveIndex];
        else
            EnemyHitPlayerBase();
    }

    void EnemyHitPlayerBase()
    {

    }

    public void Damage(int val)
    {
        if (IsDead) return;

        _bleedAnim.StopAndPlay("Bleed");
        _enemyAnim.StopAndPlay("Hit");
        _enemyMove.Stop = true;
        Health -= val;
        HealthBarSlider.fillAmount = Health / 100f;
        CheckHealth();
    }

    void CheckHealth()
    {
        if (IsDead) return;

        if (Health <= 0f)
        {
            if (_runEndHandler != null)
                _enemyAnim.animations[3].onAnimationEnd.RemoveListener(_runEndHandler);

            _enemyMove.Stop = true;
            IsDead = true;
            _enemyAnim.StopAndPlay("Dead");
            transform.tag = "Untagged";
            _enemyAnim.enabled = false;
            OnDead?.Invoke(this.gameObject);
        }
    }

    void Update()
    {
        
    }
}
