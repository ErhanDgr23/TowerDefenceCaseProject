using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour {

    public bool Dead;

    [SerializeField] float MoveSpeed;

    float HorizontalAxis, VerticalAxis;
    GunController _gunController;
    CustomAnimator _animator;

    private void Start()
    {
        _gunController = GetComponent<GunController>();
        _animator = GetComponent<CustomAnimator>();
    }

    void Update()
    {
        if (Dead) return;

        _gunController.LookEnemy();
        Move();
    }

    public void Move()
    {
        HorizontalAxis = Input.GetAxis("Horizontal");
        VerticalAxis = Input.GetAxis("Vertical");

        transform.position += new Vector3(-VerticalAxis, 0f, HorizontalAxis) * MoveSpeed * Time.deltaTime;

        Vector3 pos = transform.position;
        pos.z = Mathf.Clamp(pos.z, -13.5f, 13.5f);
        pos.x = Mathf.Clamp(pos.x, -6.5f, 11f);
        transform.position = pos;

        if (VerticalAxis != 0f || HorizontalAxis != 0f)
        {
            _animator.flipX = HorizontalAxis > 0 ? false : true;

            if (_animator.CurrentPlayingAnimation != "Run")
                _animator.Play("Run");
        }
        else if (_animator.CurrentPlayingAnimation != "Idle")
        {
            _animator.Play("Idle");
        }
    }
}