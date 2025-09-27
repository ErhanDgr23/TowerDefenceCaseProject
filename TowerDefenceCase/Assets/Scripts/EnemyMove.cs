using Random = UnityEngine.Random;
using UnityEngine;
using System;

public class EnemyMove : MonoBehaviour
{
    public event Action PathComplete;

    public bool Stop;
    public Transform Targettr;
    public float MoveSpeed;

    private EnemyManager _manager;
    private CustomAnimator _animator;
    private Vector3 _dir, _rdmTargetOffset;
    private float _dist;

    public int _currentPathIndex = 0;
    [HideInInspector] public Transform[] _pathPoints;

    public void SetPath(Transform[] pathPoints)
    {
        _pathPoints = pathPoints;
        _currentPathIndex = 0;

        if (_pathPoints.Length > 0)
            Targettr = _pathPoints[_currentPathIndex];
    }

    public void TakeThisPath(int pathIndex, Transform[] pathPoints)
    {
        _currentPathIndex = pathIndex;
        _pathPoints = pathPoints;

        if (pathIndex >= pathPoints.Length)
            pathIndex = pathPoints.Length - 1;

        if (_pathPoints.Length > 0)
            Targettr = _pathPoints[_currentPathIndex];
    }

    private void Start()
    {
        _animator = GetComponent<CustomAnimator>();
        _manager = GetComponent<EnemyManager>();

        _rdmTargetOffset.x += Random.Range(-0.35f, 0.35f);
        _rdmTargetOffset.z += Random.Range(-0.35f, 0.35f);
    }

    void LateUpdate()
    {
        if (Stop || _manager.IsDead || Targettr == null || _pathPoints == null || _pathPoints.Length == 0)
            return;

        if (_animator.CurrentPlayingAnimation != "Run" && _animator.CurrentPlayingAnimation != "Dead")
            _animator.Play("Run");

        _dir = (Targettr.position - transform.position).normalized;
        _dir.y = 0f;
        _dir += _rdmTargetOffset;
        transform.position += _dir * MoveSpeed * Time.deltaTime;

        _dist = Vector3.Distance(new Vector3(Targettr.position.x, transform.position.y, Targettr.position.z), transform.position);

        if (_dist < 0.5f)
        {
            _currentPathIndex++;
            if (_currentPathIndex < _pathPoints.Length)
            {
                Targettr = _pathPoints[_currentPathIndex];
            }
            else
            {
                Stop = true;
                PathComplete?.Invoke();
            }
        }
    }
}
