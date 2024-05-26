using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelector : MonoBehaviour
{
    [SerializeField]
    private string _mapName;
    public void LoadMap()
    {
        SceneManager.LoadScene(_mapName);
    }
}
