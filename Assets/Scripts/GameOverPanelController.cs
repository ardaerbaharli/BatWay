using TMPro;
using UnityEngine;

public class GameOverPanelController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    private void Awake()
    {
        var score = GameManager.instance.score;
        currentScoreText.text = $"Score: {score}";

        var bestScore = PlayerPrefs.GetInt("BestScore", 0);
        bestScoreText.text = $"Best: {bestScore}";
    }


    public void OnMainMenuButtonClicked()
    {
        GameManager.instance.LoadMainMenu();
    }
}