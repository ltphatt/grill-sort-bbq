using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI levelTxt;

    void Start()
    {
        GameManager.Instance.OnUpdateLevel += UpdateLevel;
        UpdateLevel(LevelManager.Instance.currentLevel);
    }

    void OnDestroy()
    {
        GameManager.Instance.OnUpdateLevel -= UpdateLevel;
    }

    public void UpdateLevel(int level)
    {
        levelTxt.text = $"Lv.{level}";
    }
}
