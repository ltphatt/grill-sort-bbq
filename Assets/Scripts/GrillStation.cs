using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrillStation : MonoBehaviour
{
    [SerializeField] Transform slotContainer;
    [SerializeField] Transform trayContainer;
    List<Tray> totalTrays;
    List<FoodSlot> totalSlots;
    Stack<Tray> trayStack = new();

    void Awake()
    {
        totalSlots = Utils.GetListInChild<FoodSlot>(slotContainer);
        totalTrays = Utils.GetListInChild<Tray>(trayContainer);
    }

    public void InitGrillStation(int totalTray, List<Sprite> listFood)
    {
        // Handle the first tray - grill
        int firstGrillFoodCount = Random.Range(1, totalSlots.Count + 1);
        List<Sprite> foods = listFood;
        List<Sprite> firstGrillFood = Utils.TakeAndRemoveRandom(foods, firstGrillFoodCount);

        for (int i = 0; i < firstGrillFood.Count; i++)
        {
            FoodSlot slot = GetRandomSlot();
            slot.SetFoodSlot(firstGrillFood[i]);
        }

        // Handle the remaining trays - waiting area
        List<List<Sprite>> remainFood = new();

        for (int i = 0; i < totalTray - 1; i++)
        {
            remainFood.Add(new List<Sprite>());
            int n = Random.Range(0, listFood.Count);
            remainFood[i].Add(listFood[n]);
            listFood.RemoveAt(n);
        }

        // Distribute the remaining food items randomly into the trays, ensuring that no tray has more than 4 items
        while (listFood.Count > 0)
        {
            int rand = Random.Range(0, remainFood.Count);
            if (remainFood[rand].Count < 4)
            {
                int n = Random.Range(0, listFood.Count);
                remainFood[rand].Add(listFood[n]);
                listFood.RemoveAt(n);
            }
        }

        for (int i = 0; i < totalTrays.Count; i++)
        {
            bool active = i < remainFood.Count;
            totalTrays[i].gameObject.SetActive(active);

            if (active)
            {
                totalTrays[i].SetFoods(remainFood[i]);
                Tray tray = totalTrays[i];
                trayStack.Push(tray);
            }
        }
    }

    FoodSlot GetRandomSlot()
    {
        int n = Random.Range(0, totalSlots.Count);
        while (totalSlots[n].HasFood())
        {
            n = Random.Range(0, totalSlots.Count);
        }
        return totalSlots[n];
    }

    public FoodSlot GetEmptySlot()
    {
        for (int i = 0; i < totalSlots.Count; i++)
        {
            if (!totalSlots[i].HasFood())
            {
                return totalSlots[i];
            }
        }
        return null;
    }

    public void OnCheckMerge()
    {
        if (GetEmptySlot() == null)
        {
            if (CheckCanMerge())
            {
                Debug.Log("Merge success");
                foreach (var slot in totalSlots)
                {
                    slot.OnActiveFood(false);
                }

                OnPrepareTray();
                GameManager.Instance.OnMinusFood();
            }
        }
    }

    public void OnCheckPrepareTray()
    {
        if (IsEmptyGrill())
        {
            OnPrepareTray();
        }
    }

    void OnPrepareTray()
    {
        if (trayStack.Count > 0)
        {
            Tray item = trayStack.Pop();
            for (int i = 0; i < item.FoodList.Count; i++)
            {
                Image img = item.FoodList[i];
                if (img.gameObject.activeInHierarchy)
                {
                    totalSlots[i].OnPrepareItem(img);
                    img.gameObject.SetActive(false);
                }
            }

            item.gameObject.SetActive(false);
        }
    }

    bool CheckCanMerge()
    {
        string name = totalSlots[0].GetSpriteFood().name;

        for (int i = 1; i < totalSlots.Count; i++)
        {
            if (totalSlots[i].GetSpriteFood().name != name)
            {
                return false;
            }
        }
        return true;
    }

    bool IsEmptyGrill()
    {
        for (int i = 0; i < totalSlots.Count; i++)
        {
            if (totalSlots[i].HasFood())
            {
                return false;
            }
        }
        return true;
    }
}
