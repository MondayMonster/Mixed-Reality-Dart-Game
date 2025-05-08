using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HelperUI_XRA4 : MonoBehaviour
{
    public static HelperUI_XRA4 Instance;

    [SerializeField] private GameObject helperUICanvas;
    [SerializeField] private GameObject startGameButton;
    [SerializeField] private GameObject resetButton;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject resetBodyText;

    [SerializeField] private Vector3 originalDartboardPosition;

    [SerializeField] DartSpawner_XRA4 _dartSpawner;
    [SerializeField] CustomSpatialAnchor_XRA4 _dartBoardAnchorScript;
    [SerializeField] private CustomSpatialAnchor_XRA4 _helperUIAnchorScript;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Cannot be two helper ui managers!");
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        helperUICanvas.SetActive(true);
        startGameButton.SetActive(true);
        resetButton.SetActive(false);
        restartButton.SetActive(false);
        resetBodyText.SetActive(false);
        _helperUIAnchorScript = GetComponent<CustomSpatialAnchor_XRA4>();
    }

    public void OnTimerEnded()
    {
        _dartSpawner.keepSpawning = false;
        helperUICanvas.SetActive(true);
        startGameButton.SetActive(false);
        resetButton.SetActive(true);
        restartButton.SetActive(true);
    }

    public void OnStartGameButtonPressed()
    {
        Debug.Log("Start button pressed!");
        ScoreManager_XRA4.Instance.timerStarted = true;
        TimeManager_XRA4.Instance.ResetTimer();
        TimeManager_XRA4.Instance.StartTimer();
        helperUICanvas.SetActive(false);

    }

    public void OnResetButtonPressed()
    {
        Debug.Log("Reset button pressed!");
        resetBodyText.SetActive(true);
        ScoreManager_XRA4.Instance.ResetScore();
        TimeManager_XRA4.Instance.ResetTimer();
        _dartBoardAnchorScript.DeleteAnchor();
        _helperUIAnchorScript.DeleteAnchor();
    }

    public void OnRestartButtonPressed()
    {
        Debug.Log("Restart button pressed!");
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
