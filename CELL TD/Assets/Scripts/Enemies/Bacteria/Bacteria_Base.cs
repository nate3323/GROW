using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This is the base class for all bacteria-type enemies
/// </summary>
public class Bacteria_Base : EnemyBase, IBacteria    
{
    new void Awake()
    {
        base.Awake();

        // Do initialization here.
        IsBacteria = true;
    }

    new void Start()
    {
        base.Start();

        // Do initialization here.
    }
}
