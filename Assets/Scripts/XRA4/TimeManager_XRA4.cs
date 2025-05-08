using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager_XRA4 : MonoBehaviour
{
    public static TimeManager_XRA4 Instance;

    [SerializeField] private AudioClip winSFX;
    [SerializeField] private AudioClip loseSFX;

    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private int timer = 60;
    [SerializeField] private int timeRemaining = 0;

    private AudioSource _audioSource;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Cannot be two time managers!");
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (timerText == null)
        {
            Debug.LogError("Need to set the timer text!");
        }
        _audioSource = GetComponentInChildren<AudioSource>();
        timeRemaining = timer;
        timerText.text = $"{timeRemaining / 60:0}:{timeRemaining % 60:00}";
    }

    public void StartTimer()
    {
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (timeRemaining > 0)
        {
            timeRemaining--;
            timerText.text = $"{timeRemaining / 60:0}:{timeRemaining % 60:00}";
            yield return new WaitForSeconds(1f);
        }
        OnTimerEnded();
    }

    public void ResetTimer()
    {
        timeRemaining = 0;
        timeRemaining = timer;
        timerText.text = $"{timeRemaining / 60:0}:{timeRemaining % 60:00}";
    }

    private void OnTimerEnded()
    {
        Debug.Log("Timer ended!");
        ScoreManager_XRA4.Instance.timerEnded = true;
        HelperUI_XRA4.Instance.OnTimerEnded();
        
        if (ScoreManager_XRA4.Instance.DidPlayerWin())
        {
            _audioSource.PlayOneShot(winSFX);
            ScoreManager_XRA4.Instance.SaveBestScore();
        } else
        {
            _audioSource.PlayOneShot(loseSFX);
        }
    }
}
