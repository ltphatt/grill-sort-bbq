using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject grillTemplate;

    [SerializeField] int totalFood;
    [SerializeField] int totalGrill;
    [SerializeField] Transform gridGrill;
    List<GrillStation> grillStations;
    float avgTray;
    List<Sprite> totalSpritesFood;

    void Awake()
    {
        Sprite[] loadedSprites = Resources.LoadAll<Sprite>("Items");
        totalSpritesFood = new List<Sprite>(loadedSprites);
    }

    void Start()
    {
        InitLevel();
    }

    void InitLevel()
    {
        List<Sprite> takeFood = totalSpritesFood.OrderBy(x => Random.value).Take(totalFood).ToList();
        List<Sprite> useFood = new List<Sprite>();

        for (int i = 0; i < takeFood.Count; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                useFood.Add(takeFood[i]);
            }
        }

        for (int i = 0; i < useFood.Count; i++)
        {
            int rand = Random.Range(0, useFood.Count);
            Sprite temp = useFood[i];
            useFood[i] = useFood[rand];
            useFood[rand] = temp;
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
            int temp = result[i];
            result[i] = result[rand];
            result[rand] = temp;
        }

        return result;
    }

    void ClearGrill()
    {
        gridGrill.gameObject.GetComponentsInChildren<GrillStation>().ToList().ForEach(x => Destroy(x.gameObject));
        grillStations = new List<GrillStation>();
    }

}
