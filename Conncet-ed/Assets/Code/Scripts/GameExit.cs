using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExit : MonoBehaviour
{
    public void quitGame()
    {
        Application.Quit();
        Debug.Log("game should exit now...");
    }
}
