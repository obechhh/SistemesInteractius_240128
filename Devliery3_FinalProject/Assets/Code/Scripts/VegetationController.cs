using System.Collections.Generic;
using UnityEngine;

public class VegetationController : MonoBehaviour
{
    public List<GameObject> arbres = new List<GameObject>();
    public List<GameObject> flowers = new List<GameObject>();
    public List<GameObject> grass = new List<GameObject>();
    public List<GameObject> plants = new List<GameObject>();

    private int totalBrossa;
    private int initialArbresCount = 1;
    private int initialFlowersCount = 3;
    private int initialGrassCount = 20;
    private int initialPlantsCount = 10;

    private GamestateManager gamestateManager;

    void Start()
    {
        // Find and populate vegetation lists
        PopulateVegetationList("arbres", arbres);
        PopulateVegetationList("flowers", flowers);
        PopulateVegetationList("grass", grass);
        PopulateVegetationList("plants", plants);

        //totalBrossa = FindObjectOfType<PlayerInteraction>().totalItems;
        gamestateManager = GamestateManager.Instance;
        //totalBrossa = gamestateManager.totalItems;

        // Set initial visibility counts for each vegetation type
        SetInitialVisibility();
    }

    void PopulateVegetationList(string tag, List<GameObject> list)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

        Debug.Log("Found " + objects.Length + " objects with tag: " + tag);

        foreach (GameObject obj in objects)
        {
            list.Add(obj);
        }
    }

    void SetInitialVisibility()
    {
        SetVisibility(arbres, initialArbresCount);
        SetVisibility(flowers, initialFlowersCount);
        SetVisibility(grass, initialGrassCount);
        SetVisibility(plants, initialPlantsCount);
    }

    public void UpdateVegetationVisibility()
    {
        Debug.Log("Updating vegetation visibility...");

        // Check if gamestateManager is null
        if (gamestateManager == null)
        {
            Debug.LogError("GamestateManager is not initialized.");
            return;
        }

        // Check the values of totalItems and remainingItems
        Debug.Log("Total items: " + gamestateManager.totalItems);
        Debug.Log("Remaining items: " + gamestateManager.remainingItems);

        int collectedBrossa = gamestateManager.totalItems - gamestateManager.remainingItems;

        // Example of increasing visibility counts differently for each vegetation type
        int visibleArbres = initialArbresCount + 2 * collectedBrossa;
        int visibleFlowers = initialFlowersCount + 2 * collectedBrossa; // Increase flowers faster
        int visibleGrass = initialGrassCount + collectedBrossa; // Increase grass slower
        int visiblePlants = initialPlantsCount + collectedBrossa; // Increase plants same rate as arbres

        Debug.Log("Visible Arbres: " + visibleArbres);
        Debug.Log("Visible Flowers: " + visibleFlowers);
        Debug.Log("Visible Grass: " + visibleGrass);
        Debug.Log("Visible Plants: " + visiblePlants);

        SetVisibility(arbres, visibleArbres);
        SetVisibility(flowers, visibleFlowers);
        SetVisibility(grass, visibleGrass);
        SetVisibility(plants, visiblePlants);
    }


    void SetVisibility(List<GameObject> list, int visibleCount)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (i < visibleCount)
            {
                list[i].SetActive(true);
            }
        }
    }
}