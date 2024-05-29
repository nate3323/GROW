using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Macrophage_UnitSpawnerTower : Tower_Base
{
    [Header("Spawned Unit Information")]
    [SerializeField, Tooltip("The prefab of the units to spawn")]
    private GameObject unitPrefab;

    [SerializeField, Tooltip("Maximum number of units to spawn")]
    private int maxUnits;
    private int currentUnits;

    [SerializeField, Tooltip("Unit spawn point")]
    private GameObject spawnPoint;



    IEnumerator Spawner()
    {
    
        if (currentUnits >= maxUnits)
        {
            StopCoroutine(Spawner());
        }
        else if (currentUnits < maxUnits)
        {
            GameObject newPerson = Instantiate(unitPrefab, spawnPoint.transform.position, Quaternion.identity, gameObject.transform);    
            currentUnits++;
        }
        yield return new WaitForSeconds(FireRate);
        StartCoroutine(Spawner());

    }



}