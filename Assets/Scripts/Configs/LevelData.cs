using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Configs/LevelData")]
public class LevelData : BaseScriptableObject
{
#if UNITY_EDITOR
    [MenuItem("Tools/Create Level Data")]
    public static void Create()
    {
        CreateAsset<LevelData>();
    }
#endif

    public int levelIndex;
    public int allFood;
    public int totalFood;
    public int totalGrill;
    public int levelTime;

}
