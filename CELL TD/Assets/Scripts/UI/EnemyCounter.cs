using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System;

public class EnemyCounter : MonoBehaviour
{
    [SerializeField]
    private WaveManager _waveEnemies;
    
    public TextMeshProUGUI enemyCountText;
    public int enemies = 0;
    private void Awake()
    {
        _waveEnemies = FindObjectOfType<WaveManager>();
        _waveEnemies.AnEnemyDied += EnemyDies;
        _waveEnemies.AnEnemyReachedGoal += EnemyReachesGoal;
        
    }
    void Update()
    {
        while(_waveEnemies.WaveNumber <= _waveEnemies.TotalWavesInLevel)
        {
            EnemyReset();
        }
    }

    void EnemyDies(object Sender, EventArgs a)
    {
       enemies = _waveEnemies.TotalEnemiesInWave - _waveEnemies.TotalEnemiesKilledInLevel;
       enemyCountText.text = "" + enemies;
    }

    void EnemyReachesGoal(object Sender, EventArgs a)
    {
        enemies = _waveEnemies.TotalEnemiesInWave - _waveEnemies.TotalEnemiesReachedGoalInLevel;
        enemyCountText.text = "" + enemies;
    }

    void EnemyReset()
    {
        if (_waveEnemies.IsWaveInProgress)
        {
            EnemyDies(this, EventArgs.Empty);
            EnemyReachesGoal(this, EventArgs.Empty);
        }
    }
    void OnDestroy()
    {
        _waveEnemies.AnEnemyDied -= EnemyDies;
        _waveEnemies.AnEnemyReachedGoal -= EnemyReachesGoal;
    }
}
