using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveCount : MonoBehaviour
{
    public TMP_Text WaveCountText;

    void Update()
    {
        WaveCountText.text = "" + WaveManager.Instance.WaveNumber;
    }
}
