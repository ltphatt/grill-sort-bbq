using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static List<T> GetListInChild<T>(Transform parent)
    {
        List<T> result = new();

        for (int i = 0; i < parent.childCount; i++)
        {
            var component = parent.GetChild(i).GetComponent<T>();
            if (component != null)
            {
                result.Add(component);
            }
        }

        return result;
    }

    public static List<T> TakeAndRemoveRandom<T>(List<T> source, int n)
    {
        List<T> result = new();

        n = Mathf.Min(n, source.Count);
        for (int i = 0; i < n; i++)
        {
            int randIndex = Random.Range(0, source.Count);
            result.Add(source[randIndex]);
            source.RemoveAt(randIndex);
        }

        return result;
    }
}