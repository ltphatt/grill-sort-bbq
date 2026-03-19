using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DropDragControl : MonoBehaviour
{
    [SerializeField] float suggestionTime = 10f;
    [SerializeField] Image imageFoodDrag;
    FoodSlot currentFood;
    FoodSlot cacheFood;
    bool hasDrag = false;
    Vector3 offset;

    float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= suggestionTime)
        {
            timer = 0f;
            GameManager.Instance.OnCheckShake();
        }

        // Handle when player click on the food item
        if (Input.GetMouseButtonDown(0))
        {
            currentFood = Utils.GetRayCastUI<FoodSlot>(Input.mousePosition);

            if (currentFood != null && currentFood.HasFood())
            {
                Debug.Log("Start Drag: " + currentFood.name);
                AudioManager.Instance.PlaySFX("PICK_UP_FOOD");

                hasDrag = true;
                cacheFood = currentFood;

                imageFoodDrag.gameObject.SetActive(true);
                imageFoodDrag.sprite = currentFood.GetSpriteFood();
                imageFoodDrag.SetNativeSize();
                imageFoodDrag.transform.position = currentFood.transform.position;

                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                offset = mouseWorldPos - currentFood.transform.position;

                currentFood.OnActiveFood(false);
            }
        }

        // Handle while player is dragging the food item
        if (hasDrag)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 foodPos = mouseWorldPos - offset;
            foodPos.z = 0f;
            imageFoodDrag.transform.position = foodPos;

            var slot = Utils.GetRayCastUI<FoodSlot>(Input.mousePosition);
            if (slot != null)
            {
                if (!slot.HasFood())
                {
                    if (cacheFood == null || cacheFood.GetInstanceID() != slot.GetInstanceID())
                    {
                        cacheFood?.OnHideFood();
                        cacheFood = slot;
                        cacheFood.OnFadeFood();
                        cacheFood.SetFoodSlot(currentFood.GetSpriteFood());
                    }
                }
                else
                {
                    FoodSlot emptySlot = slot.GetEmptySlot;
                    if (emptySlot != null)
                    {
                        cacheFood?.OnHideFood();
                        cacheFood = emptySlot;
                        cacheFood.OnFadeFood();
                        cacheFood.SetFoodSlot(currentFood.GetSpriteFood());
                    }
                    else
                    {
                        ClearCacheFood();
                    }
                }
            }
            else
            {
                ClearCacheFood();
            }
        }

        // Handle when player release the food item
        if (Input.GetMouseButtonUp(0) && hasDrag)
        {
            if (cacheFood != null)
            {
                imageFoodDrag.transform.DOMove(cacheFood.transform.position, 0.15f).OnComplete(() =>
                {
                    AudioManager.Instance.PlaySFX("SMOKE");

                    imageFoodDrag.gameObject.SetActive(false);
                    cacheFood.SetFoodSlot(currentFood.GetSpriteFood());
                    cacheFood.OnActiveFood(true);

                    cacheFood.OnCheckMerge();
                    currentFood?.OnCheckPrepareTray();

                    cacheFood = null;
                    currentFood = null;
                });
            }
            else
            {
                imageFoodDrag.transform.DOMove(currentFood.transform.position, .3f).OnComplete(() =>
                {
                    imageFoodDrag.gameObject.SetActive(false);
                    currentFood.OnActiveFood(true);
                    currentFood = null;
                });
            }

            hasDrag = false;
        }
    }

    void ClearCacheFood()
    {
        if (cacheFood != null)
        {
            cacheFood.OnHideFood();
            cacheFood = null;
        }
    }
}
