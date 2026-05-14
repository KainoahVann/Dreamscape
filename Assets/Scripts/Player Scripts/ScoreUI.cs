using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public PlayerScore playerScore;
    public TMP_Text scoreText;

    void Update()
    {
        if (playerScore != null && scoreText != null)
        {
            scoreText.text = "Score: " + playerScore.score;
        }
    }
}