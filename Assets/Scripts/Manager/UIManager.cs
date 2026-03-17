using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI levelTxt;
    public TextMeshProUGUI timerTxt;

    int timeRemaining = 0;

    void Start()
    {
        GameManager.Instance.OnUpdateLevel += UpdateLevel;
        UpdateLevel(LevelManager.Instance.currentLevel);

        timeRemaining = LevelManager.Instance.LevelDuration;
        StartCoroutine(StartCountdown());
    }

    void OnDestroy()
    {
        GameManager.Instance.OnUpdateLevel -= UpdateLevel;
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
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }
    }
}
