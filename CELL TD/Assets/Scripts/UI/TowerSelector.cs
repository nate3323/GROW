using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// This class manages the Tower Selector panel.
/// </summary>
public class TowerSelector : MonoBehaviour
{
    [Tooltip("This is the UI element that the buttons will be placed in.")]
    [SerializeField]
    private Transform _ButtonsParent;

    [SerializeField]
    private GameObject _TowerSelectorButtonPrefab;


    private List<TowerInfo_Base> _TowerInfosCollection;



    private void Awake()
    {
        // Load all of the TowerInfo_Base scriptable objects (including ones that are
        // subclasses of TowerInfo_Base) in the project, even if they
        // aren't inside the TowerInfos folder.
        _TowerInfosCollection = Resources.LoadAll<TowerInfo_Base>("").ToList();
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateTowerButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Creates the tower buttons.
    /// </summary>
    private void CreateTowerButtons()
    {
        for (int i = 0; i < _TowerInfosCollection.Count; i++)
        {
            TowerSelectorButton button = Instantiate(_TowerSelectorButtonPrefab, _ButtonsParent).GetComponent<TowerSelectorButton>();
            button.image.sprite = _TowerInfosCollection[i].DisplayIcon;

            // Store the tower type in the button's TowerType property.
            button.TowerType = _TowerInfosCollection[i].TowerType;

            button.onClick.AddListener(() =>
            {
                OnTowerSelectButtonClicked(button);
            });
            
        }
    }

    private void OnTowerSelectButtonClicked(TowerSelectorButton button)
    {
        switch(button.TowerType)
        {
            case TowerTypes.Lymphocyte_SlowingAOETower:
                break;
            case TowerTypes.Macrophage_UnitSpawnerTower:
                break;
            case TowerTypes.Neutrophil_ProjectileTower:
                break;

            default:
                Debug.LogError($"The tower type \"{Enum.GetName(typeof(TowerTypes), button.TowerType)}\" is not implemented yet in TowerSelector.OnTowerSelectButtonClicked!");
                break;

        }
    }

}
