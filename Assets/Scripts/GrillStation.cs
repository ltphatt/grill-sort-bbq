using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrillStation : MonoBehaviour
{
    [SerializeField] Transform slotContainer;
    [SerializeField] Transform trayContainer;
    List<Tray> totalTrays;
    List<FoodSlot> totalSlots;

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
}
