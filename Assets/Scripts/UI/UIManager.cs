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

    [Header("Game Canvas")]
    public Canvas resultCanvas;

    int totalTime = 0;
    int timeRemaining = 0;

    void Awake()
    {
        resultCanvas.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        Observer.Subscribe(EventMessage.ON_UPDATE_LEVEL, UpdateLevel);
        Observer.Subscribe(EventMessage.ON_COMPLETE_LEVEL, ShowResultPopup);
        Observer.Subscribe(EventMessage.ON_USE_BOOSTER_TIME, AddLevelTime);
    }

    void Start()
    {
        totalTime = GameController.Instance.GetLevelTime();
        timeRemaining = GameController.Instance.GetLevelTime();
        StartCoroutine(StartCountdown());
    }

    void OnDisable()
    {
        Observer.Unsubscribe(EventMessage.ON_UPDATE_LEVEL, UpdateLevel);
        Observer.Unsubscribe(EventMessage.ON_COMPLETE_LEVEL, ShowResultPopup);
        Observer.Unsubscribe(EventMessage.ON_USE_BOOSTER_TIME, AddLevelTime);
    }

    void OnDestroy()
    {
        timerFill.DOKill();
    }

    public void UpdateLevel(object[] data)
    {
        int level = (int)data[0];
        timeRemaining = GameController.Instance.GetLevelTime();
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

    void AddLevelTime(object[] data)
    {
        int seconds = (int)data[0];

        timeRemaining = Mathf.Clamp(timeRemaining + seconds, 0, totalTime);
        timerTxt.text = Utils.ConvertToTime(timeRemaining);
        timerFill.DOFillAmount((float)timeRemaining / totalTime, 0.5f)
            .SetEase(Ease.Linear);
    }

    void ShowResultPopup(object[] data)
    {
        resultCanvas.gameObject.SetActive(true);
    }
}
