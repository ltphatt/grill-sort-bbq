using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    [SerializeField] GameObject grillTemplate;

    [SerializeField] int allFood;
    [SerializeField] int totalFood;
    [SerializeField] int totalGrill;
    [SerializeField] Transform gridGrill;
    List<GrillStation> grillStations;
    float avgTray;
    List<Sprite> totalSpritesFood;
    const int MAX_FOOD_PER_TRAY = 3;

    void Awake()
    {
        Sprite[] loadedSprites = Resources.LoadAll<Sprite>("Items");
        totalSpritesFood = new List<Sprite>(loadedSprites);
        instance = this;
    }

    void Start()
    {
        InitLevel();
    }

    void InitLevel()
    {
        List<Sprite> takeFood = totalSpritesFood.OrderBy(x => Random.value).Take(totalFood).ToList();
        List<Sprite> useFood = new();

        for (int i = 0; i < allFood; i++)
        {
            int randomFoodIndex = i % takeFood.Count;
            for (int j = 0; j < MAX_FOOD_PER_TRAY; j++)
            {
                useFood.Add(takeFood[randomFoodIndex]);
            }
        }

        for (int i = 0; i < useFood.Count; i++)
        {
            int rand = Random.Range(0, useFood.Count);
            (useFood[i], useFood[rand]) = (useFood[rand], useFood[i]);
        }

        avgTray = Random.Range(1.5f, 2f);

        // Calculate the total number of trays needed based on the average number of food items per tray
        int totalTray = Mathf.RoundToInt(useFood.Count / avgTray);
        List<int> trayPerGrill = DistributeEvenly(totalGrill, totalTray);
        List<int> foodPerGrill = DistributeEvenly(totalGrill, useFood.Count);

        ClearGrill();

        for (int i = 0; i < totalGrill; i++)
        {
            GrillStation grill = Instantiate(grillTemplate, gridGrill).GetComponent<GrillStation>();
            List<Sprite> listFood = Utils.TakeAndRemoveRandom(useFood, foodPerGrill[i]);
            grill.InitGrillStation(trayPerGrill[i], listFood);
            grillStations.Add(grill);
        }
    }

    List<int> DistributeEvenly(int grillCount, int totalTrays)
    {
        List<int> result = new();

        float avg = (float)totalTrays / grillCount;
        int low = Mathf.FloorToInt(avg);
        int high = Mathf.CeilToInt(avg);

        int highCount = totalTrays - (low * grillCount);
        int lowCount = grillCount - highCount;

        // Add the low trays to the result list
        for (int i = 0; i < lowCount; i++)
        {
            result.Add(low);
        }

        // Add the remaining trays to the result list
        for (int i = 0; i < highCount; i++)
        {
            result.Add(high);
        }

        // Shuffle the result list to randomize the distribution of trays among grills
        for (int i = 0; i < result.Count; i++)
        {
            int rand = Random.Range(0, result.Count);
            (result[rand], result[i]) = (result[i], result[rand]);
        }

        return result;
    }

    void ClearGrill()
    {
        gridGrill.gameObject.GetComponentsInChildren<GrillStation>().ToList().ForEach(x => Destroy(x.gameObject));
        grillStations = new List<GrillStation>();
    }

    public void OnMinusFood()
    {
        allFood--;
        if (allFood <= 0)
        {
            Debug.Log("You win!");
        }
    }

    public void OnCheckShake()
    {
        Dictionary<string, List<FoodSlot>> groupedFood = new();

        // Create a dict to group the food name with the list of food slot
        foreach (var grill in grillStations)
        {
            for (int i = 0; i < grill.TotalSlots.Count; i++)
            {
                FoodSlot slot = grill.TotalSlots[i];
                if (slot.HasFood())
                {
                    string name = slot.GetSpriteFood().name;
                    if (!groupedFood.ContainsKey(name))
                    {
                        groupedFood.Add(name, new List<FoodSlot>());
                    }
                    groupedFood[name].Add(slot);
                }
            }
        }

        foreach (var group in groupedFood)
        {
            if (group.Value.Count >= 3)
            {
                Utils.ShuffleList(group.Value);

                for (int i = 0; i < 3; i++)
                {
                    group.Value[i].ShakeFood();
                }
                break;
            }
        }
    }

}
