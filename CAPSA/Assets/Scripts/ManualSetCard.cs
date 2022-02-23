using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualSetCard : MonoBehaviour
{
    public Transform[] Decks;
    public Transform[] Deck1Cards, Deck2Cards, Deck3Cards, Deck4Cards;
    // Start is called before the first frame update
    public void SetCards()
    {
        for (int a = 0; a < Deck1Cards.Length; a++)
        {
            Deck1Cards[a].parent = Decks[0];
            Deck1Cards[a].localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }

        for (int a = 0; a < Deck2Cards.Length; a++)
        {
            Deck2Cards[a].parent = Decks[1];
            Deck2Cards[a].localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

        for (int a = 0; a < Deck3Cards.Length; a++)
        {
            Deck3Cards[a].parent = Decks[2];
            Deck3Cards[a].localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

        for (int a = 0; a < Deck4Cards.Length; a++)
        {
            Deck4Cards[a].parent = Decks[3];
            Deck4Cards[a].localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetCards();
        }
    }
}
