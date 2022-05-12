using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider playerSlider;
    //sets value of health bar based on player's health
    public void SetPlayerHealth(int health)
    {
        playerSlider.value = health;
    }
    //sets max health and sets position of health to match
    public void SetMaxHealth(int health)
    {
        playerSlider.maxValue = health;
        playerSlider.value = health;
    }
}
