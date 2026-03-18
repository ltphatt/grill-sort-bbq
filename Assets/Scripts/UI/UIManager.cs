using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI levelTxt;
    public TextMeshProUGUI timerTxt;
    public Image timerFill;

    [Header("Game Popups")]
    public GameObject pausePopup;

    int totalTime = 0;
    int timeRemaining = 0;

    void Awake()
    {
        pausePopup.SetActive(false);
    }

    void Start()
    {
        GameManager.Instance.OnUpdateLevel += UpdateLevel;
        UpdateLevel(LevelManager.Instance.currentLevel);

        totalTime = LevelManager.Instance.LevelDuration;
        timeRemaining = LevelManager.Instance.LevelDuration;
        StartCoroutine(StartCountdown());
    }

    void OnDestroy()
    {
        GameManager.Instance.OnUpdateLevel -= UpdateLevel;
        timerFill.DOKill();
    }

    public void UpdateLevel(int level)
    {
        timeRemaining = LevelManager.Instance.LevelDuration;
        levelTxt.text = $"Lv.{level}";
    }

    IEnumerator StartCountdown()
    {
        while (timeRemaining > 0)
        {
            timerTxt.text = Utils.ConvertToTime(timeRemaining);
            timerFill.DOFillAmount((float)timeRemaining / totalTime, 1f)
                .SetEase(Ease.Linear);

            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        timerTxt.text = Utils.ConvertToTime(0);
        timerFill.DOFillAmount(0, 0);
    }
}
