using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class XRA3LevelManager : MonoBehaviour
{
    public static XRA3LevelManager Instance;

    public bool gameOver = false;

    [SerializeField] private int currentScore = 0;
    [SerializeField] private int defaultShapeScore = 50;
    [SerializeField] private int defaultBlockHitScore = 100;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private AudioSource musicSource;

    [SerializeField] private GameObject winCanvas;
    [SerializeField] private AudioClip winSFX;
    [SerializeField] private GameObject loseCanvas;
    [SerializeField] private AudioClip loseSFX;
    [SerializeField] XRA3ShapeSpawner spawnerScript;
    [SerializeField] private bool gameLost = false;

    public Transform SFXPlayerLocation;
    
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            throw new System.Exception("Can't be two Level Managers!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreText();
        gameLost = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver) return;

        if (gameLost)
        {
            gameOver = true;
            Lose();
        } else if (!musicSource.isPlaying)
        {
            gameOver = true;
            Win();
        }
    }

    private void UpdateScoreText()
    {
        if (gameLost)
        {
            scoreText.color = Color.red;
        }
        scoreText.text = currentScore.ToString();
    }

    private void Lose()
    {
        musicSource.Stop();
        AudioSource.PlayClipAtPoint(loseSFX, SFXPlayerLocation.position);
        spawnerScript.stopSpawning = true;
        loseCanvas.SetActive(true);
    }

    private void Win()
    {
        musicSource.Stop();
        AudioSource.PlayClipAtPoint(winSFX, SFXPlayerLocation.position);
        spawnerScript.stopSpawning = true;
        winCanvas.SetActive(true);
    }

    public void UpdateScoreFromBlockHit()
    {
        if (currentScore - defaultBlockHitScore < 0)
        {
            currentScore = 0;
            gameLost = true;
        }
        else
        {
            currentScore -= defaultBlockHitScore;
        }

        UpdateScoreText();
    }

    public void UpdateScore(float strength, bool goodHit)
    {
        float scoreModifier = strength * 10;
        
        if (goodHit)
        {
            currentScore += defaultShapeScore + (int) scoreModifier;
        } else
        {
            if (currentScore - defaultShapeScore < 0)
            {
                currentScore = 0;
                gameLost = true;
            } else
            {
                currentScore -= defaultShapeScore;
            }
        }

        UpdateScoreText();
    }
}
