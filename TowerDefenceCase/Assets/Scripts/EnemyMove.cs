using Random = UnityEngine.Random;
using UnityEngine;
using System;

public class EnemyMove : MonoBehaviour {

    public event Action ReachTheEnd;

    public bool Stop;
    public Transform Targettr;
    public float MoveSpeed;

    private EnemyManager _manager;
    private CustomAnimator _animator;
    private Vector3 _dir, _rdmTargetOffset;
    private float _dist;

    private void Start()
    {
        _animator = GetComponent<CustomAnimator>();
        _manager = GetComponent<EnemyManager>();

        _rdmTargetOffset.x += Random.Range(-0.35f, 0.35f);
        _rdmTargetOffset.z += Random.Range(-0.35f, 0.35f);
    }

    void LateUpdate()
    {
        if (Stop || _manager.IsDead || Targettr == null)
            return;

        if(_animator.CurrentPlayingAnimation != "Run" && _animator.CurrentPlayingAnimation != "Dead")
            _animator.Play("Run");

        _dir = (Targettr.position - transform.position).normalized;
        _dir.y = 0f;
        _dir += _rdmTargetOffset;
        transform.position += _dir * MoveSpeed * Time.deltaTime;
        _dist = Vector3.Distance(new Vector3(Targettr.position.x, transform.position.y, Targettr.position.z), transform.position);

        if (_dist < 1.5f)
        {
            ReachTheEnd?.Invoke();
            Debug.Log("<Color=red>EnemyMove</Color> EnemyTargetChanged");
        }
    }
}
