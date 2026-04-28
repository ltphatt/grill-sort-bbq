using UnityEngine;
using UnityEditor;
using System.IO;

public class BaseScriptableObject : ScriptableObject
{
#if UNITY_EDITOR
    public static void CreateAsset<T>(string folderPath = "Assets/Resources") where T : ScriptableObject
    {
        T asset = CreateInstance<T>();
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string fileName = typeof(T).Name;
        string fullPath = AssetDatabase.GenerateUniqueAssetPath($"{folderPath}/{fileName}.asset");

        AssetDatabase.CreateAsset(asset, fullPath);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

    public static void CreateAsset<T>() where T : ScriptableObject
    {
        string fullPath = EditorUtility.SaveFilePanel(
                  "Save " + typeof(T).Name,
                  "Assets",
                  "New " + typeof(T).Name,
                  "asset"
              );

        if (!string.IsNullOrEmpty(fullPath))
        {
            string relPath = FileUtil.GetProjectRelativePath(fullPath);
            if (string.IsNullOrEmpty(relPath))
            {
                EditorUtility.DisplayDialog("Error", "Please select a path within the Assets folder.", "OK");
                return;
            }

            T asset = CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, relPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }

#endif
}