using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This is the base class for all fungi-type enemies
/// </summary>
public class FungiBase : EnemyBase, IFungi
{
    new void Awake()
    {
        base.Awake();

        // Do initialization here.
        IsFungi = true;
    }

    new void Start()
    {
        base.Start();

        // Do initialization here.
    }
}
