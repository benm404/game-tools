using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    PlayerMovement PlayerMovement;
    public Slider DashSlider;
    public Slider HideSlider;
    
    void Start()
    {
        PlayerMovement = GameObject.FindWithTag("Player").GetComponent < PlayerMovement > ();
        
    }

    
    void Update()
    {
        DashSlider.value = PlayerMovement.Stamina;
        HideSlider.value = PlayerMovement.HideStamina;

        // Reload the current scene when R & - are pressed
        if (Input.GetKey(KeyCode.R) && Input.GetKeyDown(KeyCode.Minus))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Quit the application when Escape is pressed
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
            print("I will exit on unity builds");
        }
    }

    public void ExitGame()
    {
        Application.Quit();
        print("I will exit on unity builds");
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
