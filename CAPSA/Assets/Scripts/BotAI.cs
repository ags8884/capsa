using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BotAI : MonoBehaviour
{
    [Header("MANUAL INPUT")]
    public GameObject Text;
    public GameObject NextPlayer;
    public CharactersBehaviour CB;


    [Header("NON/AUTO INPUT")]
    public bool turn;
    public PlayingDeckScript PlayingDeck;
    public CardId[] Cards;
    public CardId PlayingDeckCard;
    public GameObject[] CardsToBeSetToDeck;
    private BotAI Bot;
    private PlayerBehaviour Player;
    public ArrangeCardOnDeck ArrangeCardOnDeck;

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
        if (NextPlayer.GetComponent<BotAI>() != null)
        {
            Bot = NextPlayer.GetComponent<BotAI>();
        }
        
        if (NextPlayer.GetComponent<PlayerBehaviour>() != null)
        {
            Player = NextPlayer.GetComponent<PlayerBehaviour>();
        }
        PlayingDeck = PlayingDeckScript.main;
        ArrangeCardOnDeck = this.GetComponent<ArrangeCardOnDeck>();
        Text.SetActive(false);
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
                ArrangeCardOnDeck.ArrangeCard();
                PlayingDeck.SingleCard();
                PlayingDeck.ArrangeTheCards();
                if (Bot != null)
                {
                    Bot.PlayTurn(0);
                    turn = false;
                }
                else if (Player != null)
                {
                    Player.PassCounter = 0;
                    Player.turn = true;
                    Player.TurnOnPassButton();
                    turn = false;
                }

                UpdateText("FIRST MOVE");
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

        StartCoroutine(WaitUpdateText());
    }

    IEnumerator WaitUpdateText()
    {
        yield return new WaitForSeconds(0.3f);
        UpdateText(transform.name + " Turn");
    }

    public void CheckRemainingCards()
    {
        StartCoroutine(CheckRemainingCardsEnum());
    }

    IEnumerator CheckRemainingCardsEnum()
    {
        yield return new WaitForSeconds(0.3f);
        Cards = new CardId[this.transform.childCount];
        if (Cards.Length <= 0)
        {
            UpdateText(this.transform.name + " WIN!!");
            CB.Win();
            SFXManager.main.Losing();
            Time.timeScale = 0;
        }
    }

    IEnumerator WaitPlayTurn(int PassCounter)
    {
        yield return new WaitForSeconds(1);

        PlayingDeckCard = PlayingDeck.transform.GetChild(0).GetComponent<CardId>();

        turn = true;

        if (PlayingDeck.Single)
        {

            Cards = new CardId[this.transform.childCount];

            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i] = this.transform.GetChild(i).GetComponent<CardId>();
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
                        if ((int)Cards[s].CardIdentity == i && (int)Cards[s].SingleCardNumber == a)
                        {
                            Cards[s].transform.parent = this.transform;
                        }
                    }
                }
            }

            for (int i = 0; i < Cards.Length; i++)
            {
                if ((int)Cards[i].SingleCardNumber == (int)PlayingDeckCard.SingleCardNumber && (int)Cards[i].CardIdentity > (int)PlayingDeckCard.CardIdentity)
                {
                    PlayingDeck.CleanTheChild();
                    Cards[i].transform.parent = PlayingDeck.transform;
                    Cards[i].MoveTheCards();
                    ArrangeCardOnDeck.ArrangeCard();
                    PlayingDeck.SingleCard();
                    PlayingDeck.ArrangeTheCards();
                    CheckRemainingCards();
                    if (Bot != null)
                    {
                        Bot.PlayTurn(0);
                        turn = false;
                    }
                    else if (Player != null)
                    {
                        Player.PassCounter = 0;
                        Player.turn = true;
                        Player.TurnOnPassButton();
                        turn = false;
                    }
                    i = Cards.Length;
                }
                else if ((int)Cards[i].SingleCardNumber > (int)PlayingDeckCard.SingleCardNumber)
                {
                    PlayingDeck.CleanTheChild();
                    Cards[i].transform.parent = PlayingDeck.transform;
                    Cards[i].MoveTheCards();
                    ArrangeCardOnDeck.ArrangeCard();
                    PlayingDeck.SingleCard();
                    PlayingDeck.ArrangeTheCards();
                    CheckRemainingCards();
                    if (Bot != null)
                    {
                        Bot.PlayTurn(0);
                        turn = false;
                    }
                    else if (Player != null)
                    {
                        Player.PassCounter = 0;
                        Player.turn = true;
                        Player.TurnOnPassButton();
                        turn = false;
                    }
                    i = Cards.Length;
                }
                else if (i == Cards.Length - 1)
                {
                    Debug.Log(this.transform.name + " = Pass");
                    UpdateText("Pass ( " + (PassCounter + 1).ToString() + " )");
                    CB.Sad();
                    ArrangeCardOnDeck.ArrangeCard();
                    SFXManager.main.Passing();
                    PlayingDeck.SingleCard();
                    PlayingDeck.ArrangeTheCards();
                    if (Bot != null)
                    {
                        Bot.PlayTurn(PassCounter + 1);
                        turn = false;
                    }
                    else if (Player != null)
                    {
                        Player.PassCounter = PassCounter + 1;
                        Player.turn = true;
                        Player.TurnOnPassButton();
                        turn = false;
                    }
                    i = Cards.Length;

                    if (PassCounter + 1 >= 3)
                    {
                        PlayingDeck.CleanTheChild();
                    }
                }
            }
        }

        if (PlayingDeck.FullHouse)
        {
            Cards = new CardId[this.transform.childCount];

            if (Cards.Length >= 5)
            {
                for (int i = 0; i < Cards.Length; i++)
                {
                    Cards[i] = this.transform.GetChild(i).GetComponent<CardId>();
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
                            if ((int)Cards[c].CardNumber == i && (int)Cards[c].CardIdentity == s)
                            {
                                Cards[c].transform.parent = this.transform;
                            }
                        }
                    }
                }

                for (int c = 0; c < Cards.Length; c++)
                {
                    if ((int)Cards[c].CardNumber > (int)PlayingDeckCard.CardNumber)
                    {
                        if (Cards.Length - c >= 5)
                        {
                            if ((int)Cards[c].CardNumber == (int)Cards[c + 1].CardNumber && (int)Cards[c + 1].CardNumber == (int)Cards[c + 2].CardNumber && (int)Cards[c + 3].CardNumber == (int)Cards[c + 4].CardNumber)
                            {
                                PlayingDeck.CleanTheChild();

                                Cards[c].transform.parent = PlayingDeck.transform;
                                Cards[c].MoveTheCards();
                                Cards[c + 1].transform.parent = PlayingDeck.transform;
                                Cards[c + 1].MoveTheCards();
                                Cards[c + 2].transform.parent = PlayingDeck.transform;
                                Cards[c + 2].MoveTheCards();
                                Cards[c + 3].transform.parent = PlayingDeck.transform;
                                Cards[c + 3].MoveTheCards();
                                Cards[c + 4].transform.parent = PlayingDeck.transform;
                                Cards[c + 4].MoveTheCards();

                                ArrangeCardOnDeck.ArrangeCard();
                                CheckRemainingCards();
                                PlayingDeck.FullHouseCard();
                                PlayingDeck.ArrangeTheCards();

                                if (Bot != null)
                                {
                                    Bot.PlayTurn(0);
                                    turn = false;
                                }
                                else if (Player != null)
                                {
                                    Player.PassCounter = 0;
                                    Player.turn = true;
                                    Player.TurnOnPassButton();
                                    turn = false;
                                }
                                c = Cards.Length;
                            }
                            else if ((int)Cards[c].CardNumber == (int)Cards[c + 1].CardNumber && (int)Cards[c + 2].CardNumber == (int)Cards[c + 3].CardNumber && (int)Cards[c + 3].CardNumber == (int)Cards[c + 4].CardNumber)
                            {
                                PlayingDeck.CleanTheChild();

                                Cards[c].transform.parent = PlayingDeck.transform;
                                Cards[c].MoveTheCards();
                                Cards[c + 1].transform.parent = PlayingDeck.transform;
                                Cards[c + 1].MoveTheCards();
                                Cards[c + 2].transform.parent = PlayingDeck.transform;
                                Cards[c + 2].MoveTheCards();
                                Cards[c + 3].transform.parent = PlayingDeck.transform;
                                Cards[c + 3].MoveTheCards();
                                Cards[c + 4].transform.parent = PlayingDeck.transform;
                                Cards[c + 4].MoveTheCards();

                                ArrangeCardOnDeck.ArrangeCard();
                                CheckRemainingCards();
                                PlayingDeck.FullHouseCard();
                                PlayingDeck.ArrangeTheCards();

                                if (Bot != null)
                                {
                                    Bot.PlayTurn(0);
                                    turn = false;
                                }
                                else if (Player != null)
                                {
                                    Player.PassCounter = 0;
                                    Player.turn = true;
                                    Player.TurnOnPassButton();
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
                SFXManager.main.Passing();
                UpdateText("Pass ( " + (PassCounter + 1).ToString() + " )");
                CB.Sad();
                ArrangeCardOnDeck.ArrangeCard();
                PlayingDeck.FullHouseCard();
                PlayingDeck.ArrangeTheCards();
                if (Bot != null)
                {
                    Bot.PlayTurn(PassCounter + 1);
                    turn = false;
                }
                else if (Player != null)
                {
                    Player.PassCounter = PassCounter + 1;
                    Player.turn = true;
                    Player.TurnOnPassButton();
                    turn = false;
                }

                if (PassCounter + 1 >= 3)
                {
                    PlayingDeck.CleanTheChild();
                }
            }
        }

        if (PlayingDeck.RoyalFlush)
        {

            Cards = new CardId[this.transform.childCount];

            if (Cards.Length >= 5)
            {
                for (int i = 0; i < Cards.Length; i++)
                {
                    Cards[i] = this.transform.GetChild(i).GetComponent<CardId>();
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
                            if ((int)Cards[c].CardNumber == i && (int)Cards[c].CardIdentity == s)
                            {
                                Cards[c].transform.parent = this.transform;
                            }
                        }
                    }
                }

                for (int c = 0; c < Cards.Length; c++)
                {
                    if ((int)Cards[c].CardNumber > (int)PlayingDeckCard.CardNumber)
                    {
                        if (Cards.Length - c >= 5)
                        {
                            if ((int)Cards[c].CardIdentity == (int)Cards[c + 1].CardIdentity && (int)Cards[c + 1].CardIdentity == (int)Cards[c + 2].CardIdentity && (int)Cards[c + 2].CardIdentity == (int)Cards[c + 3].CardIdentity && (int)Cards[c + 3].CardIdentity == (int)Cards[c + 4].CardIdentity)
                            {
                                if ((int)Cards[c].CardNumber + 1 == (int)Cards[c + 1].CardNumber && (int)Cards[c + 1].CardNumber + 1 == (int)Cards[c + 2].CardNumber && (int)Cards[c + 2].CardNumber + 1 == (int)Cards[c + 3].CardNumber && (int)Cards[c + 3].CardNumber + 1 == (int)Cards[c + 4].CardNumber)
                                {
                                    PlayingDeck.CleanTheChild();

                                    Cards[c].transform.parent = PlayingDeck.transform;
                                    Cards[c].MoveTheCards();
                                    Cards[c + 1].transform.parent = PlayingDeck.transform;
                                    Cards[c + 1].MoveTheCards();
                                    Cards[c + 2].transform.parent = PlayingDeck.transform;
                                    Cards[c + 2].MoveTheCards();
                                    Cards[c + 3].transform.parent = PlayingDeck.transform;
                                    Cards[c + 3].MoveTheCards();
                                    Cards[c + 4].transform.parent = PlayingDeck.transform;
                                    Cards[c + 4].MoveTheCards();

                                    ArrangeCardOnDeck.ArrangeCard();
                                    CheckRemainingCards();
                                    PlayingDeck.RoyalFlushCard();
                                    PlayingDeck.ArrangeTheCards();

                                    if (Bot != null)
                                    {
                                        Bot.PlayTurn(0);
                                        turn = false;
                                    }
                                    else if (Player != null)
                                    {
                                        Player.PassCounter = 0;
                                        Player.turn = true;
                                        Player.TurnOnPassButton();
                                        turn = false;
                                    }
                                    c = Cards.Length;
                                }
                            }
                        }
                    }
                }
            }

            if (turn)
            {
                Debug.Log(this.transform.name + " = Pass");
                SFXManager.main.Passing();
                UpdateText("Pass ( " + (PassCounter + 1).ToString() + " )");
                CB.Sad();
                ArrangeCardOnDeck.ArrangeCard();
                PlayingDeck.RoyalFlushCard();
                PlayingDeck.ArrangeTheCards();
                if (Bot != null)
                {
                    Bot.PlayTurn(PassCounter + 1);
                    turn = false;
                }
                else if (Player != null)
                {
                    Player.PassCounter = PassCounter + 1;
                    Player.turn = true;
                    Player.TurnOnPassButton();
                    turn = false;
                }

                if (PassCounter + 1 >= 3)
                {
                    PlayingDeck.CleanTheChild();
                }
            }
        }

        if (PlayingDeck.Flush)
        {

            Cards = new CardId[this.transform.childCount];

            if (Cards.Length >= 5)
            {
                for (int i = 0; i < Cards.Length; i++)
                {
                    Cards[i] = this.transform.GetChild(i).GetComponent<CardId>();
                }

                for (int i = 0; i < Cards.Length; i++)
                {
                    Cards[i].transform.parent = null;
                }


                for (int s = 1; s <= 4; s++)
                {
                    for (int c = 0; c < Cards.Length; c++)
                    {
                        if ((int)Cards[c].CardIdentity == s)
                        {
                            Cards[c].transform.parent = this.transform;
                        }
                    }
                }

                for (int c = 0; c < Cards.Length; c++)
                {
                    if ((int)Cards[c].CardNumber > (int)PlayingDeckCard.CardNumber)
                    {
                        if (Cards.Length - c >= 5)
                        {
                            if ((int)Cards[c].CardIdentity == (int)Cards[c + 1].CardIdentity && (int)Cards[c + 1].CardIdentity == (int)Cards[c + 2].CardIdentity && (int)Cards[c + 2].CardIdentity == (int)Cards[c + 3].CardIdentity && (int)Cards[c + 3].CardIdentity == (int)Cards[c + 4].CardIdentity)
                            {
                                PlayingDeck.CleanTheChild();

                                Cards[c].transform.parent = PlayingDeck.transform;
                                Cards[c].MoveTheCards();
                                Cards[c + 1].transform.parent = PlayingDeck.transform;
                                Cards[c + 1].MoveTheCards();
                                Cards[c + 2].transform.parent = PlayingDeck.transform;
                                Cards[c + 2].MoveTheCards();
                                Cards[c + 3].transform.parent = PlayingDeck.transform;
                                Cards[c + 3].MoveTheCards();
                                Cards[c + 4].transform.parent = PlayingDeck.transform;
                                Cards[c + 4].MoveTheCards();

                                ArrangeCardOnDeck.ArrangeCard();
                                CheckRemainingCards();
                                PlayingDeck.FlushCard();
                                PlayingDeck.ArrangeTheCards();

                                if (Bot != null)
                                {
                                    Bot.PlayTurn(0);
                                    turn = false;
                                }
                                else if (Player != null)
                                {
                                    Player.PassCounter = 0;
                                    Player.turn = true;
                                    Player.TurnOnPassButton();
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
                SFXManager.main.Passing();
                UpdateText("Pass ( " + (PassCounter + 1).ToString() + " )");
                CB.Sad();
                ArrangeCardOnDeck.ArrangeCard();
                PlayingDeck.FlushCard();
                PlayingDeck.ArrangeTheCards();
                if (Bot != null)
                {
                    Bot.PlayTurn(PassCounter + 1);
                    turn = false;
                }
                else if (Player != null)
                {
                    Player.PassCounter = PassCounter + 1;
                    Player.turn = true;
                    Player.TurnOnPassButton();
                    turn = false;
                }

                if (PassCounter + 1 >= 3)
                {
                    PlayingDeck.CleanTheChild();
                }
            }
        }


        if (PlayingDeck.Straight)
        {
            Cards = new CardId[this.transform.childCount];

            if (Cards.Length >= 5)
            {
                for (int i = 0; i < Cards.Length; i++)
                {
                    Cards[i] = this.transform.GetChild(i).GetComponent<CardId>();
                }

                for (int i = 0; i < Cards.Length; i++)
                {
                    Cards[i].transform.parent = null;
                }


                for (int c = 0; c < Cards.Length; c++)
                {
                    for (int s = 2; s <= 14; s++)
                    {
                        if ((int)Cards[c].CardNumber == s)
                        {
                            Cards[c].transform.parent = this.transform;
                        }
                    }
                }

                for (int c = 0; c < Cards.Length; c++)
                {
                    if ((int)Cards[c].CardNumber > (int)PlayingDeckCard.CardNumber)
                    {
                        if (Cards.Length - c >= 5)
                        {
                            if ((int)Cards[c].CardNumber + 1 == (int)Cards[c + 1].CardNumber && (int)Cards[c + 1].CardNumber + 1 == (int)Cards[c + 2].CardNumber && (int)Cards[c + 2].CardNumber + 1 == (int)Cards[c + 3].CardNumber && (int)Cards[c + 3].CardNumber + 1 == (int)Cards[c + 4].CardNumber)
                            {
                                PlayingDeck.CleanTheChild();

                                Cards[c].transform.parent = PlayingDeck.transform;
                                Cards[c].MoveTheCards();
                                Cards[c + 1].transform.parent = PlayingDeck.transform;
                                Cards[c + 1].MoveTheCards();
                                Cards[c + 2].transform.parent = PlayingDeck.transform;
                                Cards[c + 2].MoveTheCards();
                                Cards[c + 3].transform.parent = PlayingDeck.transform;
                                Cards[c + 3].MoveTheCards();
                                Cards[c + 4].transform.parent = PlayingDeck.transform;
                                Cards[c + 4].MoveTheCards();

                                ArrangeCardOnDeck.ArrangeCard();
                                CheckRemainingCards();
                                PlayingDeck.StraightCard();
                                PlayingDeck.ArrangeTheCards();

                                if (Bot != null)
                                {
                                    Bot.PlayTurn(0);
                                    turn = false;
                                }
                                else if (Player != null)
                                {
                                    Player.PassCounter = 0;
                                    Player.turn = true;
                                    Player.TurnOnPassButton();
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
                SFXManager.main.Passing();
                UpdateText("Pass ( " + (PassCounter + 1).ToString() + " )");
                CB.Sad();
                ArrangeCardOnDeck.ArrangeCard();
                PlayingDeck.StraightCard();
                PlayingDeck.ArrangeTheCards();
                if (Bot != null)
                {
                    Bot.PlayTurn(PassCounter + 1);
                    turn = false;
                }
                else if (Player != null)
                {
                    Player.PassCounter = PassCounter + 1;
                    Player.turn = true;
                    Player.TurnOnPassButton();
                    turn = false;
                }

                if (PassCounter + 1 >= 3)
                {
                    PlayingDeck.CleanTheChild();
                }
            }
        }


        if (PlayingDeck.FourOfKind)
        {
            Cards = new CardId[this.transform.childCount];

            if (Cards.Length >= 5)
            {
                for (int i = 0; i < Cards.Length; i++)
                {
                    Cards[i] = this.transform.GetChild(i).GetComponent<CardId>();
                }

                for (int i = 0; i < Cards.Length; i++)
                {
                    Cards[i].transform.parent = null;
                }

                for (int s = 2; s <= 14; s++)
                {
                    for (int c = 0; c < Cards.Length; c++)
                    {
                        if ((int)Cards[c].CardNumber == s)
                        {
                            Cards[c].transform.parent = this.transform;
                        }
                    }
                }

                for (int c = 0; c < Cards.Length; c++)
                {
                    if ((int)Cards[c].CardNumber > (int)PlayingDeckCard.CardNumber)
                    {
                        if (Cards.Length - c >= 5)
                        {
                            if ((int)Cards[c].CardNumber == (int)Cards[c + 1].CardNumber && (int)Cards[c + 1].CardNumber == (int)Cards[c + 2].CardNumber && (int)Cards[c + 2].CardNumber == (int)Cards[c + 3].CardNumber)
                            {
                                PlayingDeck.CleanTheChild();

                                Cards[c].transform.parent = PlayingDeck.transform;
                                Cards[c].MoveTheCards();
                                Cards[c + 1].transform.parent = PlayingDeck.transform;
                                Cards[c + 1].MoveTheCards();
                                Cards[c + 2].transform.parent = PlayingDeck.transform;
                                Cards[c + 2].MoveTheCards();
                                Cards[c + 3].transform.parent = PlayingDeck.transform;
                                Cards[c + 3].MoveTheCards();
                                Cards[c + 4].transform.parent = PlayingDeck.transform;
                                Cards[c + 4].MoveTheCards();

                                ArrangeCardOnDeck.ArrangeCard();
                                CheckRemainingCards();
                                PlayingDeck.FourOfKindCard();
                                PlayingDeck.ArrangeTheCards();

                                if (Bot != null)
                                {
                                    Bot.PlayTurn(0);
                                    turn = false;
                                }
                                else if (Player != null)
                                {
                                    Player.PassCounter = 0;
                                    Player.turn = true;
                                    Player.TurnOnPassButton();
                                    turn = false;
                                }
                                c = Cards.Length;
                                Debug.Log(this.transform.name + " FourOfKindTester");
                            }
                            else if ((int)Cards[c + 1].CardNumber == (int)Cards[c + 2].CardNumber && (int)Cards[c + 2].CardNumber == (int)Cards[c + 3].CardNumber && (int)Cards[c + 3].CardNumber == (int)Cards[c + 4].CardNumber)
                            {
                                PlayingDeck.CleanTheChild();

                                Cards[c].transform.parent = PlayingDeck.transform;
                                Cards[c].MoveTheCards();
                                Cards[c + 1].transform.parent = PlayingDeck.transform;
                                Cards[c + 1].MoveTheCards();
                                Cards[c + 2].transform.parent = PlayingDeck.transform;
                                Cards[c + 2].MoveTheCards();
                                Cards[c + 3].transform.parent = PlayingDeck.transform;
                                Cards[c + 3].MoveTheCards();
                                Cards[c + 4].transform.parent = PlayingDeck.transform;
                                Cards[c + 4].MoveTheCards();

                                ArrangeCardOnDeck.ArrangeCard();
                                CheckRemainingCards();
                                PlayingDeck.FourOfKindCard();
                                PlayingDeck.ArrangeTheCards();

                                if (Bot != null)
                                {
                                    Bot.PlayTurn(0);
                                    turn = false;
                                }
                                else if (Player != null)
                                {
                                    Player.PassCounter = 0;
                                    Player.turn = true;
                                    Player.TurnOnPassButton();
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
                SFXManager.main.Passing();
                UpdateText("Pass ( " + (PassCounter + 1).ToString() + " )");
                CB.Sad();
                ArrangeCardOnDeck.ArrangeCard();
                PlayingDeck.FourOfKindCard();
                PlayingDeck.ArrangeTheCards();
                if (Bot != null)
                {
                    Bot.PlayTurn(PassCounter + 1);
                    turn = false;
                }
                else if (Player != null)
                {
                    Player.PassCounter = PassCounter + 1;
                    Player.turn = true;
                    Player.TurnOnPassButton();
                    turn = false;
                }

                if (PassCounter + 1 >= 3)
                {
                    PlayingDeck.CleanTheChild();
                }
            }
        }

        if (PlayingDeck.ThreeOfKind)
        {
            Cards = new CardId[this.transform.childCount];

            if (Cards.Length >= 3)
            {
                for (int i = 0; i < Cards.Length; i++)
                {
                    Cards[i] = this.transform.GetChild(i).GetComponent<CardId>();
                }

                for (int i = 0; i < Cards.Length; i++)
                {
                    Cards[i].transform.parent = null;
                }

                for (int s = 2; s <= 14; s++)
                {
                    for (int c = 0; c < Cards.Length; c++)
                    {
                        if ((int)Cards[c].CardNumber == s)
                        {
                            Cards[c].transform.parent = this.transform;
                        }
                    }
                }

                for (int c = 0; c < Cards.Length; c++)
                {
                    if ((int)Cards[c].CardNumber > (int)PlayingDeckCard.CardNumber)
                    {
                        if (Cards.Length - c >= 3)
                        {
                            if ((int)Cards[c].CardNumber == (int)Cards[c + 1].CardNumber && (int)Cards[c + 1].CardNumber == (int)Cards[c + 2].CardNumber)
                            {
                                PlayingDeck.CleanTheChild();

                                Cards[c].transform.parent = PlayingDeck.transform;
                                Cards[c].MoveTheCards();
                                Cards[c + 1].transform.parent = PlayingDeck.transform;
                                Cards[c + 1].MoveTheCards();
                                Cards[c + 2].transform.parent = PlayingDeck.transform;
                                Cards[c + 2].MoveTheCards();

                                ArrangeCardOnDeck.ArrangeCard();
                                CheckRemainingCards();
                                PlayingDeck.ThreeOfKindCard();
                                PlayingDeck.ArrangeTheCards();

                                if (Bot != null)
                                {
                                    Bot.PlayTurn(0);
                                    turn = false;
                                }
                                else if (Player != null)
                                {
                                    Player.PassCounter = 0;
                                    Player.turn = true;
                                    Player.TurnOnPassButton();
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
                SFXManager.main.Passing();
                UpdateText("Pass ( " + (PassCounter + 1).ToString() + " )");
                CB.Sad();
                ArrangeCardOnDeck.ArrangeCard();
                PlayingDeck.ThreeOfKindCard();
                PlayingDeck.ArrangeTheCards();
                if (Bot != null)
                {
                    Bot.PlayTurn(PassCounter + 1);
                    turn = false;
                }
                else if (Player != null)
                {
                    Player.PassCounter = PassCounter + 1;
                    Player.turn = true;
                    Player.TurnOnPassButton();
                    turn = false;
                }

                if (PassCounter + 1 >= 3)
                {
                    PlayingDeck.CleanTheChild();
                }
            }
        }

        if (PlayingDeck.Pair)
        {

            Cards = new CardId[this.transform.childCount];

            if (Cards.Length >= 2)
            {
                for (int i = 0; i < Cards.Length; i++)
                {
                    Cards[i] = this.transform.GetChild(i).GetComponent<CardId>();
                }

                for (int i = 0; i < Cards.Length; i++)
                {
                    Cards[i].transform.parent = null;
                }

                for (int s = 2; s <= 14; s++)
                {
                    for (int c = 0; c < Cards.Length; c++)
                    {
                        if ((int)Cards[c].CardNumber == s)
                        {
                            Cards[c].transform.parent = this.transform;
                        }
                    }
                }

                for (int c = 0; c < Cards.Length; c++)
                {
                    if ((int)Cards[c].CardNumber > (int)PlayingDeckCard.CardNumber)
                    {
                        if (Cards.Length - c >= 2)
                        {
                            if ((int)Cards[c].CardNumber == (int)Cards[c + 1].CardNumber)
                            {
                                PlayingDeck.CleanTheChild();

                                Cards[c].transform.parent = PlayingDeck.transform;
                                Cards[c].MoveTheCards();
                                Cards[c + 1].transform.parent = PlayingDeck.transform;
                                Cards[c + 1].MoveTheCards();

                                ArrangeCardOnDeck.ArrangeCard();
                                CheckRemainingCards();
                                PlayingDeck.PairCard();
                                PlayingDeck.ArrangeTheCards();

                                if (Bot != null)
                                {
                                    Bot.PlayTurn(0);
                                    turn = false;
                                }
                                else if (Player != null)
                                {
                                    Player.PassCounter = 0;
                                    Player.turn = true;
                                    Player.TurnOnPassButton();
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
                SFXManager.main.Passing();
                UpdateText("Pass ( " + (PassCounter + 1).ToString() + " )");
                CB.Sad();
                ArrangeCardOnDeck.ArrangeCard();
                PlayingDeck.PairCard();
                PlayingDeck.ArrangeTheCards();
                if (Bot != null)
                {
                    Bot.PlayTurn(PassCounter + 1);
                    turn = false;
                }
                else if (Player != null)
                {
                    Player.PassCounter = PassCounter + 1;
                    Player.turn = true;
                    Player.TurnOnPassButton();
                    turn = false;
                }

                if (PassCounter + 1 >= 3)
                {
                    PlayingDeck.CleanTheChild();
                }
            }
        }
    }

    IEnumerator FirstPlayTurn(int PassCounter)
    {
        turn = true;
        yield return new WaitForSeconds(1f);
        if (turn)
        {
            StartCoroutine(CheckFullHouse(PassCounter));
        }
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

    IEnumerator CheckFullHouse(int PassCounter)
    {
        Cards = new CardId[this.transform.childCount];

        if (Cards.Length >= 5)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i] = this.transform.GetChild(i).GetComponent<CardId>();
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
                        if ((int)Cards[c].CardNumber == i && (int)Cards[c].CardIdentity == s)
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
                    if ((int)Cards[c].CardNumber == (int)Cards[c + 1].CardNumber && (int)Cards[c + 1].CardNumber == (int)Cards[c + 2].CardNumber && (int)Cards[c + 3].CardNumber == (int)Cards[c + 4].CardNumber)
                    {
                        PlayingDeck.CleanTheChild();

                        Cards[c].transform.parent = PlayingDeck.transform;
                        Cards[c].MoveTheCards();
                        Cards[c + 1].transform.parent = PlayingDeck.transform;
                        Cards[c + 1].MoveTheCards();
                        Cards[c + 2].transform.parent = PlayingDeck.transform;
                        Cards[c + 2].MoveTheCards();
                        Cards[c + 3].transform.parent = PlayingDeck.transform;
                        Cards[c + 3].MoveTheCards();
                        Cards[c + 4].transform.parent = PlayingDeck.transform;
                        Cards[c + 4].MoveTheCards();

                        ArrangeCardOnDeck.ArrangeCard();
                        CheckRemainingCards();
                        PlayingDeck.FullHouseCard();
                        PlayingDeck.UpdateText("Full House");
                        PlayingDeck.ArrangeTheCards();

                        if (Bot != null)
                        {
                            Bot.PlayTurn(0);
                            turn = false;
                        }
                        else if (Player != null)
                        {
                            Player.PassCounter = 0;
                            Player.turn = true;
                            Player.TurnOnPassButton();
                            turn = false;
                        }
                        c = Cards.Length;
                    }
                    else if ((int)Cards[c].CardNumber == (int)Cards[c + 1].CardNumber && (int)Cards[c + 2].CardNumber == (int)Cards[c + 3].CardNumber && (int)Cards[c + 3].CardNumber == (int)Cards[c + 4].CardNumber)
                    {
                        PlayingDeck.CleanTheChild();

                        Cards[c].transform.parent = PlayingDeck.transform;
                        Cards[c].MoveTheCards();
                        Cards[c + 1].transform.parent = PlayingDeck.transform;
                        Cards[c + 1].MoveTheCards();
                        Cards[c + 2].transform.parent = PlayingDeck.transform;
                        Cards[c + 2].MoveTheCards();
                        Cards[c + 3].transform.parent = PlayingDeck.transform;
                        Cards[c + 3].MoveTheCards();
                        Cards[c + 4].transform.parent = PlayingDeck.transform;
                        Cards[c + 4].MoveTheCards();

                        ArrangeCardOnDeck.ArrangeCard();
                        CheckRemainingCards();
                        PlayingDeck.FullHouseCard();
                        PlayingDeck.UpdateText("Full House");
                        PlayingDeck.ArrangeTheCards();

                        if (Bot != null)
                        {
                            Bot.PlayTurn(0);
                            turn = false;
                        }
                        else if (Player != null)
                        {
                            Player.PassCounter = 0;
                            Player.turn = true;
                            Player.TurnOnPassButton();
                            turn = false;
                        }
                        c = Cards.Length;
                    }
                }
            }
        }

        Debug.Log(this.transform.name + " Full House Checked ************* ");
        yield return null;
    }

    IEnumerator CheckRoyalFlush(int PassCounter)
    {
        Cards = new CardId[this.transform.childCount];

        if (Cards.Length >= 5)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i] = this.transform.GetChild(i).GetComponent<CardId>();
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
                        if ((int)Cards[c].CardNumber == i && (int)Cards[c].CardIdentity == s)
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
                    if ((int)Cards[c].CardIdentity == (int)Cards[c + 1].CardIdentity && (int)Cards[c + 1].CardIdentity == (int)Cards[c + 2].CardIdentity && (int)Cards[c + 2].CardIdentity == (int)Cards[c + 3].CardIdentity && (int)Cards[c + 3].CardIdentity == (int)Cards[c + 4].CardIdentity)
                    {
                        if ((int)Cards[c].CardNumber + 1 == (int)Cards[c + 1].CardNumber && (int)Cards[c + 1].CardNumber + 1 == (int)Cards[c + 2].CardNumber && (int)Cards[c + 2].CardNumber + 1 == (int)Cards[c + 3].CardNumber && (int)Cards[c + 3].CardNumber + 1 == (int)Cards[c + 4].CardNumber)
                        {
                            PlayingDeck.CleanTheChild();

                            Cards[c].transform.parent = PlayingDeck.transform;
                            Cards[c].MoveTheCards();
                            Cards[c + 1].transform.parent = PlayingDeck.transform;
                            Cards[c + 1].MoveTheCards();
                            Cards[c + 2].transform.parent = PlayingDeck.transform;
                            Cards[c + 2].MoveTheCards();
                            Cards[c + 3].transform.parent = PlayingDeck.transform;
                            Cards[c + 3].MoveTheCards();
                            Cards[c + 4].transform.parent = PlayingDeck.transform;
                            Cards[c + 4].MoveTheCards();

                            ArrangeCardOnDeck.ArrangeCard();
                            CheckRemainingCards();
                            PlayingDeck.RoyalFlushCard();
                            PlayingDeck.UpdateText("Royal Flush");
                            PlayingDeck.ArrangeTheCards();

                            if (Bot != null)
                            {
                                Bot.PlayTurn(0);
                                turn = false;
                            }
                            else if (Player != null)
                            {
                                Player.PassCounter = 0;
                                Player.turn = true;
                                Player.TurnOnPassButton();
                                turn = false;
                            }
                            c = Cards.Length;
                        }
                    }
                }
            }
        }

        Debug.Log(this.transform.name + " Royal Flush Checked ************* ");
        yield return null;
    }

    IEnumerator CheckFlush(int PassCounter)
    {
        Cards = new CardId[this.transform.childCount];

        if (Cards.Length >= 5)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i] = this.transform.GetChild(i).GetComponent<CardId>();
            }

            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i].transform.parent = null;
            }


            for (int s = 1; s <= 4; s++)
            {
                for (int c = 0; c < Cards.Length; c++)
                {
                    if ((int)Cards[c].CardIdentity == s)
                    {
                        Cards[c].transform.parent = this.transform;
                    }
                }
            }

            for (int c = 0; c < Cards.Length; c++)
            {
                if (Cards.Length - c >= 5)
                {
                    if ((int)Cards[c].CardIdentity == (int)Cards[c + 1].CardIdentity && (int)Cards[c + 1].CardIdentity == (int)Cards[c + 2].CardIdentity && (int)Cards[c + 2].CardIdentity == (int)Cards[c + 3].CardIdentity && (int)Cards[c + 3].CardIdentity == (int)Cards[c + 4].CardIdentity)
                    {
                        PlayingDeck.CleanTheChild();

                        Cards[c].transform.parent = PlayingDeck.transform;
                        Cards[c].MoveTheCards();
                        Cards[c + 1].transform.parent = PlayingDeck.transform;
                        Cards[c + 1].MoveTheCards();
                        Cards[c + 2].transform.parent = PlayingDeck.transform;
                        Cards[c + 2].MoveTheCards();
                        Cards[c + 3].transform.parent = PlayingDeck.transform;
                        Cards[c + 3].MoveTheCards();
                        Cards[c + 4].transform.parent = PlayingDeck.transform;
                        Cards[c + 4].MoveTheCards();

                        ArrangeCardOnDeck.ArrangeCard();
                        CheckRemainingCards();
                        PlayingDeck.FlushCard();
                        PlayingDeck.UpdateText("Flush");
                        PlayingDeck.ArrangeTheCards();

                        if (Bot != null)
                        {
                            Bot.PlayTurn(0);
                            turn = false;
                        }
                        else if (Player != null)
                        {
                            Player.PassCounter = 0;
                            Player.turn = true;
                            Player.TurnOnPassButton();
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
        Cards = new CardId[this.transform.childCount];

        if (Cards.Length >= 5)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i] = this.transform.GetChild(i).GetComponent<CardId>();
            }

            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i].transform.parent = null;
            }


            for (int c = 0; c < Cards.Length; c++)
            {
                for (int s = 2; s <= 14; s++)
                {
                    if ((int)Cards[c].CardNumber == s)
                    {
                        Cards[c].transform.parent = this.transform;
                    }
                }
            }

            for (int c = 0; c < Cards.Length; c++)
            {
                if (Cards.Length - c >= 5)
                {
                    if ((int)Cards[c].CardNumber + 1 == (int)Cards[c + 1].CardNumber && (int)Cards[c + 1].CardNumber + 1 == (int)Cards[c + 2].CardNumber && (int)Cards[c + 2].CardNumber + 1 == (int)Cards[c + 3].CardNumber && (int)Cards[c + 3].CardNumber + 1 == (int)Cards[c + 4].CardNumber)
                    {
                        PlayingDeck.CleanTheChild();

                        Cards[c].transform.parent = PlayingDeck.transform;
                        Cards[c].MoveTheCards();
                        Cards[c + 1].transform.parent = PlayingDeck.transform;
                        Cards[c + 1].MoveTheCards();
                        Cards[c + 2].transform.parent = PlayingDeck.transform;
                        Cards[c + 2].MoveTheCards();
                        Cards[c + 3].transform.parent = PlayingDeck.transform;
                        Cards[c + 3].MoveTheCards();
                        Cards[c + 4].transform.parent = PlayingDeck.transform;
                        Cards[c + 4].MoveTheCards();

                        ArrangeCardOnDeck.ArrangeCard();
                        CheckRemainingCards();
                        PlayingDeck.StraightCard();
                        PlayingDeck.UpdateText("Straight");
                        PlayingDeck.ArrangeTheCards();

                        if (Bot != null)
                        {
                            Bot.PlayTurn(0);
                            turn = false;
                        }
                        else if (Player != null)
                        {
                            Player.PassCounter = 0;
                            Player.turn = true;
                            Player.TurnOnPassButton();
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
        Cards = new CardId[this.transform.childCount];

        if (Cards.Length >= 5)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i] = this.transform.GetChild(i).GetComponent<CardId>();
            }

            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i].transform.parent = null;
            }

            for (int s = 2; s <= 14; s++)
            {
                for (int c = 0; c < Cards.Length; c++)
                {
                    if ((int)Cards[c].CardNumber == s)
                    {
                        Cards[c].transform.parent = this.transform;
                    }
                }
            }

            for (int c = 0; c < Cards.Length; c++)
            {
                if (Cards.Length - c >= 5)
                {
                    if ((int)Cards[c].CardNumber == (int)Cards[c + 1].CardNumber && (int)Cards[c + 1].CardNumber == (int)Cards[c + 2].CardNumber && (int)Cards[c + 2].CardNumber == (int)Cards[c + 3].CardNumber)
                    {
                        PlayingDeck.CleanTheChild();

                        Cards[c].transform.parent = PlayingDeck.transform;
                        Cards[c].MoveTheCards();
                        Cards[c + 1].transform.parent = PlayingDeck.transform;
                        Cards[c + 1].MoveTheCards();
                        Cards[c + 2].transform.parent = PlayingDeck.transform;
                        Cards[c + 2].MoveTheCards();
                        Cards[c + 3].transform.parent = PlayingDeck.transform;
                        Cards[c + 3].MoveTheCards();
                        Cards[c + 4].transform.parent = PlayingDeck.transform;
                        Cards[c + 4].MoveTheCards();

                        ArrangeCardOnDeck.ArrangeCard();
                        CheckRemainingCards();
                        PlayingDeck.FourOfKindCard();
                        PlayingDeck.UpdateText("Four Of A Kind");
                        PlayingDeck.ArrangeTheCards();

                        if (Bot != null)
                        {
                            Bot.PlayTurn(0);
                            turn = false;
                        }
                        else if (Player != null)
                        {
                            Player.PassCounter = 0;
                            Player.turn = true;
                            Player.TurnOnPassButton();
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
        Cards = new CardId[this.transform.childCount];

        if (Cards.Length >= 3)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i] = this.transform.GetChild(i).GetComponent<CardId>();
            }

            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i].transform.parent = null;
            }

            for (int s = 2; s <= 14; s++)
            {
                for (int c = 0; c < Cards.Length; c++)
                {
                    if ((int)Cards[c].CardNumber == s)
                    {
                        Cards[c].transform.parent = this.transform;
                    }
                }
            }

            for (int c = 0; c < Cards.Length; c++)
            {
                if (Cards.Length - c >= 3)
                {
                    if ((int)Cards[c].CardNumber == (int)Cards[c + 1].CardNumber && (int)Cards[c + 1].CardNumber == (int)Cards[c + 2].CardNumber)
                    {
                        PlayingDeck.CleanTheChild();

                        Cards[c].transform.parent = PlayingDeck.transform;
                        Cards[c].MoveTheCards();
                        Cards[c + 1].transform.parent = PlayingDeck.transform;
                        Cards[c + 1].MoveTheCards();
                        Cards[c + 2].transform.parent = PlayingDeck.transform;
                        Cards[c + 2].MoveTheCards();

                        ArrangeCardOnDeck.ArrangeCard();
                        CheckRemainingCards();
                        PlayingDeck.ThreeOfKindCard();
                        PlayingDeck.UpdateText("Threes");
                        PlayingDeck.ArrangeTheCards();

                        if (Bot != null)
                        {
                            Bot.PlayTurn(0);
                            turn = false;
                        }
                        else if (Player != null)
                        {
                            Player.PassCounter = 0;
                            Player.turn = true;
                            Player.TurnOnPassButton();
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
        Cards = new CardId[this.transform.childCount];

        if (Cards.Length >= 2)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i] = this.transform.GetChild(i).GetComponent<CardId>();
            }

            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i].transform.parent = null;
            }

            for (int s = 2; s <= 14; s++)
            {
                for (int c = 0; c < Cards.Length; c++)
                {
                    if ((int)Cards[c].CardNumber == s)
                    {
                        Cards[c].transform.parent = this.transform;
                    }
                }
            }

            for (int c = 0; c < Cards.Length; c++)
            {
                if (Cards.Length - c >= 2)
                {
                    if ((int)Cards[c].CardNumber == (int)Cards[c + 1].CardNumber)
                    {
                        PlayingDeck.CleanTheChild();

                        Cards[c].transform.parent = PlayingDeck.transform;
                        Cards[c].MoveTheCards();
                        Cards[c + 1].transform.parent = PlayingDeck.transform;
                        Cards[c + 1].MoveTheCards();

                        ArrangeCardOnDeck.ArrangeCard();
                        CheckRemainingCards();
                        PlayingDeck.PairCard();
                        PlayingDeck.UpdateText("Pair");
                        PlayingDeck.ArrangeTheCards();

                        if (Bot != null)
                        {
                            Bot.PlayTurn(0);
                            turn = false;
                        }
                        else if (Player != null)
                        {
                            Player.PassCounter = 0;
                            Player.turn = true;
                            Player.TurnOnPassButton();
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
        Cards = new CardId[this.transform.childCount];

        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i] = this.transform.GetChild(i).GetComponent<CardId>();
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
                    if ((int)Cards[s].CardIdentity == i && (int)Cards[s].SingleCardNumber == a)
                    {
                        Cards[s].transform.parent = this.transform;
                    }
                }
            }
        }

        for (int i = 0; i < Cards.Length; i++)
        {
            PlayingDeck.CleanTheChild();
            Cards[i].transform.parent = PlayingDeck.transform;
            Cards[i].MoveTheCards();
            ArrangeCardOnDeck.ArrangeCard();
            CheckRemainingCards();
            PlayingDeck.SingleCard();
            PlayingDeck.ArrangeTheCards();
            if (Bot != null)
            {
                Bot.PlayTurn(0);
                turn = false;
            }
            else if (Player != null)
            {
                Player.PassCounter = 0;
                Player.turn = true;
                Player.TurnOnPassButton();
                turn = false;
            }
            i = Cards.Length;
        }

        Debug.Log(this.transform.name + " Single Card Checked ************* ");

        yield return null;
    }

    public void UpdateText(string TextUpdt)
    {
        Text.SetActive(false);
        Text.GetComponent<Text>().text = TextUpdt;
        Text.SetActive(true);
    }
}
