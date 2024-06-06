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

    public TMP_Text WaveCountText;

    private void Awake()
    {
        _waveCount = FindObjectOfType<WaveManager>();
        _waveCount.WaveEnded += WaveNumber;
    }
    void Update()
    {
        
    }

    void WaveNumber(object Sender, EventArgs a)
    {
        WaveCountText.text = "" + WaveManager.Instance.WaveNumber;
    }

    void OnDestroy()
    {
        _waveCount.WaveEnded -= WaveNumber;
    }
}
