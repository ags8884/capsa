using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour {

    public Image LoadingBar;
	// Use this for initialization
	void Start () {
        if (LoadingBar == null)
        {
            LoadingBar = GameObject.Find("LoadingBar").GetComponent<Image>();
        }
    }

    public void StartGame() {
        StartCoroutine(StartGameAsync());
    }

    public void StartGameAllBot()
    {
        StartCoroutine(StartGameAllBotAsync());
    }

    public void ExitGame()
    {
        Time.timeScale = 1;
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Application.Quit();
        }
        else
        {
            StartCoroutine(BackToMainMenuAsync());
        }
    }

    IEnumerator StartGameAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Cards");
        
        while (!asyncLoad.isDone)
        {
            if (LoadingBar == null)
            {
                LoadingBar = GameObject.Find("LoadingBar").GetComponent<Image>();
            }
            else
            {
                LoadingBar.fillAmount = asyncLoad.progress + 0.1f;
            }
            yield return null;
        }

        LoadingBar = null;
    }

    IEnumerator StartGameAllBotAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("CardsAllBots");

        while (!asyncLoad.isDone)
        {
            if (LoadingBar == null)
            {
                LoadingBar = GameObject.Find("LoadingBar").GetComponent<Image>();
            }
            else
            {
                LoadingBar.fillAmount = asyncLoad.progress + 0.1f;
            }
            yield return null;
        }

        LoadingBar = null;
    }

    IEnumerator BackToMainMenuAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");

        while (!asyncLoad.isDone)
        {
            if (LoadingBar == null)
            {
                LoadingBar = GameObject.Find("LoadingBar").GetComponent<Image>();
            }
            else
            {
                LoadingBar.fillAmount = asyncLoad.progress + 0.1f;
            }
            yield return null;
        }

        LoadingBar = null;
    }
}
