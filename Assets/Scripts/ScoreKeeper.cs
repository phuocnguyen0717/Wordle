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
        scoreText.text = "Score \n" + score.ToString();
        LoadScoreFromFirebase();
    }
    public void SetScore(int score)
    {
        this.score = score;
        scoreText.text = this.score.ToString();
        SaveScoreToFirebase(score);
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
        SaveScoreToFirebase(score);
    }
    private void SaveScoreToFirebase(int score)
    {
        FirebaseManager.Instance.SaveScore(score);
    }
    private void LoadScoreFromFirebase()
    {
        FirebaseManager.Instance.LoadScore(loadedScore =>
        {
            score = loadedScore;
            scoreText.text = score.ToString();
        });
    }
}
