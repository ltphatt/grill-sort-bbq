using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "LevelChain", menuName = "Configs/LevelChain")]
public class LevelChain : BaseScriptableObject
{
#if UNITY_EDITOR
    [MenuItem("Tools/Create Level Chain")]
    public static void Create()
    {
        CreateAsset<LevelChain>();
    }
#endif

    public List<LevelData> levels;

    public LevelData GetLevelData(int levelIndex)
    {
        if (levelIndex <= 0)
        {
            return levels[0];
        }

        if (levelIndex >= levels.Count)
        {
            Debug.LogWarning($"Level index {levelIndex} is out of range!");
            return levels[levels.Count - 1];
        }

        return levels[levelIndex - 1];
    }
}
