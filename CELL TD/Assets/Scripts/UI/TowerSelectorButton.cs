using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// This is a custom button class that simply adds the ability for the button to know which tower type
/// it is associated with.
/// </summary>
public class TowerSelectorButton : Button
{
    // The tower type associated with this button
    public TowerTypes TowerType;
}
