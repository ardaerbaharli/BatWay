using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public int score;

    public float speed;
    public bool isGameStarted;
    public static GameManager instance;
    public bool areSafeZonesMoving;

    public delegate void OnScore(int score);

    public event OnScore onScore;

    public delegate void OnGameOver();

    public event OnGameOver onGameOver;

    public delegate void OnGameStarted();

    public event OnGameStarted onGameStarted;

    public delegate void OnNewEnemySpawned(Transform enemyTransform);

    public event OnNewEnemySpawned onNewEnemySpawned;

    public delegate void OnPause();

    public event OnPause onPause;

    public delegate void OnResume();

    public event OnResume onResume;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // fix fps and vsync
        Application.targetFrameRate = 30;
        QualitySettings.vSyncCount = 0;
        score = 0;
        speed = 70f;
    }

    public void GameOver()
    {
        ObjectPool.instance.ClearPool();
        Time.timeScale = 0;
        var bestScore = PlayerPrefs.GetInt("BestScore");
        if (score > bestScore)
            PlayerPrefs.SetInt("BestScore", score);
        onGameOver?.Invoke();
    }


    public void Score()
    {
        score++;
        onScore?.Invoke(score);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        onPause?.Invoke();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        onResume?.Invoke();
    }

    public void StartGame()
    {
        score = 0;
        StartCoroutine(StartGameC());
    }


    private IEnumerator StartGameC()
    {
        Time.timeScale = 1;
        yield return new WaitForSeconds(0.4f);
        isGameStarted = true;
        onGameStarted?.Invoke();
    }

    public void SpawnEnemy(Transform enemyTransform)
    {
        onNewEnemySpawned?.Invoke(enemyTransform);
    }

    public void LoadMainMenu()
    {
        isGameStarted = false;
        SceneManager.LoadScene("Main");
        Time.timeScale = 1;
    }
}