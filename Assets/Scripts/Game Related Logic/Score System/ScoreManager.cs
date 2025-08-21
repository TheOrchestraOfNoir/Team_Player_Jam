using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
using TMPro;
using System;
=======
>>>>>>> parent of 4ffe70a (Score System Logic Established)

public class ScoreManager : MonoBehaviour
{
<<<<<<< HEAD
    public static ScoreManagerTMP Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TMP_Text scoreText;   // assign in Inspector
    [SerializeField] private string prefix = "Score: ";

    public int Score { get; private set; }

    // NEW: anyone can subscribe to score updates
    public event Action<int> OnScoreChanged;

    private void Awake()
=======
    // Start is called before the first frame update
    void Start()
>>>>>>> parent of 4ffe70a (Score System Logic Established)
=======

public class ScoreManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
>>>>>>> parent of 4ffe70a (Score System Logic Established)
=======

public class ScoreManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
>>>>>>> parent of 4ffe70a (Score System Logic Established)
    {
        
    }

<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
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
=======
=======
>>>>>>> parent of 4ffe70a (Score System Logic Established)
=======
>>>>>>> parent of 4ffe70a (Score System Logic Established)
    // Update is called once per frame
    void Update()
    {
        
<<<<<<< HEAD
<<<<<<< HEAD
>>>>>>> parent of 4ffe70a (Score System Logic Established)
=======
>>>>>>> parent of 4ffe70a (Score System Logic Established)
=======
>>>>>>> parent of 4ffe70a (Score System Logic Established)
    }
}
