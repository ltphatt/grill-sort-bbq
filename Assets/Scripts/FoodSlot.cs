using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodSlot : MonoBehaviour
{
    [SerializeField] private Image foodImage;
    Color normalColor = new(1f, 1f, 1f, 1f);
    Color fadeColor = new(1f, 1f, 1f, 0.6f);

    GrillStation grillController;

    void Awake()
    {
        foodImage.gameObject.SetActive(false);
        grillController = this.transform.parent.parent.GetComponent<GrillStation>();
    }

    public void SetFoodSlot(Sprite food)
    {
        foodImage.gameObject.SetActive(true);
        foodImage.sprite = food;
        foodImage.SetNativeSize();
    }

    public bool HasFood()
    {
        return foodImage.gameObject.activeInHierarchy && foodImage.color == normalColor;
    }

    public Sprite GetSpriteFood()
    {
        return foodImage.sprite;
    }

    public void OnActiveFood(bool active)
    {
        foodImage.gameObject.SetActive(active);
        foodImage.color = normalColor;
    }

    public void OnFadeFood()
    {
        OnActiveFood(true);
        foodImage.color = fadeColor;
    }

    public void OnHideFood()
    {
        OnActiveFood(false);
        foodImage.color = normalColor;
    }

    public FoodSlot GetEmptySlot => grillController.GetEmptySlot();

    public void OnCheckMerge()
    {
        grillController?.OnCheckMerge();
    }
}
