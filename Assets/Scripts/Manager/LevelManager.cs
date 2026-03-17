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

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameManager.Instance.OnCompleteLevel += NextLevel;

        currentLevel = 1;
        GameManager.Instance.InitLevel(currentLevel, allFood, totalFood, totalGrill);
    }

    public void NextLevel()
    {
        currentLevel++;
        GameManager.Instance.InitLevel(currentLevel, allFood, totalFood, totalGrill);
    }

    void OnDestroy()
    {
        GameManager.Instance.OnCompleteLevel -= NextLevel;
    }
}
