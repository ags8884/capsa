using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtonScript : MonoBehaviour {

    public SceneManagerScript SceneManager;

    public void StartButton()
    {
        Time.timeScale = 1;
        SceneManager.CharacterSelection();
        SFXManager.main.Playing();
    }

    public void StartButtonAllBot()
    {
        Time.timeScale = 1;
        SceneManager.StartGameAllBot();
        SFXManager.main.Playing();
    }

    public void QuitButton()
    {
        Time.timeScale = 1;
        SceneManager.ExitGame();
    }

}
