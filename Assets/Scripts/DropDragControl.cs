using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDragControl : MonoBehaviour
{
    [SerializeField] Image imageFoodDrag;

    FoodSlot currentFood;
    bool hasDrag = false;
    Vector3 offset;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentFood = Utils.GetRayCastUI<FoodSlot>(Input.mousePosition);

            if (currentFood != null && currentFood.HasFood())
            {
                hasDrag = true;

                imageFoodDrag.gameObject.SetActive(true);
                imageFoodDrag.sprite = currentFood.GetSpriteFood();
                imageFoodDrag.SetNativeSize();
                imageFoodDrag.transform.position = currentFood.transform.position;

                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                offset = mouseWorldPos - currentFood.transform.position;

                currentFood.OnActiveFood(false);
            }
        }

        if (hasDrag)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 foodPos = mouseWorldPos - offset;
            foodPos.z = 0f;
            imageFoodDrag.transform.position = foodPos;
        }

        if (Input.GetMouseButtonUp(0) && hasDrag)
        {
            imageFoodDrag.gameObject.SetActive(false);
            currentFood.OnActiveFood(true);
            currentFood = null;
            hasDrag = false;
        }
    }
}
