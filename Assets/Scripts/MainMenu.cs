using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Tron");
    }

    public void SettingsScreen()
    {
        SceneManager.LoadScene("SettingsScreen");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
