using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private GameObject gameplayPanel;
    

    private void Awake()
    {
        var score = GameManager.instance.score;
        currentScoreText.text = $"{score}";
    }


    public void OnResumeButtonClicked()
    {
        gameObject.SetActive(false);
        gameplayPanel.SetActive(true);
        GameManager.instance.ResumeGame();
    }

    public void OnMainMenuButtonClicked()
    {
        GameManager.instance.LoadMainMenu();
    }
}