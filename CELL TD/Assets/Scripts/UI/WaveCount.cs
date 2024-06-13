using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System;

public class WaveCount : MonoBehaviour
{
    [SerializeField]
    private WaveManager _waveCount;
    
    public TextMeshProUGUI waveCountText;
    public int currentWave = 0;


    private void Awake()
    {
        _waveCount = FindObjectOfType<WaveManager>();
        _waveCount.WaveEnded += WaveNumber;
    }
    void Update()
    {
        //currentWave = _waveCount.WaveNumber;
        //waveCountText.text = "" + currentWave;
        WaveNumber(this, EventArgs.Empty);
        
    }

    void WaveNumber(object Sender, EventArgs a)
    {
        currentWave = _waveCount.WaveNumber;
        waveCountText.text = "" + currentWave;      
    }

    void OnDestroy()
    {
        _waveCount.WaveEnded -= WaveNumber;
    }
}
