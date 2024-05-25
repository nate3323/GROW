using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class EnemySpawner : MonoBehaviour
{
    [Header("Wave Settings")]
    [Tooltip("A list of Wave scriptable Objects that have a list of enemies to spawn")]
    [SerializeField]
    private List<Wave> _Waves;

    [Tooltip("The time between enemies spawning")]
    [SerializeField]
    [Min(1f)]
    private float _TimeBetweenSpawns;

    [Header("Game Object References")]
    [SerializeField, Tooltip("One possible spawn point for enemies")]
    private Transform _SpawnPoint1;


    private int _CurrentWave;

    private Transform _SpawnedEnemiesParent;


    private void Awake()
    {
        _CurrentWave = 0;

        _SpawnedEnemiesParent = GameObject.Find("Spawned Enemies Parent").transform;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartNextWave()
    {       
        StartCoroutine(Spawn(_CurrentWave++));
    }
    public void StopSpawner()
    {
        StartCoroutine(Spawn(_CurrentWave));
    }
    IEnumerator Spawn(int currentWave)
    {
        int currentEnemyType = 0;
        EnemyTypes type = _Waves[currentWave].Enemies[currentEnemyType].EnemyInfo.Type;
        int enemiesOfCurrentType = _Waves[currentWave].Enemies[currentEnemyType].NumberToSpawn;
        int totalEnemies = EnemiesInCurrentWave();
        GameObject enemy = null;
        
        for (int i = 0; i < totalEnemies; i++)
        {
            if( enemiesOfCurrentType == 0 ) {
                type = _Waves[currentWave].Enemies[currentEnemyType].EnemyInfo.Type;
                enemiesOfCurrentType = _Waves[currentWave].Enemies[currentEnemyType].NumberToSpawn;
            }

            enemy = Instantiate(_Waves[currentWave].Enemies[currentEnemyType].EnemyInfo.Prefab, _SpawnedEnemiesParent);


            enemiesOfCurrentType--;
            if(enemiesOfCurrentType == 0)
            {
                currentEnemyType++;
            }

            yield return new WaitForSeconds(_TimeBetweenSpawns);
        }

    }


    public int EnemiesInCurrentWave()
    {
        Wave current = _Waves[_CurrentWave - 1];
        int tally = 0;
        foreach(EnemySpawnInfo cats in current.Enemies)
        {
            tally += cats.NumberToSpawn;
        }
        return tally;
    }



    public int NumberOfWaves { get { return _Waves.Count; } }
}
