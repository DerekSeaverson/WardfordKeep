using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{
    public void playGame()
    {
        SceneManager.LoadScene("Level1_Game");
    }
    public void quitGame()
    {
        Application.Quit();
    }
    public void controlsPage()
    {
        SceneManager.LoadScene("Controls");
    }
    public void backToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
