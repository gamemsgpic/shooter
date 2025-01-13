using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor;

public class UIManager : MonoBehaviour
{
    public static readonly string scoreformat = "SCORE: {0}";

    public TextMeshProUGUI scoreText;

    public GameObject gameOverPanel;

    public float speed = 1.0f;
    private Image image;
    private Color color = Color.black;

    private void Awake()
    {
        color = gameOverPanel.GetComponent<Image>().color;
        color.a = 1f;
    }

    private void Update()
    {
        color.a *= speed * Time.deltaTime;
    }

    public void UpdateScoreText(int newScore)
    {
        scoreText.text = string.Format(scoreformat, newScore);
    }

    public void SetActiveGameOverPanel(bool active)
    {
        gameOverPanel.SetActive(active);
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
