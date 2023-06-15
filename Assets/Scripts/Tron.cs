using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tron : MonoBehaviour
{
    private bool end = false;

    // Update is called once per frame
    private void Update()
    {
        ManageGameEnd();
    }

    private void ManageGameEnd()
    {
        if (!CheckForAlivePlayers() && !end)
        {
            end = true;
            Invoke(nameof(EndScreen), 1f);
        }
    }

    private void EndScreen()
    {
        SceneManager.LoadScene("EndScreen");
    }

    private bool CheckForAlivePlayers()
    {
        return GameObject.FindWithTag("Player") != null;
    }
}
