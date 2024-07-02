using TMPro;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public int score { get; set; }
    public TextMeshProUGUI scoreText;
    void Awake()
    {
        scoreText = GetComponentInChildren<TextMeshProUGUI>();
    }
    void Start()
    {
        score = 0;
        scoreText.text = score.ToString();
    }
    public void SetScore(int score)
    {
        this.score = score;
        scoreText.text = this.score.ToString();
    }
    public int GetScore()
    {
        scoreText.text = score.ToString();
        return score;
    }
    public void IncrementScore()
    {
        score += 10;
        scoreText.text = score.ToString();
    }
    public int EndGameScore()
    {
        return score = 0;
    }
}
