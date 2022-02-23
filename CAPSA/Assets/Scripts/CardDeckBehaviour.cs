using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardDeckBehaviour : MonoBehaviour
{

    List<int> RandCard = new List<int>();
    public GameObject[] Cards;
    public GameObject[] Decks;
    public int RandNum;

    public void Start()
    {
        Time.timeScale = 1;
        RandomTheCard();
    }

    public void RandomTheCard()
    {
        Cards = new GameObject[this.transform.childCount];

        for (int i = 0; i < this.transform.childCount; i++)
        {
            Cards[i] = this.transform.GetChild(i).gameObject;
        }

        RandCard = new List<int>(new int[Cards.Length]);

        for (int i = 0; i < Cards.Length; i++)
        {
            do
            {
                RandNum = Random.Range(0, Cards.Length + 1);
            } while (RandCard.Contains(RandNum));
            RandCard[i] = RandNum;
        }

        for (int i = 0; i < Cards.Length; i++)
        {
            this.transform.GetChild(RandCard[i] - 1).SetSiblingIndex(i);
        }

        for (int i = 0; i < Cards.Length; i++)
        {
            this.transform.GetChild(0).localPosition = new Vector3(0, 0, 0);

            if (i != 0)
            {
                this.transform.GetChild(i).localPosition = new Vector3(this.transform.GetChild(i).localPosition.x, this.transform.GetChild(i).localPosition.y, this.transform.GetChild(i - 1).localPosition.z + 0.01f);
            }
        }

        RandCard.Clear();
        StartCoroutine(ShareTheCard());
    }

    IEnumerator ShareTheCard()
    {
        int players = 0;

        for (int i = 0; i < Cards.Length; i++)
        {
            yield return new WaitForSeconds(0.15f);
            this.transform.GetChild(0).SetParent(Decks[players].transform);
            Decks[players].transform.GetChild(Decks[players].transform.childCount - 1).GetComponent<CardId>().MoveTheCards();
            if (players < Decks.Length - 1)
            {
                players++;
            }
            else
            {
                players = 0;
            }
        }

        yield return new WaitForSeconds(0.75f);

        for (int i = 0; i < Decks.Length; i++)
        {
            Decks[i].GetComponent<ArrangeCardOnDeck>().ArrangeCard();
        }

        yield return new WaitForSeconds(0.75f);

        for (int i = 0; i < Decks.Length; i++)
        {
            if (Decks[i].GetComponent<BotAI>() != null)
            {
                Decks[i].GetComponent<BotAI>().CheckThreeDiamond();
            } else if (Decks[i].GetComponent<PlayerBehaviour>() != null)
            {
                Decks[i].GetComponent<PlayerBehaviour>().CheckThreeDiamond();
            }
        }
    }
}
