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

    private bool end = false;

    private void Start()
    {
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
        if(PlayerPrefs.GetInt("GameMode") == 0)
        {
            Instantiate(player1);
            Instantiate(player2);
        }
        else
        {
            Instantiate(player1);
            Instantiate(cpu1);
            Instantiate(cpu2);
            Instantiate(cpu3);
            Instantiate(cpu4);
            Instantiate(cpu5);
            Instantiate(cpu6);
            Instantiate(cpu7);
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
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (listOfCPU.Length == 1)
        {
            PlayerPrefs.SetString("Winner", listOfCPU[0].name.Replace("(Clone)", ""));
        }
        else if (playerObject && listOfCPU.Length == 0)
        {
            PlayerPrefs.SetString("Winner", playerObject.name.Replace("(Clone)", ""));
        }
    }

    private void EndScreen()
    {
        SceneManager.LoadScene("EndScreen");
    }

    private bool CheckForAlivePlayers()
    {
        bool result = true;
        if (GameObject.FindWithTag("Player") == null && GameObject.FindWithTag("CPU") == null)
        {
            result = false;
        }
        return result;
    }
}
