using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public int currentLevel = 1;
    [SerializeField] int allFood;
    [SerializeField] int totalFood;
    [SerializeField] int totalGrill;
    public int LevelDuration = 120;

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        Observer.AddObserver(EventMessage.ON_COMPLETE_LEVEL, NextLevel);
    }

    void Start()
    {
        currentLevel = 1;
        GameManager.Instance.InitLevel(currentLevel, allFood, totalFood, totalGrill);
    }

    void OnDisable()
    {
        Observer.RemoveObserver(EventMessage.ON_COMPLETE_LEVEL, NextLevel);
    }

    public void NextLevel(object[] data)
    {
        currentLevel++;
    }

    public void PauseLevel()
    {
        Time.timeScale = 0f;
    }

    public void ResumeLevel()
    {
        Time.timeScale = 1f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        GameManager.Instance.InitLevel(currentLevel, allFood, totalFood, totalGrill);
    }

    public void ToNextLevel()
    {
        GameManager.Instance.InitLevel(currentLevel, allFood, totalFood, totalGrill);
    }
}
