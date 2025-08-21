using UnityEngine;
using TMPro;

public class ScoreManagerTMP : MonoBehaviour
{
    public static ScoreManagerTMP Instance { get; private set; }

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private string prefix = "Score: ";

    public int Score { get; private set; }
    private const int DumpPoints = 100;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        Refresh();
    }

    private void OnEnable()  { TrashCan.OnAnyEmptied += HandleEmptied; }
    private void OnDisable() { TrashCan.OnAnyEmptied -= HandleEmptied; }

    private void HandleEmptied(TrashCan _)
    {
        Add(DumpPoints);
    }

    public void Add(int amount)
    {
        Score += amount;
        Refresh();
    }

    private void Refresh()
    {
        if (scoreText != null)
            scoreText.text = prefix + Score;
    }
}
