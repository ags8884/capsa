using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayingDeckScript : MonoBehaviour
{
    [Header("MANUAL INPUT")]
    public float Width;
    public GameObject TheCardDeck;
    public GameObject Text;

    [Header("NON/AUTO INPUT")]
    public static PlayingDeckScript main;
    public float Speed;
    public Vector3 leftPoint;
    public Vector3 rightPoint;
    public float delta;
    public float howMany;
    public float howManyGapsBetweenItems;
    public float gapFromOneItemToTheNextOne;

    public bool Single, Pair, ThreeOfKind, FourOfKind, Straight, Flush, RoyalFlush, FullHouse;
    public GameObject[] Cards;

    private void Awake()
    {
        main = this;
    }

    public void UpdateText(string TextUpdt)
    {
        Text.SetActive(false);
        Text.GetComponent<Text>().text = TextUpdt;
        Text.SetActive(true);
    }

    public void Empty()
    {
        Single = false;
        Pair = false;
        ThreeOfKind = false;
        FourOfKind = false;
        Straight = false;
        Flush = false;
        RoyalFlush = false;
        FullHouse = false;
    }

    public void SingleCard()
    {
        Single = true;
        Pair = false;
        ThreeOfKind = false;
        FourOfKind = false;
        Straight = false;
        Flush = false;
        RoyalFlush = false;
        FullHouse = false;
    }

    public void PairCard()
    {
        Single = false;
        Pair = true;
        ThreeOfKind = false;
        FourOfKind = false;
        Straight = false;
        Flush = false;
        RoyalFlush = false;
        FullHouse = false;
    }

    public void ThreeOfKindCard()
    {
        Single = false;
        Pair = false;
        ThreeOfKind = true;
        FourOfKind = false;
        Straight = false;
        Flush = false;
        RoyalFlush = false;
        FullHouse = false;
    }

    public void ThreeOfKindCardWithPairCard()
    {
        Single = false;
        Pair = true;
        ThreeOfKind = true;
        FourOfKind = false;
        Straight = false;
        Flush = false;
        RoyalFlush = false;
        FullHouse = false;
    }

    public void FourOfKindCard()
    {
        Single = false;
        Pair = false;
        ThreeOfKind = false;
        FourOfKind = true;
        Straight = false;
        Flush = false;
        RoyalFlush = false;
        FullHouse = false;
    }

    public void StraightCard()
    {
        Single = false;
        Pair = false;
        ThreeOfKind = false;
        FourOfKind = false;
        Straight = true;
        Flush = false;
        RoyalFlush = false;
        FullHouse = false;
    }

    public void FlushCard()
    {
        Single = false;
        Pair = false;
        ThreeOfKind = false;
        FourOfKind = false;
        Straight = false;
        Flush = true;
        RoyalFlush = false;
        FullHouse = false;
    }

    public void RoyalFlushCard()
    {
        Single = false;
        Pair = false;
        ThreeOfKind = false;
        FourOfKind = false;
        Straight = false;
        Flush = false;
        RoyalFlush = true;
        FullHouse = false;
    }

    public void FullHouseCard()
    {
        Single = false;
        Pair = false;
        ThreeOfKind = false;
        FourOfKind = false;
        Straight = false;
        Flush = false;
        RoyalFlush = false;
        FullHouse = true;
    }

    public void CleanTheChild() {
        Cards = new GameObject[this.transform.childCount];
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Cards[i] = this.transform.GetChild(i).gameObject;
        }
        if (Cards.Length > 0) {
            for (int i = 0; i < Cards.Length; i++) {
                Cards[i].transform.parent = TheCardDeck.transform;
                Cards[i].GetComponent<MeshRenderer>().enabled = false;
                Cards[i].GetComponent<CardId>().MoveTheCards();
            }
        }
    }

    public void ArrangeTheCards()
    {
        StartCoroutine(WaitForArrangeTheCards());
    }

    IEnumerator WaitForArrangeTheCards()
    {
        yield return new WaitForSeconds(0.5f);

        leftPoint = new Vector3(0 - Width, 0, 0);
        rightPoint = new Vector3(0 + Width, 0, 0);
        delta = (rightPoint - leftPoint).magnitude;
        howMany = this.transform.childCount;
        howManyGapsBetweenItems = howMany - 1;
        gapFromOneItemToTheNextOne = delta / howManyGapsBetweenItems;

        Cards = new GameObject[this.transform.childCount];

        for (int i = 0; i < this.transform.childCount; i++)
        {
            Cards[i] = this.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).localPosition = leftPoint;

            if (Cards.Length > 1)
            {
                if (i == 0)
                {
                    this.transform.GetChild(i).localPosition += new Vector3((i * gapFromOneItemToTheNextOne), this.transform.GetChild(i).localPosition.y, this.transform.GetChild(i).localPosition.z);
                }
                else
                {
                    this.transform.GetChild(i).localPosition += new Vector3((i * gapFromOneItemToTheNextOne), this.transform.GetChild(i).localPosition.y, this.transform.GetChild(i - 1).localPosition.z - 0.001f);
                }
            }
            else
            {
                this.transform.GetChild(i).localPosition = new Vector3(0, 0, 0);
            }
        }
    }
}
