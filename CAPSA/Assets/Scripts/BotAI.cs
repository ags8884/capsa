using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BotAI : MonoBehaviour
{
    public GameObject Text;
    public bool turn;
    public GameObject[] Cards;
    public GameObject[] CardsToBeSetToDeck;
    public GameObject PlayingDeck;
    public GameObject NextPlayer;

    public int SpadeCardsCount;
    public int HeartCardsCount;
    public int ClubCardsCount;
    public int DiamondCardsCount;

    public GameObject[] SpadeCards;
    public GameObject[] HeartCards;
    public GameObject[] ClubCards;
    public GameObject[] DiamondCards;

    public Dictionary<int, int> PairsDic;
    public Dictionary<int, int> RoyalFlushDic;
    public List<int> CardNumberList;
    public List<int> CardSuitList;
    string SuitName;

    //Royal Flush And Flush Variable
    public int cardnumber;
    public int identity;
    public int value;


    // Use this for initialization
    void Start()
    {
        Text.SetActive(false);
    }

    public void CheckThreeDiamond()
    {
        Cards = new GameObject[this.transform.childCount];

        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i] = this.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < Cards.Length; i++)
        {
            if ((int)Cards[i].GetComponent<CardId>().CardNumber == 3 && (int)Cards[i].GetComponent<CardId>().CardIdentity == 1)
            {
                PlayingDeck.GetComponent<PlayingDeckScript>().CleanTheChild();
                Cards[i].transform.parent = PlayingDeck.transform;
                Cards[i].GetComponent<CardId>().MoveTheCards();
                this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
                PlayingDeck.GetComponent<PlayingDeckScript>().SingleCard();
                PlayingDeck.GetComponent<PlayingDeckScript>().ArrangeTheCards();
                if (NextPlayer.GetComponent<BotAI>() != null)
                {
                    NextPlayer.GetComponent<BotAI>().PlayTurn(0);
                    turn = false;
                } else if (NextPlayer.GetComponent<PlayerBehaviour>() != null)
                {
                    NextPlayer.GetComponent<PlayerBehaviour>().PassCounter = 0;
                    NextPlayer.GetComponent<PlayerBehaviour>().turn = true;
                    NextPlayer.GetComponent<PlayerBehaviour>().TurnOnPassButton();
                    turn = false;
                }
            }
        }
    }

    public void PlayTurn(int PassCounter)
    {
        if (PassCounter < 3)
        {
            StartCoroutine(WaitPlayTurn(PassCounter));
            Debug.Log(this.transform.name + " : Wait Play Turn, passcounter " + PassCounter);
        }
        else
        {
            PassCounter = 0;
            StartCoroutine(FirstPlayTurn(PassCounter));
            Debug.Log(this.transform.name + " : First Play Turn, passcounter " + PassCounter);
        }
    }

    public void CheckRemainingCards() {
        StartCoroutine(CheckRemainingCardsEnum());
    }

    IEnumerator CheckRemainingCardsEnum()
    {
        yield return new WaitForSeconds(0.3f);
        Cards = new GameObject[this.transform.childCount];
        if (Cards.Length <= 0)
        {
            Text.SetActive(false);
            Text.GetComponent<Text>().text = this.transform.name + " WIN!!";
            Text.SetActive(true);
            Time.timeScale = 0;
        }
    }

    IEnumerator WaitPlayTurn(int PassCounter)
    {
        yield return new WaitForSeconds(1);
        turn = true;

        if (PlayingDeck.GetComponent<PlayingDeckScript>().Single)
        {

            Cards = new GameObject[this.transform.childCount];

            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i] = this.transform.GetChild(i).gameObject;
            }

            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i].transform.parent = null;
            }

            for (int a = 3; a <= 15; a++)
            {
                for (int i = 1; i <= 4; i++)
                {
                    for (int s = 0; s < Cards.Length; s++)
                    {
                        if ((int)Cards[s].GetComponent<CardId>().CardIdentity == i && (int)Cards[s].GetComponent<CardId>().SingleCardNumber == a)
                        {
                            Cards[s].transform.parent = this.transform;
                        }
                    }
                }
            }

            for (int i = 0; i < Cards.Length; i++)
            {
                if ((int)Cards[i].GetComponent<CardId>().SingleCardNumber == (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().SingleCardNumber && (int)Cards[i].GetComponent<CardId>().CardIdentity > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardIdentity)
                {
                    PlayingDeck.GetComponent<PlayingDeckScript>().CleanTheChild();
                    Cards[i].transform.parent = PlayingDeck.transform;
                    Cards[i].GetComponent<CardId>().MoveTheCards();
                    this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
                    PlayingDeck.GetComponent<PlayingDeckScript>().SingleCard();
                    PlayingDeck.GetComponent<PlayingDeckScript>().ArrangeTheCards();
                    CheckRemainingCards();
                    if (NextPlayer.GetComponent<BotAI>() != null)
                    {
                        NextPlayer.GetComponent<BotAI>().PlayTurn(0);
                        turn = false;
                    }
                    else if (NextPlayer.GetComponent<PlayerBehaviour>() != null)
                    {
                        NextPlayer.GetComponent<PlayerBehaviour>().PassCounter = 0;
                        NextPlayer.GetComponent<PlayerBehaviour>().turn = true;
                        NextPlayer.GetComponent<PlayerBehaviour>().TurnOnPassButton();
                        turn = false;
                    }
                    i = Cards.Length;
                }
                else if ((int)Cards[i].GetComponent<CardId>().SingleCardNumber > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().SingleCardNumber)
                {
                    PlayingDeck.GetComponent<PlayingDeckScript>().CleanTheChild();
                    Cards[i].transform.parent = PlayingDeck.transform;
                    Cards[i].GetComponent<CardId>().MoveTheCards();
                    this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
                    PlayingDeck.GetComponent<PlayingDeckScript>().SingleCard();
                    PlayingDeck.GetComponent<PlayingDeckScript>().ArrangeTheCards();
                    CheckRemainingCards();
                    if (NextPlayer.GetComponent<BotAI>() != null)
                    {
                        NextPlayer.GetComponent<BotAI>().PlayTurn(0);
                        turn = false;
                    }
                    else if (NextPlayer.GetComponent<PlayerBehaviour>() != null)
                    {
                        NextPlayer.GetComponent<PlayerBehaviour>().PassCounter = 0;
                        NextPlayer.GetComponent<PlayerBehaviour>().turn = true;
                        NextPlayer.GetComponent<PlayerBehaviour>().TurnOnPassButton();
                        turn = false;
                    }
                    i = Cards.Length;
                }
                else if (i == Cards.Length - 1)
                {
                    Debug.Log(this.transform.name + " = Pass");
                    Text.SetActive(false);
                    Text.GetComponent<Text>().text = "Pass ( " + (PassCounter + 1).ToString() + " )";
                    Text.SetActive(true);
                    this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
                    PlayingDeck.GetComponent<PlayingDeckScript>().SingleCard();
                    PlayingDeck.GetComponent<PlayingDeckScript>().ArrangeTheCards();
                    if (NextPlayer.GetComponent<BotAI>() != null)
                    {
                        NextPlayer.GetComponent<BotAI>().PlayTurn(PassCounter + 1);
                        turn = false;
                    }
                    else if (NextPlayer.GetComponent<PlayerBehaviour>() != null)
                    {
                        NextPlayer.GetComponent<PlayerBehaviour>().PassCounter = PassCounter + 1;
                        NextPlayer.GetComponent<PlayerBehaviour>().turn = true;
                        NextPlayer.GetComponent<PlayerBehaviour>().TurnOnPassButton();
                        turn = false;
                    }
                    i = Cards.Length;
                }
            }
        }

        if (PlayingDeck.GetComponent<PlayingDeckScript>().RoyalFlush)
        {

            Cards = new GameObject[this.transform.childCount];

            if (Cards.Length >= 5)
            {
                for (int i = 0; i < Cards.Length; i++)
                {
                    Cards[i] = this.transform.GetChild(i).gameObject;
                }

                for (int i = 0; i < Cards.Length; i++)
                {
                    Cards[i].transform.parent = null;
                }


                for (int s = 1; s <= 4; s++)
                {
                    for (int i = 2; i <= 14; i++)
                    {
                        for (int c = 0; c < Cards.Length; c++)
                        {
                            if ((int)Cards[c].GetComponent<CardId>().CardNumber == i && (int)Cards[c].GetComponent<CardId>().CardIdentity == s)
                            {
                                Cards[c].transform.parent = this.transform;
                            }
                        }
                    }
                }

                for (int c = 0; c < Cards.Length; c++)
                {
                    if ((int)Cards[c].GetComponent<CardId>().CardNumber > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardNumber)
                    {
                        if (Cards.Length - c >= 5)
                        {
                            if ((int)Cards[c].GetComponent<CardId>().CardNumber + 1 == (int)Cards[c + 1].GetComponent<CardId>().CardNumber && (int)Cards[c + 1].GetComponent<CardId>().CardNumber + 1 == (int)Cards[c + 2].GetComponent<CardId>().CardNumber && (int)Cards[c + 2].GetComponent<CardId>().CardNumber + 1 == (int)Cards[c + 3].GetComponent<CardId>().CardNumber && (int)Cards[c + 3].GetComponent<CardId>().CardNumber + 1 == (int)Cards[c + 4].GetComponent<CardId>().CardNumber)
                            {
                                PlayingDeck.GetComponent<PlayingDeckScript>().CleanTheChild();

                                Cards[c].transform.parent = PlayingDeck.transform;
                                Cards[c].GetComponent<CardId>().MoveTheCards();
                                Cards[c + 1].transform.parent = PlayingDeck.transform;
                                Cards[c + 1].GetComponent<CardId>().MoveTheCards();
                                Cards[c + 2].transform.parent = PlayingDeck.transform;
                                Cards[c + 2].GetComponent<CardId>().MoveTheCards();
                                Cards[c + 3].transform.parent = PlayingDeck.transform;
                                Cards[c + 3].GetComponent<CardId>().MoveTheCards();
                                Cards[c + 4].transform.parent = PlayingDeck.transform;
                                Cards[c + 4].GetComponent<CardId>().MoveTheCards();

                                this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
                                CheckRemainingCards();
                                PlayingDeck.GetComponent<PlayingDeckScript>().RoyalFlushCard();
                                PlayingDeck.GetComponent<PlayingDeckScript>().ArrangeTheCards();

                                if (NextPlayer.GetComponent<BotAI>() != null)
                                {
                                    NextPlayer.GetComponent<BotAI>().PlayTurn(0);
                                    turn = false;
                                }
                                else if (NextPlayer.GetComponent<PlayerBehaviour>() != null)
                                {
                                    NextPlayer.GetComponent<PlayerBehaviour>().PassCounter = 0;
                                    NextPlayer.GetComponent<PlayerBehaviour>().turn = true;
                                    NextPlayer.GetComponent<PlayerBehaviour>().TurnOnPassButton();
                                    turn = false;
                                }
                                c = Cards.Length;
                            }
                        }
                    }
                }
            }

            if (turn)
            {
                Debug.Log(this.transform.name + " = Pass");
                Text.SetActive(false);
                Text.GetComponent<Text>().text = "Pass ( " + (PassCounter + 1).ToString() + " )";
                Text.SetActive(true);
                this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
                PlayingDeck.GetComponent<PlayingDeckScript>().RoyalFlushCard();
                PlayingDeck.GetComponent<PlayingDeckScript>().ArrangeTheCards();
                if (NextPlayer.GetComponent<BotAI>() != null)
                {
                    NextPlayer.GetComponent<BotAI>().PlayTurn(PassCounter + 1);
                    turn = false;
                }
                else if (NextPlayer.GetComponent<PlayerBehaviour>() != null)
                {
                    NextPlayer.GetComponent<PlayerBehaviour>().PassCounter = PassCounter + 1;
                    NextPlayer.GetComponent<PlayerBehaviour>().turn = true;
                    NextPlayer.GetComponent<PlayerBehaviour>().TurnOnPassButton();
                    turn = false;
                }
            }
        }

        if (PlayingDeck.GetComponent<PlayingDeckScript>().Flush)
        {

            Cards = new GameObject[this.transform.childCount];

            if (Cards.Length >= 5)
            {
                for (int i = 0; i < Cards.Length; i++)
                {
                    Cards[i] = this.transform.GetChild(i).gameObject;
                }

                for (int i = 0; i < Cards.Length; i++)
                {
                    Cards[i].transform.parent = null;
                }


                for (int s = 1; s <= 4; s++)
                {
                    for (int c = 0; c < Cards.Length; c++)
                    {
                        if ((int)Cards[c].GetComponent<CardId>().CardIdentity == s)
                        {
                            Cards[c].transform.parent = this.transform;
                        }
                    }
                }

                for (int c = 0; c < Cards.Length; c++)
                {
                    if ((int)Cards[c].GetComponent<CardId>().CardNumber > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardNumber)
                    {
                        if (Cards.Length - c >= 5)
                        {
                            if ((int)Cards[c].GetComponent<CardId>().CardIdentity == (int)Cards[c + 1].GetComponent<CardId>().CardIdentity && (int)Cards[c + 1].GetComponent<CardId>().CardIdentity == (int)Cards[c + 2].GetComponent<CardId>().CardIdentity && (int)Cards[c + 2].GetComponent<CardId>().CardIdentity == (int)Cards[c + 3].GetComponent<CardId>().CardIdentity && (int)Cards[c + 3].GetComponent<CardId>().CardIdentity == (int)Cards[c + 4].GetComponent<CardId>().CardIdentity)
                            {
                                PlayingDeck.GetComponent<PlayingDeckScript>().CleanTheChild();

                                Cards[c].transform.parent = PlayingDeck.transform;
                                Cards[c].GetComponent<CardId>().MoveTheCards();
                                Cards[c + 1].transform.parent = PlayingDeck.transform;
                                Cards[c + 1].GetComponent<CardId>().MoveTheCards();
                                Cards[c + 2].transform.parent = PlayingDeck.transform;
                                Cards[c + 2].GetComponent<CardId>().MoveTheCards();
                                Cards[c + 3].transform.parent = PlayingDeck.transform;
                                Cards[c + 3].GetComponent<CardId>().MoveTheCards();
                                Cards[c + 4].transform.parent = PlayingDeck.transform;
                                Cards[c + 4].GetComponent<CardId>().MoveTheCards();

                                this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
                                CheckRemainingCards();
                                PlayingDeck.GetComponent<PlayingDeckScript>().FlushCard();
                                PlayingDeck.GetComponent<PlayingDeckScript>().ArrangeTheCards();

                                if (NextPlayer.GetComponent<BotAI>() != null)
                                {
                                    NextPlayer.GetComponent<BotAI>().PlayTurn(0);
                                    turn = false;
                                }
                                else if (NextPlayer.GetComponent<PlayerBehaviour>() != null)
                                {
                                    NextPlayer.GetComponent<PlayerBehaviour>().PassCounter = 0;
                                    NextPlayer.GetComponent<PlayerBehaviour>().turn = true;
                                    NextPlayer.GetComponent<PlayerBehaviour>().TurnOnPassButton();
                                    turn = false;
                                }
                                c = Cards.Length;
                            }
                        }
                    }
                }
            }

            if (turn)
            {
                Debug.Log(this.transform.name + " = Pass");
                Text.SetActive(false);
                Text.GetComponent<Text>().text = "Pass ( " + (PassCounter + 1).ToString() + " )";
                Text.SetActive(true);
                this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
                PlayingDeck.GetComponent<PlayingDeckScript>().FlushCard();
                PlayingDeck.GetComponent<PlayingDeckScript>().ArrangeTheCards();
                if (NextPlayer.GetComponent<BotAI>() != null)
                {
                    NextPlayer.GetComponent<BotAI>().PlayTurn(PassCounter + 1);
                    turn = false;
                }
                else if (NextPlayer.GetComponent<PlayerBehaviour>() != null)
                {
                    NextPlayer.GetComponent<PlayerBehaviour>().PassCounter = PassCounter + 1;
                    NextPlayer.GetComponent<PlayerBehaviour>().turn = true;
                    NextPlayer.GetComponent<PlayerBehaviour>().TurnOnPassButton();
                    turn = false;
                }
            }
        }


        if (PlayingDeck.GetComponent<PlayingDeckScript>().Straight)
        {
            Cards = new GameObject[this.transform.childCount];

            if (Cards.Length >= 5)
            {
                for (int i = 0; i < Cards.Length; i++)
                {
                    Cards[i] = this.transform.GetChild(i).gameObject;
                }

                for (int i = 0; i < Cards.Length; i++)
                {
                    Cards[i].transform.parent = null;
                }


                for (int c = 0; c < Cards.Length; c++)
                {
                    for (int s = 2; s <= 14; s++)
                    {
                        if ((int)Cards[c].GetComponent<CardId>().CardNumber == s)
                        {
                            Cards[c].transform.parent = this.transform;
                        }
                    }
                }

                for (int c = 0; c < Cards.Length; c++)
                {
                    if ((int)Cards[c].GetComponent<CardId>().CardNumber > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardNumber)
                    {
                        if (Cards.Length - c >= 5)
                        {
                            if ((int)Cards[c].GetComponent<CardId>().CardNumber + 1 == (int)Cards[c + 1].GetComponent<CardId>().CardNumber && (int)Cards[c + 1].GetComponent<CardId>().CardNumber + 1 == (int)Cards[c + 2].GetComponent<CardId>().CardNumber && (int)Cards[c + 2].GetComponent<CardId>().CardNumber + 1 == (int)Cards[c + 3].GetComponent<CardId>().CardNumber && (int)Cards[c + 3].GetComponent<CardId>().CardNumber + 1 == (int)Cards[c + 4].GetComponent<CardId>().CardNumber)
                            {
                                PlayingDeck.GetComponent<PlayingDeckScript>().CleanTheChild();

                                Cards[c].transform.parent = PlayingDeck.transform;
                                Cards[c].GetComponent<CardId>().MoveTheCards();
                                Cards[c + 1].transform.parent = PlayingDeck.transform;
                                Cards[c + 1].GetComponent<CardId>().MoveTheCards();
                                Cards[c + 2].transform.parent = PlayingDeck.transform;
                                Cards[c + 2].GetComponent<CardId>().MoveTheCards();
                                Cards[c + 3].transform.parent = PlayingDeck.transform;
                                Cards[c + 3].GetComponent<CardId>().MoveTheCards();
                                Cards[c + 4].transform.parent = PlayingDeck.transform;
                                Cards[c + 4].GetComponent<CardId>().MoveTheCards();

                                this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
                                CheckRemainingCards();
                                PlayingDeck.GetComponent<PlayingDeckScript>().StraightCard();
                                PlayingDeck.GetComponent<PlayingDeckScript>().ArrangeTheCards();

                                if (NextPlayer.GetComponent<BotAI>() != null)
                                {
                                    NextPlayer.GetComponent<BotAI>().PlayTurn(0);
                                    turn = false;
                                }
                                else if (NextPlayer.GetComponent<PlayerBehaviour>() != null)
                                {
                                    NextPlayer.GetComponent<PlayerBehaviour>().PassCounter = 0;
                                    NextPlayer.GetComponent<PlayerBehaviour>().turn = true;
                                    NextPlayer.GetComponent<PlayerBehaviour>().TurnOnPassButton();
                                    turn = false;
                                }
                                c = Cards.Length;
                            }
                        }
                    }
                }
            }

            if (turn)
            {
                Debug.Log(this.transform.name + " = Pass");
                Text.SetActive(false);
                Text.GetComponent<Text>().text = "Pass ( " + (PassCounter + 1).ToString() + " )";
                Text.SetActive(true);
                this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
                PlayingDeck.GetComponent<PlayingDeckScript>().StraightCard();
                PlayingDeck.GetComponent<PlayingDeckScript>().ArrangeTheCards();
                if (NextPlayer.GetComponent<BotAI>() != null)
                {
                    NextPlayer.GetComponent<BotAI>().PlayTurn(PassCounter + 1);
                    turn = false;
                }
                else if (NextPlayer.GetComponent<PlayerBehaviour>() != null)
                {
                    NextPlayer.GetComponent<PlayerBehaviour>().PassCounter = PassCounter + 1;
                    NextPlayer.GetComponent<PlayerBehaviour>().turn = true;
                    NextPlayer.GetComponent<PlayerBehaviour>().TurnOnPassButton();
                    turn = false;
                }
            }
        }


        if (PlayingDeck.GetComponent<PlayingDeckScript>().FourOfKind)
        {
            Cards = new GameObject[this.transform.childCount];

            if (Cards.Length >= 5)
            {
                for (int i = 0; i < Cards.Length; i++)
                {
                    Cards[i] = this.transform.GetChild(i).gameObject;
                }

                for (int i = 0; i < Cards.Length; i++)
                {
                    Cards[i].transform.parent = null;
                }

                for (int s = 2; s <= 14; s++)
                {
                    for (int c = 0; c < Cards.Length; c++)
                    {
                        if ((int)Cards[c].GetComponent<CardId>().CardNumber == s)
                        {
                            Cards[c].transform.parent = this.transform;
                        }
                    }
                }

                for (int c = 0; c < Cards.Length; c++)
                {
                    if ((int)Cards[c].GetComponent<CardId>().CardNumber > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardNumber)
                    {
                        if (Cards.Length - c >= 5)
                        {
                            if ((int)Cards[c].GetComponent<CardId>().CardNumber == (int)Cards[c + 1].GetComponent<CardId>().CardNumber && (int)Cards[c + 1].GetComponent<CardId>().CardNumber == (int)Cards[c + 2].GetComponent<CardId>().CardNumber && (int)Cards[c + 2].GetComponent<CardId>().CardNumber == (int)Cards[c + 3].GetComponent<CardId>().CardNumber)
                            {
                                PlayingDeck.GetComponent<PlayingDeckScript>().CleanTheChild();

                                Cards[c].transform.parent = PlayingDeck.transform;
                                Cards[c].GetComponent<CardId>().MoveTheCards();
                                Cards[c + 1].transform.parent = PlayingDeck.transform;
                                Cards[c + 1].GetComponent<CardId>().MoveTheCards();
                                Cards[c + 2].transform.parent = PlayingDeck.transform;
                                Cards[c + 2].GetComponent<CardId>().MoveTheCards();
                                Cards[c + 3].transform.parent = PlayingDeck.transform;
                                Cards[c + 3].GetComponent<CardId>().MoveTheCards();
                                Cards[c + 4].transform.parent = PlayingDeck.transform;
                                Cards[c + 4].GetComponent<CardId>().MoveTheCards();

                                this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
                                CheckRemainingCards();
                                PlayingDeck.GetComponent<PlayingDeckScript>().FourOfKindCard();
                                PlayingDeck.GetComponent<PlayingDeckScript>().ArrangeTheCards();

                                if (NextPlayer.GetComponent<BotAI>() != null)
                                {
                                    NextPlayer.GetComponent<BotAI>().PlayTurn(0);
                                    turn = false;
                                }
                                else if (NextPlayer.GetComponent<PlayerBehaviour>() != null)
                                {
                                    NextPlayer.GetComponent<PlayerBehaviour>().PassCounter = 0;
                                    NextPlayer.GetComponent<PlayerBehaviour>().turn = true;
                                    NextPlayer.GetComponent<PlayerBehaviour>().TurnOnPassButton();
                                    turn = false;
                                }
                                c = Cards.Length;
                                Debug.Log(this.transform.name + " FourOfKindTester");
                            }
                        }
                    }
                }
            }

            if (turn)
            {
                Debug.Log(this.transform.name + " = Pass");
                Text.SetActive(false);
                Text.GetComponent<Text>().text = "Pass ( " + (PassCounter + 1).ToString() + " )";
                Text.SetActive(true);
                this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
                PlayingDeck.GetComponent<PlayingDeckScript>().FourOfKindCard();
                PlayingDeck.GetComponent<PlayingDeckScript>().ArrangeTheCards();
                if (NextPlayer.GetComponent<BotAI>() != null)
                {
                    NextPlayer.GetComponent<BotAI>().PlayTurn(PassCounter + 1);
                    turn = false;
                }
                else if (NextPlayer.GetComponent<PlayerBehaviour>() != null)
                {
                    NextPlayer.GetComponent<PlayerBehaviour>().PassCounter = PassCounter + 1;
                    NextPlayer.GetComponent<PlayerBehaviour>().turn = true;
                    NextPlayer.GetComponent<PlayerBehaviour>().TurnOnPassButton();
                    turn = false;
                }
            }
        }

        if (PlayingDeck.GetComponent<PlayingDeckScript>().ThreeOfKind)
        {
            Cards = new GameObject[this.transform.childCount];

            if (Cards.Length >= 3)
            {
                for (int i = 0; i < Cards.Length; i++)
                {
                    Cards[i] = this.transform.GetChild(i).gameObject;
                }

                for (int i = 0; i < Cards.Length; i++)
                {
                    Cards[i].transform.parent = null;
                }

                for (int s = 2; s <= 14; s++)
                {
                    for (int c = 0; c < Cards.Length; c++)
                    {
                        if ((int)Cards[c].GetComponent<CardId>().CardNumber == s)
                        {
                            Cards[c].transform.parent = this.transform;
                        }
                    }
                }

                for (int c = 0; c < Cards.Length; c++)
                {
                    if ((int)Cards[c].GetComponent<CardId>().CardNumber > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardNumber)
                    {
                        if (Cards.Length - c >= 3)
                        {
                            if ((int)Cards[c].GetComponent<CardId>().CardNumber == (int)Cards[c + 1].GetComponent<CardId>().CardNumber && (int)Cards[c + 1].GetComponent<CardId>().CardNumber == (int)Cards[c + 2].GetComponent<CardId>().CardNumber)
                            {
                                PlayingDeck.GetComponent<PlayingDeckScript>().CleanTheChild();

                                Cards[c].transform.parent = PlayingDeck.transform;
                                Cards[c].GetComponent<CardId>().MoveTheCards();
                                Cards[c + 1].transform.parent = PlayingDeck.transform;
                                Cards[c + 1].GetComponent<CardId>().MoveTheCards();
                                Cards[c + 2].transform.parent = PlayingDeck.transform;
                                Cards[c + 2].GetComponent<CardId>().MoveTheCards();

                                this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
                                CheckRemainingCards();
                                PlayingDeck.GetComponent<PlayingDeckScript>().ThreeOfKindCard();
                                PlayingDeck.GetComponent<PlayingDeckScript>().ArrangeTheCards();

                                if (NextPlayer.GetComponent<BotAI>() != null)
                                {
                                    NextPlayer.GetComponent<BotAI>().PlayTurn(0);
                                    turn = false;
                                }
                                else if (NextPlayer.GetComponent<PlayerBehaviour>() != null)
                                {
                                    NextPlayer.GetComponent<PlayerBehaviour>().PassCounter = 0;
                                    NextPlayer.GetComponent<PlayerBehaviour>().turn = true;
                                    NextPlayer.GetComponent<PlayerBehaviour>().TurnOnPassButton();
                                    turn = false;
                                }
                                c = Cards.Length;
                            }
                            Debug.Log(this.transform.name + " ThreeOfKindTester");
                        }
                    }
                }
            }

            if (turn)
            {
                Debug.Log(this.transform.name + " = Pass");
                Text.SetActive(false);
                Text.GetComponent<Text>().text = "Pass ( " + (PassCounter + 1).ToString() + " )";
                Text.SetActive(true);
                this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
                PlayingDeck.GetComponent<PlayingDeckScript>().ThreeOfKindCard();
                PlayingDeck.GetComponent<PlayingDeckScript>().ArrangeTheCards();
                if (NextPlayer.GetComponent<BotAI>() != null)
                {
                    NextPlayer.GetComponent<BotAI>().PlayTurn(PassCounter + 1);
                    turn = false;
                }
                else if (NextPlayer.GetComponent<PlayerBehaviour>() != null)
                {
                    NextPlayer.GetComponent<PlayerBehaviour>().PassCounter = PassCounter + 1;
                    NextPlayer.GetComponent<PlayerBehaviour>().turn = true;
                    NextPlayer.GetComponent<PlayerBehaviour>().TurnOnPassButton();
                    turn = false;
                }
            }
        }

        if (PlayingDeck.GetComponent<PlayingDeckScript>().Pair)
        {

            Cards = new GameObject[this.transform.childCount];

            if (Cards.Length >= 2)
            {
                for (int i = 0; i < Cards.Length; i++)
                {
                    Cards[i] = this.transform.GetChild(i).gameObject;
                }

                for (int i = 0; i < Cards.Length; i++)
                {
                    Cards[i].transform.parent = null;
                }

                for (int s = 2; s <= 14; s++)
                {
                    for (int c = 0; c < Cards.Length; c++)
                    {
                        if ((int)Cards[c].GetComponent<CardId>().CardNumber == s)
                        {
                            Cards[c].transform.parent = this.transform;
                        }
                    }
                }

                for (int c = 0; c < Cards.Length; c++)
                {
                    if ((int)Cards[c].GetComponent<CardId>().CardNumber > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardNumber)
                    {
                        if (Cards.Length - c >= 2)
                        {
                            if ((int)Cards[c].GetComponent<CardId>().CardNumber == (int)Cards[c + 1].GetComponent<CardId>().CardNumber)
                            {
                                PlayingDeck.GetComponent<PlayingDeckScript>().CleanTheChild();

                                Cards[c].transform.parent = PlayingDeck.transform;
                                Cards[c].GetComponent<CardId>().MoveTheCards();
                                Cards[c + 1].transform.parent = PlayingDeck.transform;
                                Cards[c + 1].GetComponent<CardId>().MoveTheCards();

                                this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
                                CheckRemainingCards();
                                PlayingDeck.GetComponent<PlayingDeckScript>().PairCard();
                                PlayingDeck.GetComponent<PlayingDeckScript>().ArrangeTheCards();

                                if (NextPlayer.GetComponent<BotAI>() != null)
                                {
                                    NextPlayer.GetComponent<BotAI>().PlayTurn(0);
                                    turn = false;
                                }
                                else if (NextPlayer.GetComponent<PlayerBehaviour>() != null)
                                {
                                    NextPlayer.GetComponent<PlayerBehaviour>().PassCounter = 0;
                                    NextPlayer.GetComponent<PlayerBehaviour>().turn = true;
                                    NextPlayer.GetComponent<PlayerBehaviour>().TurnOnPassButton();
                                    turn = false;
                                }
                                c = Cards.Length;
                            }
                            Debug.Log(this.transform.name + " PairOfKindTester");
                        }
                    }
                }
            }

            if (turn)
            {
                Debug.Log(this.transform.name + " = Pass");
                Text.SetActive(false);
                Text.GetComponent<Text>().text = "Pass ( " + (PassCounter + 1).ToString() + " )";
                Text.SetActive(true);
                this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
                PlayingDeck.GetComponent<PlayingDeckScript>().PairCard();
                PlayingDeck.GetComponent<PlayingDeckScript>().ArrangeTheCards();
                if (NextPlayer.GetComponent<BotAI>() != null)
                {
                    NextPlayer.GetComponent<BotAI>().PlayTurn(PassCounter + 1);
                    turn = false;
                }
                else if (NextPlayer.GetComponent<PlayerBehaviour>() != null)
                {
                    NextPlayer.GetComponent<PlayerBehaviour>().PassCounter = PassCounter + 1;
                    NextPlayer.GetComponent<PlayerBehaviour>().turn = true;
                    NextPlayer.GetComponent<PlayerBehaviour>().TurnOnPassButton();
                    turn = false;
                }
            }
        }
    }

    IEnumerator FirstPlayTurn(int PassCounter)
    {
        turn = true;
        yield return new WaitForSeconds(0.1f);
        if (turn)
        {
            StartCoroutine(CheckRoyalFlush(PassCounter));
        }
        yield return new WaitForSeconds(0.1f);
        if (turn)
        {
            StartCoroutine(CheckFlush(PassCounter));
        }
        yield return new WaitForSeconds(0.1f);
        if (turn)
        {
            StartCoroutine(CheckStraight(PassCounter));
        }
        yield return new WaitForSeconds(0.1f);
        if (turn)
        {
            StartCoroutine(CheckFourOfKind(PassCounter));
        }
        yield return new WaitForSeconds(0.1f);
        if (turn)
        {
            StartCoroutine(CheckThreeOfKind(PassCounter));
        }
        yield return new WaitForSeconds(0.1f);
        if (turn)
        {
            StartCoroutine(CheckPair(PassCounter));
        }
        yield return new WaitForSeconds(0.1f);
        if (turn)
        {
            StartCoroutine(SingleCard(PassCounter));
        }
    }

    IEnumerator CheckRoyalFlush(int PassCounter)
    {
        Cards = new GameObject[this.transform.childCount];

        if (Cards.Length >= 5)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i] = this.transform.GetChild(i).gameObject;
            }

            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i].transform.parent = null;
            }


            for (int s = 1; s <= 4; s++)
            {
                for (int i = 2; i <= 14; i++)
                {
                    for (int c = 0; c < Cards.Length; c++)
                    {
                        if ((int)Cards[c].GetComponent<CardId>().CardNumber == i && (int)Cards[c].GetComponent<CardId>().CardIdentity == s)
                        {
                            Cards[c].transform.parent = this.transform;
                        }
                    }
                }
            }

            for (int c = 0; c < Cards.Length; c++)
            {
                if (Cards.Length - c >= 5)
                {
                    if ((int)Cards[c].GetComponent<CardId>().CardNumber + 1 == (int)Cards[c + 1].GetComponent<CardId>().CardNumber && (int)Cards[c + 1].GetComponent<CardId>().CardNumber + 1 == (int)Cards[c + 2].GetComponent<CardId>().CardNumber && (int)Cards[c + 2].GetComponent<CardId>().CardNumber + 1 == (int)Cards[c + 3].GetComponent<CardId>().CardNumber && (int)Cards[c + 3].GetComponent<CardId>().CardNumber + 1 == (int)Cards[c + 4].GetComponent<CardId>().CardNumber)
                    {
                        PlayingDeck.GetComponent<PlayingDeckScript>().CleanTheChild();

                        Cards[c].transform.parent = PlayingDeck.transform;
                        Cards[c].GetComponent<CardId>().MoveTheCards();
                        Cards[c + 1].transform.parent = PlayingDeck.transform;
                        Cards[c + 1].GetComponent<CardId>().MoveTheCards();
                        Cards[c + 2].transform.parent = PlayingDeck.transform;
                        Cards[c + 2].GetComponent<CardId>().MoveTheCards();
                        Cards[c + 3].transform.parent = PlayingDeck.transform;
                        Cards[c + 3].GetComponent<CardId>().MoveTheCards();
                        Cards[c + 4].transform.parent = PlayingDeck.transform;
                        Cards[c + 4].GetComponent<CardId>().MoveTheCards();

                        this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
                        CheckRemainingCards();
                        PlayingDeck.GetComponent<PlayingDeckScript>().RoyalFlushCard();
                        PlayingDeck.GetComponent<PlayingDeckScript>().ArrangeTheCards();

                        if (NextPlayer.GetComponent<BotAI>() != null)
                        {
                            NextPlayer.GetComponent<BotAI>().PlayTurn(0);
                            turn = false;
                        }
                        else if (NextPlayer.GetComponent<PlayerBehaviour>() != null)
                        {
                            NextPlayer.GetComponent<PlayerBehaviour>().PassCounter = 0;
                            NextPlayer.GetComponent<PlayerBehaviour>().turn = true;
                            NextPlayer.GetComponent<PlayerBehaviour>().TurnOnPassButton();
                            turn = false;
                        }
                        c = Cards.Length;
                    }
                }
            }
        }

        Debug.Log(this.transform.name + " Royal Flush Checked ************* ");
        yield return null;
    }

    IEnumerator CheckFlush(int PassCounter)
    {
        Cards = new GameObject[this.transform.childCount];

        if (Cards.Length >= 5)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i] = this.transform.GetChild(i).gameObject;
            }

            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i].transform.parent = null;
            }


            for (int s = 1; s <= 4; s++)
            {
                for (int c = 0; c < Cards.Length; c++)
                {
                    if ((int)Cards[c].GetComponent<CardId>().CardIdentity == s)
                    {
                        Cards[c].transform.parent = this.transform;
                    }
                }
            }

            for (int c = 0; c < Cards.Length; c++)
            {
                if (Cards.Length - c >= 5)
                {
                    if ((int)Cards[c].GetComponent<CardId>().CardIdentity == (int)Cards[c + 1].GetComponent<CardId>().CardIdentity && (int)Cards[c + 1].GetComponent<CardId>().CardIdentity == (int)Cards[c + 2].GetComponent<CardId>().CardIdentity && (int)Cards[c + 2].GetComponent<CardId>().CardIdentity == (int)Cards[c + 3].GetComponent<CardId>().CardIdentity && (int)Cards[c + 3].GetComponent<CardId>().CardIdentity == (int)Cards[c + 4].GetComponent<CardId>().CardIdentity)
                    {
                        PlayingDeck.GetComponent<PlayingDeckScript>().CleanTheChild();

                        Cards[c].transform.parent = PlayingDeck.transform;
                        Cards[c].GetComponent<CardId>().MoveTheCards();
                        Cards[c + 1].transform.parent = PlayingDeck.transform;
                        Cards[c + 1].GetComponent<CardId>().MoveTheCards();
                        Cards[c + 2].transform.parent = PlayingDeck.transform;
                        Cards[c + 2].GetComponent<CardId>().MoveTheCards();
                        Cards[c + 3].transform.parent = PlayingDeck.transform;
                        Cards[c + 3].GetComponent<CardId>().MoveTheCards();
                        Cards[c + 4].transform.parent = PlayingDeck.transform;
                        Cards[c + 4].GetComponent<CardId>().MoveTheCards();

                        this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
                        CheckRemainingCards();
                        PlayingDeck.GetComponent<PlayingDeckScript>().FlushCard();
                        PlayingDeck.GetComponent<PlayingDeckScript>().ArrangeTheCards();

                        if (NextPlayer.GetComponent<BotAI>() != null)
                        {
                            NextPlayer.GetComponent<BotAI>().PlayTurn(0);
                            turn = false;
                        }
                        else if (NextPlayer.GetComponent<PlayerBehaviour>() != null)
                        {
                            NextPlayer.GetComponent<PlayerBehaviour>().PassCounter = 0;
                            NextPlayer.GetComponent<PlayerBehaviour>().turn = true;
                            NextPlayer.GetComponent<PlayerBehaviour>().TurnOnPassButton();
                            turn = false;
                        }
                        c = Cards.Length;
                    }
                }
            }
        }

        Debug.Log(this.transform.name + " Flush Checked ************* ");
        yield return null;
    }

    IEnumerator CheckStraight(int PassCounter)
    {
        Cards = new GameObject[this.transform.childCount];

        if (Cards.Length >= 5)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i] = this.transform.GetChild(i).gameObject;
            }

            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i].transform.parent = null;
            }


            for (int c = 0; c < Cards.Length; c++)
            {
                for (int s = 2; s <= 14; s++)
                {
                    if ((int)Cards[c].GetComponent<CardId>().CardNumber == s)
                    {
                        Cards[c].transform.parent = this.transform;
                    }
                }
            }

            for (int c = 0; c < Cards.Length; c++)
            {
                if (Cards.Length - c >= 5)
                {
                    if ((int)Cards[c].GetComponent<CardId>().CardNumber + 1 == (int)Cards[c + 1].GetComponent<CardId>().CardNumber && (int)Cards[c + 1].GetComponent<CardId>().CardNumber + 1 == (int)Cards[c + 2].GetComponent<CardId>().CardNumber && (int)Cards[c + 2].GetComponent<CardId>().CardNumber + 1 == (int)Cards[c + 3].GetComponent<CardId>().CardNumber && (int)Cards[c + 3].GetComponent<CardId>().CardNumber + 1 == (int)Cards[c + 4].GetComponent<CardId>().CardNumber)
                    {
                        PlayingDeck.GetComponent<PlayingDeckScript>().CleanTheChild();

                        Cards[c].transform.parent = PlayingDeck.transform;
                        Cards[c].GetComponent<CardId>().MoveTheCards();
                        Cards[c + 1].transform.parent = PlayingDeck.transform;
                        Cards[c + 1].GetComponent<CardId>().MoveTheCards();
                        Cards[c + 2].transform.parent = PlayingDeck.transform;
                        Cards[c + 2].GetComponent<CardId>().MoveTheCards();
                        Cards[c + 3].transform.parent = PlayingDeck.transform;
                        Cards[c + 3].GetComponent<CardId>().MoveTheCards();
                        Cards[c + 4].transform.parent = PlayingDeck.transform;
                        Cards[c + 4].GetComponent<CardId>().MoveTheCards();

                        this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
                        CheckRemainingCards();
                        PlayingDeck.GetComponent<PlayingDeckScript>().FlushCard();
                        PlayingDeck.GetComponent<PlayingDeckScript>().ArrangeTheCards();

                        if (NextPlayer.GetComponent<BotAI>() != null)
                        {
                            NextPlayer.GetComponent<BotAI>().PlayTurn(0);
                            turn = false;
                        }
                        else if (NextPlayer.GetComponent<PlayerBehaviour>() != null)
                        {
                            NextPlayer.GetComponent<PlayerBehaviour>().PassCounter = 0;
                            NextPlayer.GetComponent<PlayerBehaviour>().turn = true;
                            NextPlayer.GetComponent<PlayerBehaviour>().TurnOnPassButton();
                            turn = false;
                        }
                        c = Cards.Length;
                    }
                }
            }

            Debug.Log(this.transform.name + " Straight Checked ************* ");

            yield return null;
        }
    }

    IEnumerator CheckFourOfKind(int PassCounter)
    {
        Cards = new GameObject[this.transform.childCount];

        if (Cards.Length >= 5)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i] = this.transform.GetChild(i).gameObject;
            }

            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i].transform.parent = null;
            }

            for (int s = 2; s <= 14; s++)
            {
                for (int c = 0; c < Cards.Length; c++)
                {
                    if ((int)Cards[c].GetComponent<CardId>().CardNumber == s)
                    {
                        Cards[c].transform.parent = this.transform;
                    }
                }
            }

            for (int c = 0; c < Cards.Length; c++)
            {
                if (Cards.Length - c >= 5)
                {
                    if ((int)Cards[c].GetComponent<CardId>().CardNumber == (int)Cards[c + 1].GetComponent<CardId>().CardNumber && (int)Cards[c + 1].GetComponent<CardId>().CardNumber == (int)Cards[c + 2].GetComponent<CardId>().CardNumber && (int)Cards[c + 2].GetComponent<CardId>().CardNumber == (int)Cards[c + 3].GetComponent<CardId>().CardNumber)
                    {
                        PlayingDeck.GetComponent<PlayingDeckScript>().CleanTheChild();

                        Cards[c].transform.parent = PlayingDeck.transform;
                        Cards[c].GetComponent<CardId>().MoveTheCards();
                        Cards[c + 1].transform.parent = PlayingDeck.transform;
                        Cards[c + 1].GetComponent<CardId>().MoveTheCards();
                        Cards[c + 2].transform.parent = PlayingDeck.transform;
                        Cards[c + 2].GetComponent<CardId>().MoveTheCards();
                        Cards[c + 3].transform.parent = PlayingDeck.transform;
                        Cards[c + 3].GetComponent<CardId>().MoveTheCards();
                        Cards[c + 4].transform.parent = PlayingDeck.transform;
                        Cards[c + 4].GetComponent<CardId>().MoveTheCards();

                        this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
                        CheckRemainingCards();
                        PlayingDeck.GetComponent<PlayingDeckScript>().FourOfKindCard();
                        PlayingDeck.GetComponent<PlayingDeckScript>().ArrangeTheCards();

                        if (NextPlayer.GetComponent<BotAI>() != null)
                        {
                            NextPlayer.GetComponent<BotAI>().PlayTurn(0);
                            turn = false;
                        }
                        else if (NextPlayer.GetComponent<PlayerBehaviour>() != null)
                        {
                            NextPlayer.GetComponent<PlayerBehaviour>().PassCounter = 0;
                            NextPlayer.GetComponent<PlayerBehaviour>().turn = true;
                            NextPlayer.GetComponent<PlayerBehaviour>().TurnOnPassButton();
                            turn = false;
                        }
                        c = Cards.Length;
                    }
                }
            }

            Debug.Log(this.transform.name + " Four Of Kind Checked ************* ");

            yield return null;
        }
    }

    IEnumerator CheckThreeOfKind(int PassCounter)
    {
        Cards = new GameObject[this.transform.childCount];

        if (Cards.Length >= 3)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i] = this.transform.GetChild(i).gameObject;
            }

            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i].transform.parent = null;
            }

            for (int s = 2; s <= 14; s++)
            {
                for (int c = 0; c < Cards.Length; c++)
                {
                    if ((int)Cards[c].GetComponent<CardId>().CardNumber == s)
                    {
                        Cards[c].transform.parent = this.transform;
                    }
                }
            }

            for (int c = 0; c < Cards.Length; c++)
            {
                if (Cards.Length - c >= 3)
                {
                    if ((int)Cards[c].GetComponent<CardId>().CardNumber == (int)Cards[c + 1].GetComponent<CardId>().CardNumber && (int)Cards[c + 1].GetComponent<CardId>().CardNumber == (int)Cards[c + 2].GetComponent<CardId>().CardNumber)
                    {
                        PlayingDeck.GetComponent<PlayingDeckScript>().CleanTheChild();

                        Cards[c].transform.parent = PlayingDeck.transform;
                        Cards[c].GetComponent<CardId>().MoveTheCards();
                        Cards[c + 1].transform.parent = PlayingDeck.transform;
                        Cards[c + 1].GetComponent<CardId>().MoveTheCards();
                        Cards[c + 2].transform.parent = PlayingDeck.transform;
                        Cards[c + 2].GetComponent<CardId>().MoveTheCards();

                        this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
                        CheckRemainingCards();
                        PlayingDeck.GetComponent<PlayingDeckScript>().ThreeOfKindCard();
                        PlayingDeck.GetComponent<PlayingDeckScript>().ArrangeTheCards();

                        if (NextPlayer.GetComponent<BotAI>() != null)
                        {
                            NextPlayer.GetComponent<BotAI>().PlayTurn(0);
                            turn = false;
                        }
                        else if (NextPlayer.GetComponent<PlayerBehaviour>() != null)
                        {
                            NextPlayer.GetComponent<PlayerBehaviour>().PassCounter = 0;
                            NextPlayer.GetComponent<PlayerBehaviour>().turn = true;
                            NextPlayer.GetComponent<PlayerBehaviour>().TurnOnPassButton();
                            turn = false;
                        }
                        c = Cards.Length;
                    }
                }
            }

            Debug.Log(this.transform.name + " Three Of Kind Checked ************* ");

            yield return null;
        }
    }

    IEnumerator CheckPair(int PassCounter)
    {
        Cards = new GameObject[this.transform.childCount];

        if (Cards.Length >= 2)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i] = this.transform.GetChild(i).gameObject;
            }

            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i].transform.parent = null;
            }

            for (int s = 2; s <= 14; s++)
            {
                for (int c = 0; c < Cards.Length; c++)
                {
                    if ((int)Cards[c].GetComponent<CardId>().CardNumber == s)
                    {
                        Cards[c].transform.parent = this.transform;
                    }
                }
            }

            for (int c = 0; c < Cards.Length; c++)
            {
                if (Cards.Length - c >= 2)
                {
                    if ((int)Cards[c].GetComponent<CardId>().CardNumber == (int)Cards[c + 1].GetComponent<CardId>().CardNumber)
                    {
                        PlayingDeck.GetComponent<PlayingDeckScript>().CleanTheChild();

                        Cards[c].transform.parent = PlayingDeck.transform;
                        Cards[c].GetComponent<CardId>().MoveTheCards();
                        Cards[c + 1].transform.parent = PlayingDeck.transform;
                        Cards[c + 1].GetComponent<CardId>().MoveTheCards();

                        this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
                        CheckRemainingCards();
                        PlayingDeck.GetComponent<PlayingDeckScript>().PairCard();
                        PlayingDeck.GetComponent<PlayingDeckScript>().ArrangeTheCards();

                        if (NextPlayer.GetComponent<BotAI>() != null)
                        {
                            NextPlayer.GetComponent<BotAI>().PlayTurn(0);
                            turn = false;
                        }
                        else if (NextPlayer.GetComponent<PlayerBehaviour>() != null)
                        {
                            NextPlayer.GetComponent<PlayerBehaviour>().PassCounter = 0;
                            NextPlayer.GetComponent<PlayerBehaviour>().turn = true;
                            NextPlayer.GetComponent<PlayerBehaviour>().TurnOnPassButton();
                            turn = false;
                        }
                        c = Cards.Length;
                    }
                }
            }

            Debug.Log(this.transform.name + " Pair Checked ************* ");

            yield return null;
        }
    }

    IEnumerator SingleCard(int PassCounter)
    {
        Cards = new GameObject[this.transform.childCount];

        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i] = this.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i].transform.parent = null;
        }

        for (int a = 3; a <= 15; a++)
        {
            for (int i = 1; i <= 4; i++)
            {
                for (int s = 0; s < Cards.Length; s++)
                {
                    if ((int)Cards[s].GetComponent<CardId>().CardIdentity == i && (int)Cards[s].GetComponent<CardId>().SingleCardNumber == a)
                    {
                        Cards[s].transform.parent = this.transform;
                    }
                }
            }
        }

        for (int i = 0; i < Cards.Length; i++)
        {
            PlayingDeck.GetComponent<PlayingDeckScript>().CleanTheChild();
            Cards[i].transform.parent = PlayingDeck.transform;
            Cards[i].GetComponent<CardId>().MoveTheCards();
            this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
            CheckRemainingCards();
            PlayingDeck.GetComponent<PlayingDeckScript>().SingleCard();
            PlayingDeck.GetComponent<PlayingDeckScript>().ArrangeTheCards();
            if (NextPlayer.GetComponent<BotAI>() != null)
            {
                NextPlayer.GetComponent<BotAI>().PlayTurn(0);
                turn = false;
            }
            else if (NextPlayer.GetComponent<PlayerBehaviour>() != null)
            {
                NextPlayer.GetComponent<PlayerBehaviour>().PassCounter = 0;
                NextPlayer.GetComponent<PlayerBehaviour>().turn = true;
                NextPlayer.GetComponent<PlayerBehaviour>().TurnOnPassButton();
                turn = false;
            }
            i = Cards.Length;
        }

        Debug.Log(this.transform.name + " Single Card Checked ************* ");

        yield return null;
    }
}
