using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{

    public Slider enemySlider;
    //sets value of health bar based on player's health
    public void SetEnemyHealth(int health)
    {
        enemySlider.value = health;
    }
    //sets max health and sets position of health to match
    public void SetMaxHealth(int health)
    {
        enemySlider.maxValue = health;
        enemySlider.value = health;
    }
}
