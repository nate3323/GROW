using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveCount : MonoBehaviour
{
    public TMP_Text _WaveCountText;

    void Update()
    {  
        if (WaveManager.Instance.IsWaveInProgress == false)
        {
            _WaveCountText.text = "" + WaveManager.Instance.WaveNumber;
        }
    }
}
