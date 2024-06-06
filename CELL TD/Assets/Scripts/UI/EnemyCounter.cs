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

    public TMP_Text enemyCountText;
    public int enemies = 0;
    private void Awake()
    {
        _waveEnemies = FindObjectOfType<WaveManager>();
        _waveEnemies.AnEnemyDied += EnemyDies;
        _waveEnemies.AnEnemyReachedGoal += EnemyReachesGoal;
        
    }
    void Update()
    {
        
    }

    void EnemyDies(object Sender, EventArgs a)
    {
       enemies = _waveEnemies.TotalEnemiesInWave - 1;
       enemyCountText.text = "" + enemies;
    }

    void EnemyReachesGoal(object Sender, EventArgs a)
    {
        enemies = _waveEnemies.TotalEnemiesInWave - 1;
        enemyCountText.text = "" + enemies;
    }

    void OnDestroy()
    {
        _waveEnemies.AnEnemyDied -= EnemyDies;
        _waveEnemies.AnEnemyReachedGoal -= EnemyReachesGoal;
    }
}
