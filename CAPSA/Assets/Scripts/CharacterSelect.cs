using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    public SceneManagerScript SceneManager;
    public Sprite Smile;
    public Image[] Faces;
    public Button[] Buttons;
    public bool Clicked;

    public void PlayerSelect(int i)
    {
        if (!Clicked)
        {
            Clicked = true;
            PlayerPrefs.SetInt("Chara", i);
            SFXManager.main.Playing();
            StartCoroutine(Selecting(i));
        }
    }

    IEnumerator Selecting(int i)
    {
        for (int j = 0; j < Buttons.Length; j++)
        {
            if (j != i)
            {
                Buttons[j].interactable = false;
            }
            else 
            {
                Faces[j].sprite = Smile;
            }
        }
        yield return new WaitForSeconds(1);
        SceneManager.StartGame();
    }
}
