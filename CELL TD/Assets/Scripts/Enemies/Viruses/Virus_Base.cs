using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This is the base class for all virus-type enemies
/// </summary>
public class Virus_Base : EnemyBase, IVirus
{
    new void Awake()
    {
        base.Awake();

        // Do initialization here.
        IsVirus = true;
    }

    new void Start()
    {
        base.Start();

        // Do initialization here.        
    }
}
