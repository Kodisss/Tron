using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tron : MonoBehaviour
{
    [Header("Players Prefabs")]
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;

    [Header("CPUs Prefabs")]
    [SerializeField] private GameObject cpu1;
    [SerializeField] private GameObject cpu2;
    [SerializeField] private GameObject cpu3;
    [SerializeField] private GameObject cpu4;
    [SerializeField] private GameObject cpu5;
    [SerializeField] private GameObject cpu6;
    [SerializeField] private GameObject cpu7;

    GameObject[] cpuPrefabs;

    private bool end = false;

    private void Start()
    {
        cpuPrefabs = new GameObject[] { cpu1, cpu2, cpu3, cpu4, cpu5, cpu6, cpu7 };
        SpawnEveryone();
    }

    // Update is called once per frame
    private void Update()
    {
        GetWinnerName();
        ManageGameEnd();
    }

    private void SpawnEveryone()
    {
        Instantiate(player1);

        for (int i = 0; i < PlayerPrefs.GetInt("NumberOfCpus"); i++)
        {
            Instantiate(cpuPrefabs[i]);
        }

        if (PlayerPrefs.GetInt("GameMode") == 0)
        {
            Instantiate(player2);
        }
    }

    private void ManageGameEnd()
    {
        if (!CheckForAlivePlayers() && !end)
        {
            end = true;
            Invoke(nameof(EndScreen), 1f);
        }
    }

    private void GetWinnerName()
    {
        GameObject[] listOfCPU = GameObject.FindGameObjectsWithTag("CPU");
        GameObject playerObject1 = GameObject.FindGameObjectWithTag("Player1");
        GameObject playerObject2 = GameObject.FindGameObjectWithTag("Player2");

        if (listOfCPU.Length == 1)
        {
            PlayerPrefs.SetString("Winner", listOfCPU[0].name.Replace("(Clone)", ""));
        }
        else if (playerObject1 && listOfCPU.Length == 0)
        {
            PlayerPrefs.SetString("Winner", playerObject1.name.Replace("(Clone)", ""));
        }
        else if (playerObject2 && listOfCPU.Length == 0 && PlayerPrefs.GetInt("GameMode") == 0)
        {
            PlayerPrefs.SetString("Winner", playerObject2.name.Replace("(Clone)", ""));
        }
    }

    private void EndScreen()
    {
        SceneManager.LoadScene("EndScreen");
    }

    private bool CheckForAlivePlayers()
    {
        bool result = true;

        if (PlayerPrefs.GetInt("GameMode") == 0)
        {
            if (GameObject.FindWithTag("Player1") == null && GameObject.FindWithTag("Player 2") == null && GameObject.FindWithTag("CPU") == null)
            {
                result = false;
            }
        }
        else
        {
            if (GameObject.FindWithTag("Player1") == null && GameObject.FindWithTag("CPU") == null)
            {
                result = false;
            }
        }

        return result;
    }
}
