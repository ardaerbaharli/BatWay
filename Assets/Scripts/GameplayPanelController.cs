using TMPro;
using UnityEngine;

public class GameplayPanelController : MonoBehaviour
{
    [SerializeField] private ToggleSwitch soundToggle;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI currentScore;


    private void Start()
    {
        soundToggle.valueChanged += SoundToggleValueChanged;
        var soundValue = PlayerPrefs.GetInt("Sound", 1) == 1;
        soundToggle.Toggle(soundValue);
    }

    private void SoundToggleValueChanged(bool value)
    {
        SoundManager.instance.SetSound(value);
    }

    private void Awake()
    {
        GameManager.instance.onScore += OnScoreChanged;
        GameManager.instance.onGameOver += OnGameOver;
        currentScore.text = "0";
    }

    private void OnGameOver()
    {
        gameOverPanel.SetActive(true);
        
        Destroy(soundToggle.gameObject);
        Destroy(currentScore.gameObject);
        Destroy(gameObject);
    }

    private void OnScoreChanged(int score)
    {
        currentScore.text = $"{score}";
    }

    public void OnPauseButtonClicked()
    {
        pauseMenu.SetActive(true);
        gameObject.SetActive(false);
        GameManager.instance.PauseGame();
    }

    private void OnDestroy()
    {
        GameManager.instance.onScore -= OnScoreChanged;
        GameManager.instance.onGameOver -= OnGameOver;
    }
}