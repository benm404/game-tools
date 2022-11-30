using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    PlayerMovement PlayerMovement;
    public Slider DashSlider;
    public Slider HideSlider;
    public Slider HealthSlider;
    public TextMeshProUGUI ScoreDisplay;
    public int Score;
    
    void Start()
    {
        PlayerMovement = GameObject.FindWithTag("Player").GetComponent < PlayerMovement > ();
        Score = 0;
    }

    
    void Update()
    {
        DashSlider.value = PlayerMovement.Stamina;
        HideSlider.value = PlayerMovement.HideStamina;
        HealthSlider.value = PlayerMovement.Health;
        ScoreDisplay.text = ("Score: " + Score);

        // Reload the current scene when R & - are pressed
        if (Input.GetKey(KeyCode.R) && Input.GetKeyDown(KeyCode.Minus))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
