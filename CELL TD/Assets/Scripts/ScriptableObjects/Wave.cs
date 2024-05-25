using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Enemy Spawning Assets/New Wave Asset")]
public class Wave : ScriptableObject
{

    public List<EnemySpawnInfo> Enemies;

}