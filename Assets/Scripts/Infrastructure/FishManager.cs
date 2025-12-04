using UnityEngine;
using System.Collections.Generic;

public class FishManager : MonoBehaviour
{
    [SerializeField] int fishCount = 10;
    [SerializeField] List<FishBase> fishPrefabs;
    List<FishBase> fishList = new List<FishBase>();

    private void Start()
    {
        InstatiateFishes();
    }

    void InstatiateFishes()
    {
        for (int i = 0; i < fishCount; i++)
        {
            FishBase newFish = Instantiate(fishPrefabs[Random.Range(0, fishPrefabs.Count)], Vector3.zero, Quaternion.identity, parent: this.transform);
            fishList.Add(newFish);
        }
    }
}
