using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This is a scriptable object that can be used to define a status effect.
/// </summary>
[CreateAssetMenu(fileName = "New StatusEffectInfo_SlowedMovement", menuName = "Status Effect Info Assets/New StatusEffectInfo_SlowedMovement Asset")]
public class StatusEffectInfo_SlowedMovement : StatusEffectInfo_Base
{   
    [Tooltip("The maximum speed the enemy can move at while this status effect is active")]
    public float MaxMoveSpeed;
}
