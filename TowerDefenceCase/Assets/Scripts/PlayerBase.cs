using UnityEngine.UI;
using UnityEngine;
using System;

public class PlayerBase : MonoBehaviour{

    public static PlayerBase instance;

    public Action<int> OnDamage { get; set; }
    public Action<int> IFramePlayer { get; set; }

    public bool PlayerInIFrame;

    [SerializeField] Image HealthbarFill;

    int health = 100;

    private void Awake() => instance = this;

    private void Start() => OnDamage += TakeDamage;

    void TakeDamage(int value)
    {
        if (PlayerInIFrame) return;

        health -= value;
        HealthbarFill.fillAmount = health / 100f;
        PlayerInIFrame = true;
        IFramePlayer?.Invoke(1);

        if (health <= 0f)
            Application.LoadLevel(0);
    }
}
