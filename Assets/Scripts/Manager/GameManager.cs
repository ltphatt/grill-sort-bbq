using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] GameObject grillTemplate;
    [SerializeField] Transform gridGrill;
    List<GrillStation> grillStations = new();
    float avgTray;
    List<Sprite> totalSpritesFood;
    const int MAX_FOOD_PER_TRAY = 3;
    int allFood;
    Dictionary<string, List<FoodSlot>> groupedFood = new();

    void Awake()
    {
        Instance = this;

        Sprite[] loadedSprites = Resources.LoadAll<Sprite>("Items");
        totalSpritesFood = new List<Sprite>(loadedSprites);
    }

    void Start()
    {
        Observer.Subscribe(EventMessage.ON_USE_BOOSTER_MAGNET, OnUseMagnet);
        Observer.Subscribe(EventMessage.ON_USE_BOOSTER_SHUFFLE, OnShuffle);
    }

    void OnDestroy()
    {
        Observer.Unsubscribe(EventMessage.ON_USE_BOOSTER_MAGNET, OnUseMagnet);
        Observer.Unsubscribe(EventMessage.ON_USE_BOOSTER_SHUFFLE, OnShuffle);
    }

    public void InitLevel(LevelData levelData)
    {
        allFood = levelData.allFood;

        List<Sprite> takeFood = totalSpritesFood.OrderBy(x => Random.value).Take(levelData.totalFood).ToList();
        List<Sprite> useFood = new();

        for (int i = 0; i < allFood; i++)
        {
            int randomFoodIndex = i % takeFood.Count;
            for (int j = 0; j < MAX_FOOD_PER_TRAY; j++)
            {
                useFood.Add(takeFood[randomFoodIndex]);
            }
        }

        // Shuffle useFood list
        Utils.ShuffleList(useFood);
        avgTray = Random.Range(1.5f, 2f);

        // Calculate the total number of trays needed based on the average number of food items per tray
        int totalTray = Mathf.RoundToInt(useFood.Count / avgTray);
        List<int> foodPerGrill = DistributeEvenly(levelData.totalGrill, useFood.Count);
        List<int> trayPerGrill = new();
        for (int i = 0; i < levelData.totalGrill; i++)
        {
            int neededTrays = Mathf.CeilToInt(foodPerGrill[i] / avgTray);
            neededTrays = Mathf.Max(1, neededTrays);
            trayPerGrill.Add(neededTrays);
        }

        ClearGrill();

        for (int i = 0; i < levelData.totalGrill; i++)
        {
            GameObject grillObject = Instantiate(grillTemplate, gridGrill);
            grillObject.name = $"Grill_{i}";

            GrillStation grill = grillObject.GetComponent<GrillStation>();
            List<Sprite> listFood = Utils.TakeAndRemoveRandom(useFood, foodPerGrill[i]);
            grill.InitGrillStation(trayPerGrill[i], listFood);
            grillStations.Add(grill);
        }

        Observer.Notify(EventMessage.ON_UPDATE_LEVEL, levelData.levelIndex);
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
        Observer.Notify(EventMessage.ON_MERGE_FOOD);

        if (allFood <= 0)
        {
            allFood = 0;
            Observer.Notify(EventMessage.ON_COMPLETE_LEVEL);
        }
    }

    public void OnCheckShake()
    {
        foreach (var kvp in groupedFood)
        {
            kvp.Value.Clear();
        }

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

    void OnUseMagnet(object[] data)
    {
        Dictionary<string, List<Image>> groups = new();

        // Get all food images in the grill and the first tray of each
        foreach (var grill in grillStations)
        {
            // Add images of slots to the dict
            for (int i = 0; i < grill.TotalSlots.Count; i++)
            {
                FoodSlot slot = grill.TotalSlots[i];
                if (slot.HasFood())
                {
                    string name = slot.GetSpriteFood().name;
                    if (!groups.ContainsKey(name))
                    {
                        groups.Add(name, new List<Image>());
                    }
                    groups[name].Add(slot.FoodImage);
                }
            }

            Tray tray = grill.GetFirstTray();
            if (tray != null)
            {
                for (int i = 0; i < tray.FoodList.Count; i++)
                {
                    Image img = tray.FoodList[i];
                    if (img.gameObject.activeInHierarchy)
                    {
                        string name = img.sprite.name;
                        if (!groups.ContainsKey(name))
                        {
                            groups.Add(name, new List<Image>());
                        }
                        groups[name].Add(img);
                    }
                }
            }
        }

        // Clear all food in the group that has 3 or more items
        foreach (var kvp in groups)
        {
            if (kvp.Value.Count >= 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    Image imgFood = kvp.Value[i];
                    imgFood.gameObject.SetActive(false);
                    imgFood.gameObject.SendMessageUpwards("OnCheckPrepareTray");
                }

                break;
            }
        }
    }

    void OnShuffle(object[] data)
    {
        StartCoroutine(IEShuffle());

        IEnumerator IEShuffle()
        {
            List<Image> results = new();

            // Get all food images in the grill and in the trays
            foreach (var grill in grillStations)
            {
                results.AddRange(grill.GetListFoodActive());
                grill.OnPlayShuffeVFX();
            }

            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < results.Count; i++)
            {
                int rand = Random.Range(0, results.Count);

                // Swap the sprites of the two images
                (results[rand].sprite, results[i].sprite) = (results[i].sprite, results[rand].sprite);

                // Set native size
                results[i].SetNativeSize();
                results[rand].SetNativeSize();
            }
        }
    }
}
