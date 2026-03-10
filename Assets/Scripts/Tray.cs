using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tray : MonoBehaviour
{
    private List<Image> foodList;

    void Awake()
    {
        foodList = Utils.GetListInChild<Image>(transform);
        foreach (var food in foodList)
        {
            food.gameObject.SetActive(false);
        }
    }

    public void SetFoods(List<Sprite> items)
    {
        if (items.Count > foodList.Count)
        {
            return;
        }

        for (int i = 0; i < items.Count; i++)
        {
            Image slot = RandomSlot();
            slot.sprite = items[i];
            slot.gameObject.SetActive(true);
            slot.SetNativeSize();
        }
    }

    Image RandomSlot()
    {
        int randomIndex = Random.Range(0, foodList.Count);
        while (foodList[randomIndex].gameObject.activeInHierarchy)
        {
            randomIndex = Random.Range(0, foodList.Count);
        }
        return foodList[randomIndex];
    }

}
