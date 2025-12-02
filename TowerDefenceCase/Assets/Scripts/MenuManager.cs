using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour{

    public static MenuManager instance;

    public CardSelector CardSelectorSc;
    public PunchAnimation LvlHolder;
    public RectTransform ExpTarget;

    public Image XpFillImage;

    public TextMeshProUGUI KillCountText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI XpText;

    public float CurrentPassedTime;

    PlayerStats _playerStats;

    float levelThresold;
    bool _gameTime = true;

    private void Awake() => instance = this;

    private void Start() => _playerStats = PlayerStats.Instance;

    public void StartGameTime() => _gameTime = true;

    private void LateUpdate()
    {
        if (!_gameTime) return;

        CurrentPassedTime += Time.deltaTime;
        TimeText.text = string.Format("{0}:{1:00}", Mathf.FloorToInt(CurrentPassedTime / 60f) , Mathf.FloorToInt(CurrentPassedTime % 60f));
    }

    public void UpdateKill() => KillCountText.text = _playerStats.PlayerKillSum.ToString();

    public void UpdateLevel(float lvl, float xp)
    {
        LevelText.text = lvl.ToString();

        int thresholdIndex = _playerStats.PlayerLevel < _playerStats.LvlThreshold.Length
            ? _playerStats.PlayerLevel
            : _playerStats.LvlThreshold.Length - 1;

        levelThresold = _playerStats.LvlThreshold[thresholdIndex];

        bool leveledUp = false; // Level atladı mı kontrolü

        // --- LEVEL UP LOGIC ---
        while (_playerStats.PlayerXp >= levelThresold)
        {
            _playerStats.PlayerXp -= (int)levelThresold;
            _playerStats.PlayerLevel++;

            LvlHolder.Replay();

            int newIndex = _playerStats.PlayerLevel < _playerStats.LvlThreshold.Length
                ? _playerStats.PlayerLevel
                : _playerStats.LvlThreshold.Length - 1;

            levelThresold = _playerStats.LvlThreshold[newIndex];

            LevelText.text = _playerStats.PlayerLevel.ToString();

            leveledUp = true; // Level atladı
        }
        // --- END LEVEL UP LOGIC ---

        // Eğer level atladıysa CardSelector panelini aç
        if (leveledUp)
        {
            CardSelectorSc.OpenPanel();
        }

        // UI XP bar ve text güncelle
        XpFillImage.fillAmount = _playerStats.PlayerXp / levelThresold;
        XpText.text = "%" + Mathf.FloorToInt((_playerStats.PlayerXp / levelThresold) * 100f);
    }

}
