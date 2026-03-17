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
    public List<FoodSlot> TotalSlots => totalSlots;
    Stack<Tray> trayStack = new();

    void Awake()
    {
        totalSlots = Utils.GetListInChild<FoodSlot>(slotContainer);
        totalTrays = Utils.GetListInChild<Tray>(trayContainer);
    }

    public void InitGrillStation(int totalTray, List<Sprite> listFood)
    {
        List<Sprite> foods = listFood;

        int minimumFoodForTrays = totalTray - 1;
        int maxFoodForFirstGrill = Mathf.Min(totalSlots.Count, foods.Count - minimumFoodForTrays);
        maxFoodForFirstGrill = Mathf.Max(1, maxFoodForFirstGrill);

        // Handle the first tray - grill
        int firstGrillFoodCount = Random.Range(1, maxFoodForFirstGrill + 1);
        List<Sprite> firstGrillFood = Utils.TakeAndRemoveRandom(foods, firstGrillFoodCount);
        // Debug.Log($"First grill food count: {firstGrillFoodCount}, foods left: {foods.Count}");

        for (int i = 0; i < firstGrillFoodCount; i++)
        {
            FoodSlot slot = GetRandomSlot();
            slot.SetFoodSlot(firstGrillFood[i]);
        }

        // Handle the remaining trays - waiting area
        List<List<Sprite>> remainFood = new();
        for (int i = 0; i < totalTray - 1; i++)
        {
            remainFood.Add(new List<Sprite>());
            if (foods.Count == 0) continue;

            int randomIndex = Random.Range(0, foods.Count);
            remainFood[i].Add(foods[randomIndex]);
            foods.RemoveAt(randomIndex);
        }

        // Distribute the remaining food items randomly into the trays, ensuring that no tray has more than 4 items
        while (foods.Count > 0)
        {
            int rand = Random.Range(0, remainFood.Count);
            if (remainFood[rand].Count < 4)
            {
                int n = Random.Range(0, foods.Count);
                remainFood[rand].Add(foods[n]);
                foods.RemoveAt(n);
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

    public Tray GetFirstTray()
    {
        if (trayStack.Count > 0)
        {
            return trayStack.Peek();
        }
        return null;
    }

    public List<Image> GetListFoodActive()
    {
        List<Image> result = new();
        for (int i = 0; i < totalSlots.Count; i++)
        {
            if (totalSlots[i].HasFood())
            {
                result.Add(totalSlots[i].FoodImage);
            }
        }

        for (int i = 0; i < totalTrays.Count; i++)
        {
            Tray tray = totalTrays[i];
            if (tray.gameObject.activeInHierarchy)
            {
                for (int j = 0; j < tray.FoodList.Count; j++)
                {
                    if (tray.FoodList[j].gameObject.activeInHierarchy)
                    {
                        result.Add(tray.FoodList[j]);
                    }
                }
            }
        }

        return result;
    }

    public void OnPlayShuffeVFX()
    {
        for (int i = 0; i < totalSlots.Count; i++)
        {
            if (totalSlots[i].HasFood())
            {
                totalSlots[i].OnShuffleFood();
            }
        }
    }
}
