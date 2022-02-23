using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrangeCardOnDeck : MonoBehaviour
{
    [Header("MANUAL INPUT")]
    public float Width;
    public CardCheckerDeck CCD;


    [Header("NON/AUTO INPUT")]
    public float Speed;
    public Vector3 leftPoint;
    public Vector3 rightPoint;
    public float delta;
    public float howMany;
    public float howManyGapsBetweenItems;
    public float gapFromOneItemToTheNextOne;

    public int SpadeCardsCount;
    public int HeartCardsCount;
    public int ClubCardsCount;
    public int DiamondCardsCount;
    public List<int> SpadeCards = new List<int>();
    public List<int> HeartCards = new List<int>();
    public List<int> ClubCards = new List<int>();
    public List<int> DiamondCards = new List<int>();
    public CardId[] Cards;
    public GameObject[] Decks;


    public void ArrangeCard()
    {
        StartCoroutine(SCArrangetheCards());
    }

    IEnumerator SCArrangetheCards()
    {
        leftPoint = new Vector3(0 - Width, 0, 0);
        rightPoint = new Vector3(0 + Width, 0, 0);
        delta = (rightPoint - leftPoint).magnitude;
        howMany = this.transform.childCount;
        howManyGapsBetweenItems = howMany - 1;
        gapFromOneItemToTheNextOne = delta / howManyGapsBetweenItems;

        float theHighestIndex = howMany;


        Cards = new CardId[this.transform.childCount];

        for (int i = 0; i < this.transform.childCount; i++)
        {
            Cards[i] = this.transform.GetChild(i).GetComponent<CardId>();
            if ((int)Cards[i].CardIdentity == 1)
            {
                SpadeCardsCount += 1;
            }
            if ((int)Cards[i].CardIdentity == 2)
            {
                HeartCardsCount += 1;
            }
            if ((int)Cards[i].CardIdentity == 3)
            {
                ClubCardsCount += 1;
            }
            if ((int)Cards[i].CardIdentity == 4)
            {
                DiamondCardsCount += 1;
            }
        }

        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i].transform.parent = null;
        }

        /*
        //pengurutan berdasarkan ID

        SpadeCards = new List<int>(new int[SpadeCardsCount]);
        HeartCards = new List<int>(new int[HeartCardsCount]);
        ClubCards = new List<int>(new int[ClubCardsCount]);
        DiamondCards = new List<int>(new int[DiamondCardsCount]);

        for (int s = 0; s < DiamondCards.Count; s++)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                if ((int)Cards[i].CardIdentity == 1 && !SpadeCards.Contains((int)Cards[i].CardNumber))
                {
                    SpadeCards[s] = (int)Cards[i].CardNumber;
                    i = Cards.Length;
                }
            }
        }

        for (int s = 0; s < ClubCards.Count; s++)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                if ((int)Cards[i].CardIdentity == 2 && !HeartCards.Contains((int)Cards[i].CardNumber))
                {
                    HeartCards[s] = (int)Cards[i].CardNumber;
                    i = Cards.Length;
                }
            }
        }

        for (int s = 0; s < HeartCards.Count; s++)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                if ((int)Cards[i].CardIdentity == 3 && !ClubCards.Contains((int)Cards[i].CardNumber))
                {
                    ClubCards[s] = (int)Cards[i].CardNumber;
                    i = Cards.Length;
                }
            }
        }

        for (int s = 0; s < SpadeCards.Count; s++)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                if ((int)Cards[i].CardIdentity == 4 && !DiamondCards.Contains((int)Cards[i].CardNumber))
                {
                    DiamondCards[s] = (int)Cards[i].CardNumber;
                    i = Cards.Length;
                }
            }
        }

        SpadeCards.Sort();
        HeartCards.Sort();
        ClubCards.Sort();
        DiamondCards.Sort();

        for (int s = 0; s < SpadeCards.Count; s++)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                if ((int)Cards[i].CardIdentity == 1 && (int)Cards[i].CardNumber == SpadeCards[s])
                {
                    Cards[i].transform.parent = this.transform;
                    i = Cards.Length;
                }
            }
        }

        for (int s = 0; s < HeartCards.Count; s++)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                if ((int)Cards[i].CardIdentity == 2 && (int)Cards[i].CardNumber == HeartCards[s])
                {
                    Cards[i].transform.parent = this.transform;
                    i = Cards.Length;
                }
            }
        }

        for (int s = 0; s < ClubCards.Count; s++)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                if ((int)Cards[i].CardIdentity == 3 && (int)Cards[i].CardNumber == ClubCards[s])
                {
                    Cards[i].transform.parent = this.transform;
                    i = Cards.Length;
                }
            }
        }

        for (int s = 0; s < DiamondCards.Count; s++)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                if ((int)Cards[i].CardIdentity == 4 && (int)Cards[i].CardNumber == DiamondCards[s])
                {
                    Cards[i].transform.parent = this.transform;
                    i = Cards.Length;
                }
            }
        }*/


        for (int a = 3; a <= 15; a++)
        {
            for (int i = 1; i <= 4; i++)
            {
                for (int s = 0; s < Cards.Length; s++)
                {
                    if ((int)Cards[s].CardIdentity == i && (int)Cards[s].SingleCardNumber == a)
                    {
                        Cards[s].transform.parent = this.transform;
                    }
                }
            }
        }

        for (int i = 0; i < theHighestIndex; i++)
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

        CCD.CheckAllCard();

        yield return null;
    }


}