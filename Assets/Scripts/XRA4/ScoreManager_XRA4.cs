using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager_XRA4 : MonoBehaviour
{
    public static ScoreManager_XRA4 Instance;

    [SerializeField] private TextMeshProUGUI currScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private string bestScoreSaveName = "best_score";

    public bool timerStarted = false;
    public bool timerEnded = false;

    private int currScore = 0;
    private int bestScore = 0;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        } else
        {
            Debug.LogError("Cannot be two score managers!");
            Destroy(this);
        }

        currScore = 0;
        currScoreText.text = currScore.ToString();
        bestScore = LoadBestScore();
        bestScoreText.text = bestScore.ToString();
        timerStarted = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (currScoreText == null || bestScoreText == null)
        {
            Debug.LogError("Need to set the curr score or best score text!");
        }
        timerEnded = false;
    }

    public void UpdateCurrentScore(int dartValue)
    {
        Debug.Log($"reached update current score");
        if (timerStarted && !timerEnded)
        {
            Debug.Log($"Adding {dartValue} to score in score manager");
            currScore += dartValue;
            currScoreText.text = currScore.ToString();
        }
    }

    public void ResetScore()
    {
        DeleteBestScore();
        currScore = 0;
        currScoreText.text = currScore.ToString();
        bestScore = LoadBestScore();
        bestScoreText.text = bestScore.ToString();
        timerStarted = false;
        timerEnded = false;
    }

    private int LoadBestScore()
    {
        if (PlayerPrefs.HasKey(bestScoreSaveName))
        {
            return PlayerPrefs.GetInt(bestScoreSaveName);
        } else
        {
            return 0;
        }
    }

    private void DeleteBestScore()
    {
        PlayerPrefs.DeleteKey(bestScoreSaveName);
    }

    public void SaveBestScore()
    {
        PlayerPrefs.SetInt(bestScoreSaveName, currScore);
        bestScoreText.text = currScore.ToString();
    }


    public bool DidPlayerWin()
    {
        return currScore > bestScore;
    }
}
