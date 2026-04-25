using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FoodSlot : MonoBehaviour
{
    [SerializeField] private Image foodImage;
    public Image FoodImage => foodImage;
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

    public void OnPrepareItem(Image food)
    {
        SetFoodSlot(food.sprite);
        foodImage.color = normalColor;
        foodImage.transform.position = food.transform.position;
        foodImage.transform.localScale = food.transform.localScale;
        foodImage.transform.localEulerAngles = food.transform.localEulerAngles;

        foodImage.transform.DOLocalMove(Vector3.zero, 0.2f);
        foodImage.transform.DOScale(Vector3.one, 0.2f);
        foodImage.transform.DOLocalRotate(Vector3.zero, 0.2f);
    }

    public void OnCheckPrepareTray()
    {
        grillController?.OnCheckPrepareTray();
    }

    public void ShakeFood()
    {
        foodImage.transform.DOShakePosition(0.5f, 10f, 10, 180);
    }

    public void OnShuffleFood()
    {
        foodImage.DOFade(0f, 0.5f).SetLoops(2, LoopType.Yoyo);
    }
}
