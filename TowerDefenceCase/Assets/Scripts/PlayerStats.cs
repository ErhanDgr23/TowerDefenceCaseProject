using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public static PlayerStats Instance;

    public int[] LvlThreshold;

    [Header("PlayerCounters")]
    public int PlayerKillSum;
    public int PlayerMoney;
    public int PlayerLevel;
    public int PlayerXp;

    [Header("PlayerBuff")]
    public float EkstraFireRate;  
    public float EkstraDamage;
    public float EkstraSpeed;

    PlayerControler _playerController;
    MenuManager _menuManager;

    private void Awake() => Instance = this;

    private void Start()
    {
        _playerController = GetComponent<PlayerControler>();
        _menuManager = MenuManager.instance;

        Invoke("UpdateLevelFunc", 0.25f);
    }

    void UpdateLevelFunc() => _menuManager.UpdateLevel(PlayerLevel, PlayerXp);

    public void ChangeStatsWithId(CardSO info, int lvl)
    {
        switch (info.Name)
        {
            case "Damage":
                EkstraDamage += info.CardLevels[Mathf.Min(info.CardLevels.Length - 1, lvl)].ValueChange;
                break;

            case "Speed":
                EkstraSpeed += info.CardLevels[Mathf.Min(info.CardLevels.Length - 1, lvl)].ValueChange;
                break;

            case "FireRate":
                EkstraFireRate += info.CardLevels[Mathf.Min(info.CardLevels.Length - 1, lvl)].ValueChange;
                break;
        }
    }
}
