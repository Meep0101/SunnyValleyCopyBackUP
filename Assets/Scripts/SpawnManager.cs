using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    
    public GameObject itemPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 5f; //seconds
    private int spawnCount = 0;

    void Start()
    {
        StartCoroutine(SpawnItemsRoutine());
    }

    IEnumerator SpawnItemsRoutine()
    {
        while (true)
        {
            SpawnItem();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnItem()
    {
        if (itemPrefab != null && spawnPoint != null)
        {

            for(int i = 0; 1 <3; i++){
            Vector3 nextSpawnPoint = spawnPoint.position + new Vector3(1f, 1f, 2f);

            Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);
            spawnCount++;
            Debug.Log("passenger available " + spawnCount);
            }
        }
        else
        {
            Debug.LogError("Item Prefab or Spawn Point not assigned in the inspector!");
        }
    }
}
