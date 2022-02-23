using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SFXManager : MonoBehaviour
{
    public static SFXManager main;
    public AudioSource SFX, Music;
    public AudioClip CardMove, Play, Pass, Win, Lose, MenuMusic;
    // Start is called before the first frame update
    void Awake()
    {
        if (main != null)
        {
            Destroy(gameObject);
        }
        else
        {
            main = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        if (Music.volume != 0.5f && (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1))
        {
            Music.volume = Mathf.MoveTowards(Music.volume, 0.5f, Time.deltaTime);
        }
        else if (Music.volume != 0)
        {
            Music.volume = Mathf.MoveTowards(Music.volume, 0, Time.deltaTime);
        }
    }

    public void CardMoving()
    {
        SFX.clip = CardMove;
        SFX.Play();
    }

    public void Playing()
    {
        SFX.clip = Play;
        SFX.Play();
    }

    public void Passing()
    {
        SFX.clip = Pass;
        SFX.Play();
    }

    public void Winning()
    {
        SFX.clip = Win;
        SFX.Play();
    }

    public void Losing()
    {
        SFX.clip = Lose;
        SFX.Play();
    }
}
