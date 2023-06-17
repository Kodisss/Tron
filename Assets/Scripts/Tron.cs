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
    [SerializeField] private GameObject cpu8;
    [SerializeField] private GameObject cpu9;

    GameObject[] cpuPrefabs;

    private bool end = false;

    private void Start()
    {
        cpuPrefabs = new GameObject[] { cpu1, cpu2, cpu3, cpu4, cpu5, cpu6, cpu7, cpu8, cpu9 };
        SpawnEveryone();
    }

    // Update is called once per frame
    private void Update()
    {
        GetWinnerName();
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

    private void GetWinnerName()
    {
        GameObject[] listOfCPU = GameObject.FindGameObjectsWithTag("CPU");
        List<string> listOfNames = new List<string>();
        GameObject playerObject1 = GameObject.FindGameObjectWithTag("Player1");
        GameObject playerObject2 = GameObject.FindGameObjectWithTag("Player2");

        for(int i = 0; i < listOfCPU.Length; i++)
        {
            if(listOfCPU[i].transform.Find("Winner") != null)
            {
                listOfNames.Add(listOfCPU[i].name.Replace("(Clone)", ""));
            }
        }

        if (playerObject1.transform.Find("Winner") != null) listOfNames.Add(playerObject1.name.Replace("(Clone)", ""));
        if (playerObject2)
        {
            if (playerObject2.transform.Find("Winner") != null) listOfNames.Add(playerObject2.name.Replace("(Clone)", ""));
        }
        

        if (listOfNames.Count == 1 && !end)
        {
            PlayerPrefs.SetString("Winner", listOfNames[0]);
            end = true;
            Invoke(nameof(EndScreen), 1f);
        }
    }

    private void EndScreen()
    {
        SceneManager.LoadScene("EndScreen");
    }
}