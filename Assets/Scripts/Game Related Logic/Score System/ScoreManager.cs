using UnityEngine;
using TMPro;
using System;

public class ScoreManagerTMP : MonoBehaviour
{
    public static ScoreManagerTMP Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TMP_Text scoreText;   // assign in Inspector
    [SerializeField] private string prefix = "Score: ";

    public int Score { get; private set; }

    // NEW: anyone can subscribe to score updates
    public event Action<int> OnScoreChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        Refresh();
    }

    public void Add(int amount)
    {
        Score += amount;
        Refresh();
        OnScoreChanged?.Invoke(Score);   // notify
    }

    public void ResetScore()
    {
        Score = 0;
        Refresh();
        OnScoreChanged?.Invoke(Score);   // notify
    }

    private void Refresh()
    {
        if (scoreText != null) scoreText.text = prefix + Score;
    }
}
