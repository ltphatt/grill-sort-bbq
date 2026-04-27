using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public int currentLevel = 1;
    public LevelChain levels;

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        Observer.Subscribe(EventMessage.ON_COMPLETE_LEVEL, NextLevel);
    }

    void OnDisable()
    {
        Observer.Unsubscribe(EventMessage.ON_COMPLETE_LEVEL, NextLevel);
    }

    public void NextLevel(object[] data)
    {
        currentLevel++;
    }

    public void RestartLevel()
    {
        GameManager.Instance.InitLevel(levels.GetLevelData(currentLevel));
    }

    public void ToNextLevel()
    {
        GameManager.Instance.InitLevel(levels.GetLevelData(currentLevel));
    }

    public int GetLevelTime()
    {
        return levels.GetLevelData(currentLevel).levelTime;
    }
}
