using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsScreen : MonoBehaviour
{
    [SerializeField] TMP_Dropdown numberOfCPUMenu;
    [SerializeField] TMP_Dropdown gameModeMenu;
    [SerializeField] TMP_Dropdown speedMenu;

    private void Start()
    {
        numberOfCPUMenu.value = PlayerPrefs.GetInt("NumberOfCpus");
        gameModeMenu.value = PlayerPrefs.GetInt("GameMode");
        speedMenu.value = PlayerPrefs.GetInt("Speed");
    }

    private void Update()
    {
        ManageInputs();
    }

    public void SetSpeed(int input)
    {
        // Save the difficulty setting
        PlayerPrefs.SetInt("Speed", input);
        PlayerPrefs.Save();
    }

    public void SetNumberOfCPU(int input)
    {
        // Save the difficulty setting
        
        PlayerPrefs.SetInt("NumberOfCpus", input);
        PlayerPrefs.Save();
    }

    public void SetGameMode(int input)
    {
        // Save the difficulty setting
        PlayerPrefs.SetInt("GameMode", input);
        PlayerPrefs.Save();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void ManageInputs()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            LoadMainMenu();
        }
    }
}
