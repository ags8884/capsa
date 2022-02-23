using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("MANUAL INPUT")]
    public CharactersBehaviour CB;
    public Transform SelectionDeck;
    public GameObject Text;
    public PlayingDeckScript PlayingDeck;
    public GameObject NextPlayer;
    public Button DEAL, PASS;

    [Header("NON/AUTO INPUT")]
    public bool turn;
    public CardId[] Cards;
    public GameObject[] CardsToBeSetToDeck;

    public int SpadeCardsCount;
    public int HeartCardsCount;
    public int ClubCardsCount;
    public int DiamondCardsCount;
    public int PassCounter;

    public GameObject[] SpadeCards;
    public GameObject[] HeartCards;
    public GameObject[] ClubCards;
    public GameObject[] DiamondCards;

    public Dictionary<int, int> PairsDic;
    public Dictionary<int, int> RoyalFlushDic;
    public List<int> CardNumberList;
    public List<int> CardSuitList;
    public List<CardId> CardsList;
    string SuitName;

    //Royal Flush And Flush Variable
    public int cardnumber;
    public int identity;
    public int value;
    // Use this for initialization
    void Start()
    {
        PlayingDeck = PlayingDeckScript.main;
        DEAL.interactable = false;
        PASS.interactable = false;
    }

    public void CheckThreeDiamond()
    {
        Cards = new CardId[this.transform.childCount];

        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i] = this.transform.GetChild(i).GetComponent<CardId>();
        }

        for (int i = 0; i < Cards.Length; i++)
        {
            if ((int)Cards[i].CardNumber == 3 && (int)Cards[i].CardIdentity == 1)
            {
                PlayingDeck.CleanTheChild();
                Cards[i].transform.parent = PlayingDeck.transform;
                Cards[i].MoveTheCards();
                this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
                PlayingDeck.SingleCard();
                PlayingDeck.ArrangeTheCards();
                if (NextPlayer.GetComponent<BotAI>() != null)
                {
                    NextPlayer.GetComponent<BotAI>().PlayTurn(0);
                    turn = false;
                }
                else if (NextPlayer.GetComponent<PlayerBehaviour>() != null)
                {
                    NextPlayer.GetComponent<PlayerBehaviour>().PassCounter = 0;
                    NextPlayer.GetComponent<PlayerBehaviour>().turn = true;
                    turn = false;
                }

                Text.SetActive(false);
                Text.GetComponent<Text>().text = "FIRST MOVE";
                Text.SetActive(true);
            }
        }
    }

    public void Clicked(int CardIdentity, int CardNumber, int ChildIndex)
    {
        StartCoroutine(ClickedCard(PassCounter, CardIdentity, CardNumber, ChildIndex));
    }

    public void ArrangeTheCard()
    {
        Cards = new CardId[this.transform.childCount];

        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i] = this.transform.GetChild(i).GetComponent<CardId>();
            //Cards[i].transform.localPosition = new Vector3(Cards[i].transform.localPosition.x, 0, Cards[i].transform.localPosition.z);
        }
    }

    public void CheckRemainingCards()
    {
        StartCoroutine(CheckRemainingCardsEnum());
    }

    IEnumerator CheckRemainingCardsEnum()
    {
        yield return new WaitForSeconds(0.2f);
        Cards = new CardId[this.transform.childCount];
        if (Cards.Length <= 0)
        {
            CB.Win();
            Text.SetActive(false);
            Text.GetComponent<Text>().text = this.transform.name + " WIN!!";
            Text.SetActive(true);
            SFXManager.main.Winning();
            Time.timeScale = 0;
        }
    }

    IEnumerator ClickedCard(int PassCounter, int CardIdentity, int CardNumber, int ChildIndex)
    {
        yield return new WaitForEndOfFrame();

        CardsList = new List<CardId>();

        for (int i = 0; i < SelectionDeck.childCount; i++)
        {
            CardsList.Add(SelectionDeck.GetChild(i).GetComponent<CardId>());
        }

        Debug.Log("Selected Card Count : " + CardsList.Count);

        for (int i = 0; i < CardsList.Count; i++)
        {
            CardsList[i].transform.parent = null;
        }

        for (int a = 3; a <= 15; a++)
        {
            for (int i = 1; i <= 4; i++)
            {
                for (int s = 0; s < CardsList.Count; s++)
                {
                    if ((int)CardsList[s].CardIdentity == i && (int)CardsList[s].SingleCardNumber == a)
                    {
                        CardsList[s].transform.parent = SelectionDeck.transform;
                    }
                }
            }
        }


        turn = true;

        yield return new WaitForEndOfFrame();

        if (turn)
        {
            DEAL.interactable = false;
            PASS.interactable = true;

            if (!DEAL.interactable && CardsList.Count == 5 && ((PassCounter < 3 && PlayingDeck.FullHouse) || PassCounter >= 3))
            {
                StartCoroutine(CheckFullHouseCard(PassCounter, CardsList));
            }
            if (!DEAL.interactable && CardsList.Count == 5 && ((PassCounter < 3 && PlayingDeck.RoyalFlush) || PassCounter >= 3))
            {
                StartCoroutine(CheckRoyalFlushCard(PassCounter, CardsList));
            }
            if (!DEAL.interactable && CardsList.Count == 5 && ((PassCounter < 3 && PlayingDeck.Flush) || PassCounter >= 3))
            {
                StartCoroutine(CheckFlushCard(PassCounter, CardsList));
            }
            if (!DEAL.interactable && CardsList.Count == 5 && ((PassCounter < 3 && PlayingDeck.Straight) || PassCounter >= 3))
            {
                StartCoroutine(CheckStraightCard(PassCounter, CardsList));
            }
            if (!DEAL.interactable && CardsList.Count == 5 && ((PassCounter < 3 && PlayingDeck.FourOfKind) || PassCounter >= 3))
            {
                StartCoroutine(CheckFourOfKindCard(PassCounter, CardsList));
            }
            if (!DEAL.interactable && CardsList.Count == 3 && ((PassCounter < 3 && PlayingDeck.ThreeOfKind) || PassCounter >= 3))
            {
                StartCoroutine(CheckThreeOfKindCard(PassCounter, CardsList));
            }
            if (!DEAL.interactable && CardsList.Count == 2 && ((PassCounter < 3 && PlayingDeck.Pair) || PassCounter >= 3))
            {
                StartCoroutine(CheckPairCard(PassCounter, CardsList));
            }
            if (!DEAL.interactable && CardsList.Count == 1 && ((PassCounter < 3 && PlayingDeck.Single) || PassCounter >= 3))
            {
                StartCoroutine(CheckSingleCard(PassCounter, CardsList));
            }
            if (CardsList.Count == 0)
            {
                DEAL.interactable = false;
                PASS.interactable = true;
            }
        }
    }

    IEnumerator CheckFullHouseCard(int PassCounter, List<CardId> CIList)
    {
        if (CIList.Count == 5)
        {
            if (PassCounter < 3)
            {
                for (int c = 0; c < CIList.Count; c++)
                {
                    if ((int)CIList[c].CardNumber > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardNumber)
                    {
                        if (CIList.Count - c >= 5)
                        {
                            if ((int)CIList[c].CardNumber == (int)CIList[c + 1].CardNumber && (int)CIList[c + 1].CardNumber == (int)CIList[c + 2].CardNumber && (int)CIList[c + 3].CardNumber == (int)CIList[c + 4].CardNumber)
                            {
                                DEAL.interactable = true;
                                PASS.interactable = true;
                            }
                            else if ((int)CIList[c].CardNumber == (int)CIList[c + 1].CardNumber && (int)CIList[c + 2].CardNumber == (int)CIList[c + 3].CardNumber && (int)CIList[c + 3].CardNumber == (int)CIList[c + 4].CardNumber)
                            {
                                DEAL.interactable = true;
                                PASS.interactable = true;
                            }
                        }
                    }
                    else if ((int)CIList[c].CardNumber == (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardNumber && (int)CIList[c].CardIdentity > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardIdentity)
                    {
                        if (CIList.Count - c >= 5)
                        {
                            if ((int)CIList[c].CardNumber == (int)CIList[c + 1].CardNumber && (int)CIList[c + 1].CardNumber == (int)CIList[c + 2].CardNumber && (int)CIList[c + 3].CardNumber == (int)CIList[c + 4].CardNumber)
                            {
                                DEAL.interactable = true;
                                PASS.interactable = true;
                            }
                            else if ((int)CIList[c].CardNumber == (int)CIList[c + 1].CardNumber && (int)CIList[c + 2].CardNumber == (int)CIList[c + 3].CardNumber && (int)CIList[c + 3].CardNumber == (int)CIList[c + 4].CardNumber)
                            {
                                DEAL.interactable = true;
                                PASS.interactable = true;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int c = 0; c < CIList.Count; c++)
                {
                    if (CIList.Count - c >= 5)
                    {
                        if ((int)CIList[c].CardNumber == (int)CIList[c + 1].CardNumber && (int)CIList[c + 1].CardNumber == (int)CIList[c + 2].CardNumber && (int)CIList[c + 3].CardNumber == (int)CIList[c + 4].CardNumber)
                        {
                            PlayingDeck.FullHouseCard();
                            PlayingDeck.UpdateText("Full House");
                            DEAL.interactable = true;
                            PASS.interactable = true;
                        }
                        else if ((int)CIList[c].CardNumber == (int)CIList[c + 1].CardNumber && (int)CIList[c + 2].CardNumber == (int)CIList[c + 3].CardNumber && (int)CIList[c + 3].CardNumber == (int)CIList[c + 4].CardNumber)
                        {
                            PlayingDeck.FullHouseCard();
                            PlayingDeck.UpdateText("Full House");
                            DEAL.interactable = true;
                            PASS.interactable = true;
                        }
                    }
                }
            }
        }

        Debug.Log(this.transform.name + " Full House Checked ************* ");

        yield return null;
    }

    IEnumerator CheckRoyalFlushCard(int PassCounter, List<CardId> CIList)
    {
        if (CIList.Count == 5)
        {
            if (PassCounter < 3)
            {
                for (int c = 0; c < CIList.Count; c++)
                {
                    if ((int)CIList[c].CardNumber > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardNumber)
                    {
                        if (CIList.Count - c >= 5)
                        {
                            if ((int)CIList[c].CardIdentity == (int)CIList[c + 1].CardIdentity && (int)CIList[c + 1].CardIdentity == (int)CIList[c + 2].CardIdentity && (int)CIList[c + 2].CardIdentity == (int)CIList[c + 3].CardIdentity && (int)CIList[c + 3].CardIdentity == (int)CIList[c + 4].CardIdentity)
                            {
                                if ((int)CIList[c].CardNumber + 1 == (int)CIList[c + 1].CardNumber && (int)CIList[c + 1].CardNumber + 1 == (int)CIList[c + 2].CardNumber && (int)CIList[c + 2].CardNumber + 1 == (int)CIList[c + 3].CardNumber && (int)CIList[c + 3].CardNumber + 1 == (int)CIList[c + 4].CardNumber)
                                {
                                    DEAL.interactable = true;
                                    PASS.interactable = true;
                                }
                            }
                        }
                    }
                    else if ((int)CIList[c].CardNumber == (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardNumber && (int)CIList[c].CardIdentity > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardIdentity)
                    {
                        if (CIList.Count - c >= 5)
                        {
                            if ((int)CIList[c].CardIdentity == (int)CIList[c + 1].CardIdentity && (int)CIList[c + 1].CardIdentity == (int)CIList[c + 2].CardIdentity && (int)CIList[c + 2].CardIdentity == (int)CIList[c + 3].CardIdentity && (int)CIList[c + 3].CardIdentity == (int)CIList[c + 4].CardIdentity)
                            {
                                if ((int)CIList[c].CardNumber + 1 == (int)CIList[c + 1].CardNumber && (int)CIList[c + 1].CardNumber + 1 == (int)CIList[c + 2].CardNumber && (int)CIList[c + 2].CardNumber + 1 == (int)CIList[c + 3].CardNumber && (int)CIList[c + 3].CardNumber + 1 == (int)CIList[c + 4].CardNumber)
                                {
                                    DEAL.interactable = true;
                                    PASS.interactable = true;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                for (int c = 0; c < CIList.Count; c++)
                {
                    if (CIList.Count - c >= 5)
                    {
                        if ((int)CIList[c].CardIdentity == (int)CIList[c + 1].CardIdentity && (int)CIList[c + 1].CardIdentity == (int)CIList[c + 2].CardIdentity && (int)CIList[c + 2].CardIdentity == (int)CIList[c + 3].CardIdentity && (int)CIList[c + 3].CardIdentity == (int)CIList[c + 4].CardIdentity)
                        {
                            if ((int)CIList[c].CardNumber + 1 == (int)CIList[c + 1].CardNumber && (int)CIList[c + 1].CardNumber + 1 == (int)CIList[c + 2].CardNumber && (int)CIList[c + 2].CardNumber + 1 == (int)CIList[c + 3].CardNumber && (int)CIList[c + 3].CardNumber + 1 == (int)CIList[c + 4].CardNumber)
                            {
                                PlayingDeck.RoyalFlushCard();
                                PlayingDeck.UpdateText("Royal Flush");
                                DEAL.interactable = true;
                                PASS.interactable = true;
                            }
                        }
                    }
                }
            }
        }

        Debug.Log(this.transform.name + " Flush Card Checked ************* ");

        yield return null;
    }


    IEnumerator CheckFlushCard(int PassCounter, List<CardId> CIList)
    {
        if (CIList.Count == 5)
        {
            if (PassCounter < 3)
            {
                for (int c = 0; c < CIList.Count; c++)
                {
                    if ((int)CIList[c].CardNumber > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardNumber)
                    {
                        if (CIList.Count - c >= 5)
                        {
                            if ((int)CIList[c].CardIdentity == (int)CIList[c + 1].CardIdentity && (int)CIList[c + 1].CardIdentity == (int)CIList[c + 2].CardIdentity && (int)CIList[c + 2].CardIdentity == (int)CIList[c + 3].CardIdentity && (int)CIList[c + 3].CardIdentity == (int)CIList[c + 4].CardIdentity)
                            {
                                DEAL.interactable = true;
                                PASS.interactable = true;
                            }
                        }
                    }
                    else if ((int)CIList[c].CardNumber == (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardNumber && (int)CIList[c].CardIdentity > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardIdentity)
                    {
                        if (CIList.Count - c >= 5)
                        {
                            if ((int)CIList[c].CardIdentity == (int)CIList[c + 1].CardIdentity && (int)CIList[c + 1].CardIdentity == (int)CIList[c + 2].CardIdentity && (int)CIList[c + 2].CardIdentity == (int)CIList[c + 3].CardIdentity && (int)CIList[c + 3].CardIdentity == (int)CIList[c + 4].CardIdentity)
                            {
                                DEAL.interactable = true;
                                PASS.interactable = true;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int c = 0; c < CIList.Count; c++)
                {
                    if (CIList.Count - c >= 5)
                    {
                        if ((int)CIList[c].CardIdentity == (int)CIList[c + 1].CardIdentity && (int)CIList[c + 1].CardIdentity == (int)CIList[c + 2].CardIdentity && (int)CIList[c + 2].CardIdentity == (int)CIList[c + 3].CardIdentity && (int)CIList[c + 3].CardIdentity == (int)CIList[c + 4].CardIdentity)
                        {
                            PlayingDeck.FlushCard();
                            PlayingDeck.UpdateText("Flush");
                            DEAL.interactable = true;
                            PASS.interactable = true;
                        }
                    }
                }
            }
        }

        Debug.Log(this.transform.name + " Flush Card Checked ************* ");

        yield return null;
    }

    IEnumerator CheckStraightCard(int PassCounter, List<CardId> CIList)
    {
        if (CIList.Count == 5)
        {
            if (PassCounter < 3)
            {
                for (int c = 0; c < CIList.Count; c++)
                {
                    if ((int)CIList[c].CardNumber > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardNumber)
                    {
                        if (CIList.Count - c >= 5)
                        {
                            if ((int)CIList[c].CardNumber + 1 == (int)CIList[c + 1].CardNumber && (int)CIList[c + 1].CardNumber + 1 == (int)CIList[c + 2].CardNumber && (int)CIList[c + 2].CardNumber + 1 == (int)CIList[c + 3].CardNumber && (int)CIList[c + 3].CardNumber + 1 == (int)CIList[c + 4].CardNumber)
                            {
                                DEAL.interactable = true;
                                PASS.interactable = true;
                            }
                        }
                    }
                    else if ((int)CIList[c].CardNumber == (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardNumber && (int)CIList[c].CardIdentity > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardIdentity)
                    {
                        if (CIList.Count - c >= 5)
                        {
                            if ((int)CIList[c].CardNumber + 1 == (int)CIList[c + 1].CardNumber && (int)CIList[c + 1].CardNumber + 1 == (int)CIList[c + 2].CardNumber && (int)CIList[c + 2].CardNumber + 1 == (int)CIList[c + 3].CardNumber && (int)CIList[c + 3].CardNumber + 1 == (int)CIList[c + 4].CardNumber)
                            {
                                DEAL.interactable = true;
                                PASS.interactable = true;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int c = 0; c < CIList.Count; c++)
                {
                    if (CIList.Count - c >= 5)
                    {
                        if ((int)CIList[c].CardNumber + 1 == (int)CIList[c + 1].CardNumber && (int)CIList[c + 1].CardNumber + 1 == (int)CIList[c + 2].CardNumber && (int)CIList[c + 2].CardNumber + 1 == (int)CIList[c + 3].CardNumber && (int)CIList[c + 3].CardNumber + 1 == (int)CIList[c + 4].CardNumber)
                        {
                            PlayingDeck.StraightCard();
                            PlayingDeck.UpdateText("Straight");
                            DEAL.interactable = true;
                            PASS.interactable = true;
                        }
                    }
                }
            }
        }

        Debug.Log(this.transform.name + " Straight Card Checked ************* ");

        yield return null;
    }

    IEnumerator CheckFourOfKindCard(int PassCounter, List<CardId> CIList)
    {
        if (CIList.Count == 5)
        {
            if (PassCounter < 3)
            {
                for (int c = 0; c < CIList.Count; c++)
                {
                    if ((int)CIList[c].CardNumber > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardNumber)
                    {
                        if (CIList.Count - c >= 5)
                        {
                            if ((int)CIList[c].CardNumber == (int)CIList[c + 1].CardNumber && (int)CIList[c + 1].CardNumber == (int)CIList[c + 2].CardNumber && (int)CIList[c + 2].CardNumber == (int)CIList[c + 3].CardNumber)
                            {
                                DEAL.interactable = true;
                                PASS.interactable = true;
                            }
                            else if ((int)CIList[c + 1].CardNumber == (int)CIList[c + 2].CardNumber && (int)CIList[c + 2].CardNumber == (int)CIList[c + 3].CardNumber && (int)CIList[c + 3].CardNumber == (int)CIList[c + 4].CardNumber)
                            {
                                DEAL.interactable = true;
                                PASS.interactable = true;
                            }
                        }
                    }
                    else if ((int)CIList[c].CardNumber == (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardNumber && (int)CIList[c].CardIdentity > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardIdentity)
                    {
                        if (CIList.Count - c >= 5)
                        {
                            if ((int)CIList[c].CardNumber == (int)CIList[c + 1].CardNumber && (int)CIList[c + 1].CardNumber == (int)CIList[c + 2].CardNumber && (int)CIList[c + 2].CardNumber == (int)CIList[c + 3].CardNumber)
                            {
                                DEAL.interactable = true;
                                PASS.interactable = true;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int c = 0; c < CIList.Count; c++)
                {
                    if (CIList.Count - c >= 5)
                    {
                        if ((int)CIList[c].CardNumber == (int)CIList[c + 1].CardNumber && (int)CIList[c + 1].CardNumber == (int)CIList[c + 2].CardNumber && (int)CIList[c + 2].CardNumber == (int)CIList[c + 3].CardNumber)
                        {
                            PlayingDeck.FourOfKindCard();
                            PlayingDeck.UpdateText("Four Of A Kind");
                            DEAL.interactable = true;
                            PASS.interactable = true;
                        }
                    }
                }
            }
        }

        Debug.Log(this.transform.name + " Four Of Kind Card Checked ************* ");

        yield return null;
    }

    IEnumerator CheckThreeOfKindCard(int PassCounter, List<CardId> CIList)
    {
        if (CIList.Count == 3)
        {
            if (PassCounter < 3)
            {
                for (int c = 0; c < CIList.Count; c++)
                {
                    if ((int)CIList[c].CardNumber > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardNumber)
                    {
                        if (CIList.Count - c >= 3)
                        {
                            if ((int)CIList[c].CardNumber == (int)CIList[c + 1].CardNumber && (int)CIList[c + 1].CardNumber == (int)CIList[c + 2].CardNumber)
                            {
                                DEAL.interactable = true;
                                PASS.interactable = true;
                            }
                        }
                    }
                    else if ((int)CIList[c].CardNumber == (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardNumber && (int)CIList[c].CardIdentity > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardIdentity)
                    {
                        if (CIList.Count - c >= 3)
                        {
                            if ((int)CIList[c].CardNumber == (int)CIList[c + 1].CardNumber && (int)CIList[c + 1].CardNumber == (int)CIList[c + 2].CardNumber)
                            {
                                DEAL.interactable = true;
                                PASS.interactable = true;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int c = 0; c < CIList.Count; c++)
                {
                    if (CIList.Count - c >= 3)
                    {
                        if ((int)CIList[c].CardNumber == (int)CIList[c + 1].CardNumber && (int)CIList[c + 1].CardNumber == (int)CIList[c + 2].CardNumber)
                        {
                            PlayingDeck.ThreeOfKindCard();
                            PlayingDeck.UpdateText("Threes");
                            DEAL.interactable = true;
                            PASS.interactable = true;
                        }
                    }
                }
            }
        }

        Debug.Log(this.transform.name + " Threes Card Checked ************* ");

        yield return null;
    }

    IEnumerator CheckPairCard(int PassCounter, List<CardId> CIList)
    {
        if (CIList.Count == 2)
        {
            if (PassCounter < 3)
            {
                for (int c = 0; c < CIList.Count; c++)
                {
                    if ((int)CIList[c].CardNumber > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardNumber)
                    {
                        if (CIList.Count - c >= 2)
                        {
                            if ((int)CIList[c].CardNumber == (int)CIList[c + 1].CardNumber)
                            {
                                DEAL.interactable = true;
                                PASS.interactable = true;
                            }
                        }
                    }
                    else if ((int)CIList[c].CardNumber == (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardNumber && (int)CIList[c].CardIdentity > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardIdentity)
                    {
                        if (CIList.Count - c >= 2)
                        {
                            if ((int)CIList[c].CardNumber == (int)CIList[c + 1].CardNumber)
                            {
                                DEAL.interactable = true;
                                PASS.interactable = true;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int c = 0; c < CIList.Count; c++)
                {
                    if (CIList.Count - c >= 2)
                    {
                        if ((int)CIList[c].CardNumber == (int)CIList[c + 1].CardNumber)
                        {
                            PlayingDeck.PairCard();
                            PlayingDeck.UpdateText("Pair");
                            DEAL.interactable = true;
                            PASS.interactable = true;
                        }
                    }
                }
            }
        }

        Debug.Log(this.transform.name + " Pair Card Checked ************* ");

        yield return null;
    }

    IEnumerator CheckSingleCard(int PassCounter, List<CardId> CIList)
    {
        if (CIList.Count == 1)
        {
            if (PassCounter < 3)
            {
                if ((int)CIList[0].SingleCardNumber == (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().SingleCardNumber && (int)CIList[0].CardIdentity > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardIdentity)
                {
                    DEAL.interactable = true;
                    PASS.interactable = true;
                }
                else if ((int)CIList[0].SingleCardNumber > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().SingleCardNumber)
                {
                    DEAL.interactable = true;
                    PASS.interactable = true;
                }
            }
            else
            {
                PlayingDeck.SingleCard();
                CIList[0].transform.localPosition = new Vector3(CIList[0].transform.localPosition.x, 0.75f, CIList[0].transform.localPosition.z);
                DEAL.interactable = true;
                PASS.interactable = true;
            }
        }

        Debug.Log(this.transform.name + " Single Card Checked ************* ");

        yield return null;
    }

    public void TurnOnPassButton()
    {
        PASS.interactable = true;
        StartCoroutine(WaitUpdateText());
    }

    IEnumerator WaitUpdateText()
    {
        yield return new WaitForSeconds(0.3f);
        UpdateText("Your Turn");
    }

    public void ButtonToDeal()
    {
        PlayingDeck.CleanTheChild();
        for (int i = 0; i < CardsList.Count; i++)
        {
            CardsList[i].transform.parent = PlayingDeck.transform;
            CardsList[i].MoveTheCards();
        }
        this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
        PlayingDeck.ArrangeTheCards();
        CheckRemainingCards();
        if (NextPlayer.GetComponent<BotAI>() != null)
        {
            NextPlayer.GetComponent<BotAI>().PlayTurn(0);
            turn = false;
        }
        DEAL.interactable = false;
        PASS.interactable = false;
    }

    public void ButtonToPass()
    {
        Debug.Log(this.transform.name + " = Pass");
        UpdateText("Pass ( " + (PassCounter + 1).ToString() + " )");
        CB.Sad();
        TurnBackCardToPlayerDeck();
        SFXManager.main.Passing();
        this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
        PlayingDeck.ArrangeTheCards();
        if (NextPlayer.GetComponent<BotAI>() != null)
        {
            NextPlayer.GetComponent<BotAI>().PlayTurn(PassCounter + 1);
            turn = false;
        }

        if (PassCounter + 1 >= 3)
        {
            PlayingDeck.CleanTheChild();
        }
        DEAL.interactable = false;
        PASS.interactable = false;
    }

    public void UpdateText(string TextUpdt)
    {
        Text.SetActive(false);
        Text.GetComponent<Text>().text = TextUpdt;
        Text.SetActive(true);
    }

    public void TurnBackCardToPlayerDeck()
    {
        CardsList = new List<CardId>();

        for (int i = 0; i < SelectionDeck.childCount; i++)
        {
            CardsList.Add(SelectionDeck.GetChild(i).GetComponent<CardId>());
        }

        for (int i = 0; i < CardsList.Count; i++)
        {
            CardsList[i].transform.parent = this.transform;
        }
    }
}
