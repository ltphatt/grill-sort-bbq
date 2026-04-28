using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class EffectManager : MonoBehaviour
{
    [Header("Magnet Effect")]
    public GameObject magnetVFX;
    public Image magnetVFXImage;

    [Header("Timer Effect")]
    public GameObject timerVFX;
    public TextMeshProUGUI timerText;
    public float tweenDuration = 1.5f;
    public GameObject clock;
    private Vector3 timerOriginPos;

    void Awake()
    {
        magnetVFX.SetActive(false);
        timerVFX.SetActive(false);
    }

    void OnEnable()
    {
        Observer.Subscribe(EventMessage.ON_USE_BOOSTER_MAGNET, ShowMagnetEffect);
        Observer.Subscribe(EventMessage.ON_USE_BOOSTER_TIME, ShowTimerEffect);
    }

    void Start()
    {
        timerOriginPos = timerVFX.transform.position;
    }

    void OnDisable()
    {
        Observer.Unsubscribe(EventMessage.ON_USE_BOOSTER_MAGNET, ShowMagnetEffect);
        Observer.Unsubscribe(EventMessage.ON_USE_BOOSTER_TIME, ShowTimerEffect);
    }

    void ShowMagnetEffect(object[] data)
    {
        magnetVFX.SetActive(true);

        magnetVFXImage.transform.DOScale(1.25f, 0.5f)
            .SetEase(Ease.OutBack)
            .SetLoops(5, LoopType.Yoyo)
            .OnComplete(() =>
            {
                magnetVFX.SetActive(false);
                magnetVFXImage.transform.localScale = Vector3.one;
            });
    }

    void ShowTimerEffect(object[] data)
    {
        int seconds = (int)data[0];
        timerText.text = $"+{seconds}s";

        timerVFX.transform.position = timerOriginPos;
        timerVFX.SetActive(true);
        AudioManager.Instance.PlaySFX("TIME_BONUS");

        timerVFX.transform.DOMove(clock.transform.position, tweenDuration)
            .OnComplete(() =>
            {
                timerVFX.SetActive(false);
            });
    }
}
