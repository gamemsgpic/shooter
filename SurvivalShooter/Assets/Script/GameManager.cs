using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    public bool IsGameOver { get; private set; }
    public UIManager uiManager;

    public void Start()
    {
        uiManager.UpdateScoreText(score);
        uiManager.SetActiveGameOverPanel(false);

        var playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();

        playerHealth.onDeath += OnGameOver;
    }

    public void AddScore(int add)
    {
        if (IsGameOver)
        {
            return;
        }
        score += add;
        uiManager.UpdateScoreText(score);
    }

    public void OnGameOver()
    {
        IsGameOver = true;
        uiManager.SetActiveGameOverPanel(true);
    }
}
