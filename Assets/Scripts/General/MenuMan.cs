using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMan : MonoBehaviour
{
    public void GoToGame()
    {
        SceneManager.LoadScene("WorldMap");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void EraseGame()
    {
        
    }
}
