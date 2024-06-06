using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class EnemyCounter : MonoBehaviour
{
    public TMP_Text enemyCountText;

    void Update()
    {
        enemyCountText.text = "" + WaveManager.Instance.TotalEnemiesInWave;
    }
}
