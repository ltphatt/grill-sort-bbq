using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodSlot : MonoBehaviour
{
    [SerializeField] private Image foodImage;

    void Awake()
    {
        foodImage.gameObject.SetActive(false);
    }

    public void SetFoodSlot(Sprite food)
    {
        foodImage.gameObject.SetActive(true);
        foodImage.sprite = food;
        foodImage.SetNativeSize();
    }

    public bool HasFood()
    {
        return foodImage.gameObject.activeInHierarchy;
    }
}
