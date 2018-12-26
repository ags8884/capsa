using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtonScript : MonoBehaviour {

    public GameObject GameManager;

    public void StartButton()
    {
        Time.timeScale = 1;
        if (GameManager == null) {
            GameManager = GameObject.Find("GameManager");
        }

        GameManager.GetComponent<SceneManagerScript>().StartGame();
    }

    public void StartButtonAllBot()
    {
        Time.timeScale = 1;
        if (GameManager == null)
        {
            GameManager = GameObject.Find("GameManager");
        }

        GameManager.GetComponent<SceneManagerScript>().StartGameAllBot();
    }

    public void QuitButton()
    {
        Time.timeScale = 1;
        if (GameManager == null)
        {
            GameManager = GameObject.Find("GameManager");
        }

        GameManager.GetComponent<SceneManagerScript>().ExitGame();
    }

}
