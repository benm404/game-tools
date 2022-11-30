using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadLevelTwo()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
        print("I will exit on unity builds");
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
