using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Go To Map Selector
    public void GoToSelector()
    {
        SceneManager.LoadScene("Selector");
    }

    //Open Settings
    public void OpenSettings()
    {
        //TODO
    }

    //Quit Game
    public void QuitGame()
    {
        Application.Quit();
    }
}
