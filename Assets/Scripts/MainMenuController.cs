using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private GameObject gamePlayPanel;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    private void Awake()
    {
        bestScoreText.text = "Best Score: " + PlayerPrefs.GetInt("BestScore", 0);
    }

    private void OnPointerDown()
    {
        GameManager.instance.StartGame();
        gamePlayPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _inputHandler.onPointerDown_ -= OnPointerDown;
    }

    private void OnEnable()
    {
        _inputHandler.onPointerDown_ += OnPointerDown;
    }
}