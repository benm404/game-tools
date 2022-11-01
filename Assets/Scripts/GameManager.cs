using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    }
}
