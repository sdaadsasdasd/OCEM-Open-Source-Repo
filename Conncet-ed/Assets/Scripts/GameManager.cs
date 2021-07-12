using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject deathscreen;

    private energyBar ereference;
   
    // Start is called before the first frame update
    void Start()
    {
        ereference = GameObject.Find("EnergyBar").GetComponent<energyBar>();
        deathscreen.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (ereference.noEnergy == true) //Basically the gameOver Status, if energy comes down to 0 == GameOver
        {
            deathscreen.gameObject.SetActive(true);
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
