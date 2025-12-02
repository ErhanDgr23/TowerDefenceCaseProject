using System.Collections;
using UnityEngine;

public class PlayerControler : MonoBehaviour {

    public bool Dead;

    [SerializeField] GameObject PlayerShadow;
    [SerializeField] FixedJoystick Joystick;
    [SerializeField] float MoveSpeed;

    float HorizontalAxis, VerticalAxis;
    SpriteRenderer _spriteRenderer;
    GunController _gunController;
    CustomAnimator _animator;
    PlayerStats _playerStats;
    PlayerBase _playerBase;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _gunController = GetComponent<GunController>();
        _playerStats = GetComponent<PlayerStats>();
        _animator = GetComponent<CustomAnimator>();
        _playerBase = PlayerBase.instance;

        _playerBase.IFramePlayer += TakeDamage;
    }

    void Update()
    {
        if (Dead) return;

        _gunController.LookEnemy();
        Move();
    }

    void TakeDamage(int value)
    {
        _spriteRenderer.color = Color.green;
        StopCoroutine(WaitIFrameAndLeave());
        StartCoroutine(WaitIFrameAndLeave());
    }

    IEnumerator WaitIFrameAndLeave()
    {
        yield return new WaitForSeconds(1f);
        _spriteRenderer.color = Color.white;
        _playerBase.PlayerInIFrame = false;
    }

    public void Move()
    {
        if(PlayerShadow != null)
        PlayerShadow.transform.position = transform.position + new Vector3(-0.45f, -0.83f, 0f);

        if(Joystick == null)
        {
            HorizontalAxis = Input.GetAxis("Horizontal");
            VerticalAxis = Input.GetAxis("Vertical");
        }
        else
        {
            HorizontalAxis = Joystick.Horizontal;
            VerticalAxis = Joystick.Vertical;
        }

        transform.position += new Vector3(-VerticalAxis, 0f, HorizontalAxis) * MoveSpeed * Time.deltaTime;

        Vector3 pos = transform.position;
        pos.z = Mathf.Clamp(pos.z, -17f, 22f);
        pos.x = Mathf.Clamp(pos.x, -5f, 8.5f);
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