using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public static T GetRayCastUI<T>(Vector2 pos) where T : MonoBehaviour
    {
        PointerEventData pointerEventData = new(EventSystem.current)
        {
            position = pos
        };

        List<RaycastResult> list = new();
        EventSystem.current.RaycastAll(pointerEventData, list);

        if (list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                T component = list[i].gameObject.GetComponent<T>();

                if (component != null)
                {
                    return component;
                }
            }
        }

        return null;
    }

    public static void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    public static string ConvertToTime(int time)
    {
        float min = Mathf.FloorToInt(time / 60);
        float sec = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", min, sec);
    }
}