using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        WaveManager.Instance.StartNextWave();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
