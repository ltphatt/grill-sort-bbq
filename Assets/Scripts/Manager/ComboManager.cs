using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    int currentCombo = 0;
    public GameObject UICombo;
    public Image comboFillImage;
    public TextMeshProUGUI comboText;
    public float comboDuration = 5f;
    float comboTimer = 0f;

    void Awake()
    {
        UICombo.SetActive(false);
    }

    void OnEnable()
    {
        Observer.Subscribe(EventMessage.ON_MERGE_FOOD, OnMergeFood);
    }

    void OnDisable()
    {
        Observer.Unsubscribe(EventMessage.ON_MERGE_FOOD, OnMergeFood);
    }

    void Update()
    {
        if (currentCombo > 0)
        {
            comboTimer -= Time.deltaTime;
            comboFillImage.fillAmount = comboTimer / comboDuration;

            if (comboTimer <= 0f)
            {
                ResetCombo();
            }
        }
    }

    void OnMergeFood(object[] data)
    {
        currentCombo++;
        comboTimer = comboDuration;
        comboText.text = $"Combo x{currentCombo}";
        UICombo.SetActive(true);
    }

    void ResetCombo()
    {
        currentCombo = 0;
        UICombo.SetActive(false);
    }
}
