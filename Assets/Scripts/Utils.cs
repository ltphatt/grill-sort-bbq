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
}