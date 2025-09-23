using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public class SpriteAnimation
{
    public string name;
    public List<Sprite> frames;
    public float frameTime = 0.1f;
    public bool loop = true;
    public UnityEvent onAnimationEnd;
}

[RequireComponent(typeof(SpriteRenderer))]
public class CustomAnimator : MonoBehaviour
{
    public List<SpriteAnimation> animations = new List<SpriteAnimation>();
    public string CurrentPlayingAnimation;
    public bool flipX, StartWithIdle = true;

    private SpriteAnimation _currentAnimation;
    private SpriteRenderer _spriteRenderer;
    private Coroutine _animCoroutine;

    private void Awake() => _spriteRenderer = GetComponent<SpriteRenderer>();

    private void Start()
    {
        if (StartWithIdle)
            Play("Idle");
    }

    private void Update()
    {
        if (_spriteRenderer != null)
            _spriteRenderer.flipX = flipX;
    }

    public void Play(string animName)
    {
        if (!gameObject.activeSelf) return;

        CurrentPlayingAnimation = animName;

        if (_currentAnimation != null && _currentAnimation.name == animName) return;

        if (_animCoroutine != null)
            StopCoroutine(_animCoroutine);

        SpriteAnimation anim = animations.Find(a => a.name == animName);
        if (anim == null)
        {
            Debug.LogWarning("Animasyon bulunamadı: " + animName);
            return;
        }

        _currentAnimation = anim;
        _animCoroutine = StartCoroutine(PlayAnimation(anim));
    }

    public void StopAndPlay(string animName)
    {
        if (!gameObject.activeSelf) return;

        if (_animCoroutine != null)
        {
            StopCoroutine(_animCoroutine);
            _animCoroutine = null;
        }

        SpriteAnimation anim = animations.Find(a => a.name == animName);
        if (anim == null)
        {
            Debug.LogWarning("Animasyon bulunamadı: " + animName);
            return;
        }

        CurrentPlayingAnimation = animName;
        _currentAnimation = anim;
        _animCoroutine = StartCoroutine(PlayAnimation(anim));
    }

    private IEnumerator PlayAnimation(SpriteAnimation anim)
    {
        int frameIndex = 0;

        while (true)
        {
            _spriteRenderer.sprite = anim.frames[frameIndex];

            yield return new WaitForSeconds(anim.frameTime);

            frameIndex++;

            if (frameIndex >= anim.frames.Count)
            {
                if (anim.loop)
                {
                    frameIndex = 0;
                }
                else
                {
                    anim.onAnimationEnd?.Invoke();
                    yield break;
                }
            }
        }
    }
}
