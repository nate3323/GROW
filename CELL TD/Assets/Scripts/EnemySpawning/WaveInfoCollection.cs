using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// This class automatically loads in all Wave assets from the Resources folder,
/// which contain the data on each wave.
/// </summary>
public class WaveCollection : MonoBehaviour
{
    private List<Wave> _WaveList = new List<Wave>();



    private void Awake()
    {
        // Load all of the WaveInfo scriptable objects in the project, even if they
        // aren't inside the EnemyInfos folder.
        _WaveList = Resources.LoadAll<Wave>("").ToList();
    }

    public List<EnemySpawnInfo> GetEnemySpawnInfo(int index)
    {
        return _WaveList[index].Enemies;
    }

    public Wave GetWaveInfo(int index)
    {
        return _WaveList[index];
    }



    public int Count { get { return _WaveList.Count; } }
}