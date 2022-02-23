using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactersBehaviour : MonoBehaviour
{
    public bool Player;
    public int PlayerId;
    public Sprite[] Faces, Hairs, Kits;
    public Image Face, Hair, Kit;
    public CharactersBehaviour[] AnotherCB;
    Coroutine Corr;
    // Start is called before the first frame update
    void Start()
    {
        Idle();
        if (Player)
        {
            PlayerId = PlayerPrefs.GetInt("Chara");
            Hair.sprite = Hairs[PlayerId];
            Kit.sprite = Kits[PlayerId];
        }
        else 
        {
            Hair.sprite = Hairs[Random.Range(0, Hairs.Length)];
            Kit.sprite = Kits[Random.Range(0, Hairs.Length)];
        }
    }
    public void Win()
    {
        Happy();

        for (int i = 0; i < AnotherCB.Length; i++)
        {
            AnotherCB[i].Sad();
        }
    }

    public void Sad()
    {
        if (Corr != null)
        {
            StopCoroutine(Corr);
        }

        Corr = StartCoroutine(DelayFaces(Faces[2]));
    }

    public void Happy()
    {
        if (Corr != null)
        {
            StopCoroutine(Corr);
        }

        Corr = StartCoroutine(DelayFaces(Faces[1]));
    }

    public void Idle()
    {
        Face.sprite = Faces[0];
    }

    IEnumerator DelayFaces(Sprite SFaces)
    {
        Face.sprite = SFaces;
        yield return new WaitForSeconds(2);
        Face.sprite = Faces[0];
        Corr = null;
    }
}
