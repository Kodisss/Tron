using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject eventSystem;
    private bool isPaused;

    // Start is called before the first frame update
    private void Start()
    {
        pauseMenu.SetActive(false);
        eventSystem.SetActive(true);
        isPaused = false;
    }

    // Update is called once per frame
    private void Update()
    {
        ManageInputs();
    }

    public bool GetPaused()
    {
        return isPaused;
    }

    public void PauseGame()
    {
        eventSystem.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        eventSystem.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Tron");
    }

    private void ManageInputs()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (isPaused && Input.GetButtonDown("Cancel"))
        {
            ResumeGame();
        }
    }
}
