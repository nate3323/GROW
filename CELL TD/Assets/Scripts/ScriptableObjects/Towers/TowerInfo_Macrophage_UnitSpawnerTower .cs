using Assets.Scripts.Towers;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This is a scriptable object that can be used to define basic information about any tower of any type.
/// However, you should use the BacteriaInfo, FungiInfo, or VirusInfo subclasses instead, depending on the type of your tower.
/// </summary>
[CreateAssetMenu(fileName = "New TowerInfo_Macrophage_UnitSpawnerTower", menuName = "Tower Info Assets/New TowerInfo_Macrophage_UnitSpawnerTower Asset")]
public class TowerInfo_Macrophage_UnitSpawnerTower : TowerInfo_Base
{
    [Header("Macrophage (Unit Spawner Tower)-Specific Stats")]

    // A dummy setting. The header causes an error if there are no properties here.
    [Tooltip("Dummy Stat")]
    [SerializeField]
    public int DummyStat;

}

