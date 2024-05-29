using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This is a scriptable object that can be used to define basic information about any tower of any type.
/// However, you should use the BacteriaInfo, FungiInfo, or VirusInfo subclasses instead, depending on the type of your tower.
/// </summary>
[CreateAssetMenu(fileName = "New TowerInfo_Base", menuName = "Tower Info Assets/New TowerInfo_Base Asset")]
public class TowerInfo_Base : ScriptableObject
{
    [Header("General Tower Info")]
    
    public string DisplayName; // The name displayed for this tower in the UI
    public TowerTypes TowerType; // The type of this tower.
    public Sprite UiIcon; // The icon used for this tower in the UI
    public GameObject Prefab; // The prefab for this tower type


    [Header("Tower Stats")]

    [Tooltip("How much it costs the player to build this type of tower.")]
    [SerializeField, Min(0)]
    public float BuildCost;

    [Tooltip("This is the percentage of the cost that is refunded when the player destroys the tower.")]
    [Range(0f, 1f)]
    [SerializeField]
    public float RefundPercentage = 0.85f;

    [Tooltip("How much it costs the player to upgrade this tower.")]
    [SerializeField, Min(0)] 
    public float UpgradeCost;

    [Tooltip("The damage done by this tower if it has direct attacks.")]
    [SerializeField, Min(0)]
    public float DamageValue;

    [Tooltip("The fire rate of this tower if it has direct attacks.")]
    [SerializeField, Min(0)]
    public float FireRate;

    [Tooltip("How many targets this tower can have at one time.")]
    [SerializeField, Min(0)]
    public int NumberOfTargets;

}
