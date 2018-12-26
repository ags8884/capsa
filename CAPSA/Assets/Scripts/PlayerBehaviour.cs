using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    public GameObject Text;
    public bool turn;
    public GameObject[] Cards;
    public GameObject[] CardsToBeSetToDeck;
    public GameObject PlayingDeck;
    public GameObject NextPlayer;
    public Button DEAL,PASS;
    public int PassCounter;

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
        DEAL.interactable = false;
        PASS.interactable = false;
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
                }
                else if (NextPlayer.GetComponent<PlayerBehaviour>() != null)
                {
                    NextPlayer.GetComponent<PlayerBehaviour>().PassCounter = 0;
                    NextPlayer.GetComponent<PlayerBehaviour>().turn = true;
                    turn = false;
                }
            }
        }
    }

    public void Clicked(int CardIdentity, int CardNumber, int ChildIndex)
    {
        StartCoroutine(ClickedCard(PassCounter, CardIdentity, CardNumber, ChildIndex));
    }

    public void ArrangeTheCard() {
        Cards = new GameObject[this.transform.childCount];

        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i] = this.transform.GetChild(i).gameObject;
            Cards[i].transform.localPosition = new Vector3(Cards[i].transform.localPosition.x, 0, Cards[i].transform.localPosition.z);
        }
    }

    public void CheckRemainingCards()
    {
        StartCoroutine(CheckRemainingCardsEnum());
    }

    IEnumerator CheckRemainingCardsEnum()
    {
        yield return new WaitForSeconds(0.2f);
        Cards = new GameObject[this.transform.childCount];
        if (Cards.Length <= 0)
        {
            Text.SetActive(false);
            Text.GetComponent<Text>().text = this.transform.name + " WIN!!";
            Text.SetActive(true);
            Time.timeScale = 0;
        }
    }

    IEnumerator ClickedCard(int PassCounter, int CardIdentity, int CardNumber, int ChildIndex)
    {
        turn = true;
        yield return new WaitForSeconds(0.1f);
        if (turn && PlayingDeck.GetComponent<PlayingDeckScript>().RoyalFlush)
        {
            StartCoroutine(CheckRoyalFlush(PassCounter));
        }
        yield return new WaitForSeconds(0.1f);
        if (turn && PlayingDeck.GetComponent<PlayingDeckScript>().Flush)
        {
            StartCoroutine(CheckFlush(PassCounter));
        }
        yield return new WaitForSeconds(0.1f);
        if (turn && PlayingDeck.GetComponent<PlayingDeckScript>().Straight)
        {
            StartCoroutine(CheckStraight(PassCounter));
        }
        yield return new WaitForSeconds(0.1f);
        if (turn && PlayingDeck.GetComponent<PlayingDeckScript>().FourOfKind)
        {
            StartCoroutine(CheckFourOfKind(PassCounter));
        }
        yield return new WaitForSeconds(0.1f);
        if (turn && PlayingDeck.GetComponent<PlayingDeckScript>().ThreeOfKind)
        {
            StartCoroutine(CheckThreeOfKind(PassCounter));
        }
        yield return new WaitForSeconds(0.1f);
        if (turn && PlayingDeck.GetComponent<PlayingDeckScript>().Pair)
        {
            StartCoroutine(CheckPair(PassCounter));
        }
        yield return new WaitForSeconds(0.1f);
        if (turn && PlayingDeck.GetComponent<PlayingDeckScript>().Single)
        {
            StartCoroutine(SingleCard(PassCounter, CardIdentity, CardNumber, ChildIndex));
        }
    }

    IEnumerator CheckRoyalFlush(int PassCounter)
    {
        yield return null;
    }

    IEnumerator CheckFlush(int PassCounter)
    {
        yield return null;
    }

    IEnumerator CheckStraight(int PassCounter)
    {
        yield return null;
    }

    IEnumerator CheckFourOfKind(int PassCounter)
    {
        yield return null;
    }

    IEnumerator CheckThreeOfKind(int PassCounter)
    {
        yield return null;
    }

    IEnumerator CheckPair(int PassCounter)
    {
        yield return null;
    }

    IEnumerator PairCard(int PassCounter, int CardIdentity, int CardNumber, int ChildIndex)
    {
        if ((int)Cards[ChildIndex].GetComponent<CardId>().SingleCardNumber == (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().SingleCardNumber && (int)Cards[ChildIndex].GetComponent<CardId>().CardIdentity > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardIdentity)
        {
            Cards[ChildIndex].transform.localPosition = new Vector3(Cards[ChildIndex].transform.localPosition.x, 0.75f, Cards[ChildIndex].transform.localPosition.z);
            DEAL.interactable = true;
            PASS.interactable = true;
        }
        else if ((int)Cards[ChildIndex].GetComponent<CardId>().SingleCardNumber > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().SingleCardNumber)
        {
            Cards[ChildIndex].transform.localPosition = new Vector3(Cards[ChildIndex].transform.localPosition.x, 0.75f, Cards[ChildIndex].transform.localPosition.z);
            DEAL.interactable = true;
            PASS.interactable = true;
        }
        else
        {
            Cards[ChildIndex].transform.localPosition = new Vector3(Cards[ChildIndex].transform.localPosition.x, 0, Cards[ChildIndex].transform.localPosition.z);
            DEAL.interactable = false;
            PASS.interactable = true;
        }
        yield return null;
    }

    IEnumerator SingleCard(int PassCounter, int CardIdentity, int CardNumber, int ChildIndex)
    {
        if (PassCounter < 3)
        {
            if ((int)Cards[ChildIndex].GetComponent<CardId>().SingleCardNumber == (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().SingleCardNumber && (int)Cards[ChildIndex].GetComponent<CardId>().CardIdentity > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().CardIdentity)
            {
                Cards[ChildIndex].transform.localPosition = new Vector3(Cards[ChildIndex].transform.localPosition.x, 0.75f, Cards[ChildIndex].transform.localPosition.z);
                DEAL.interactable = true;
                PASS.interactable = true;
            }
            else if ((int)Cards[ChildIndex].GetComponent<CardId>().SingleCardNumber > (int)PlayingDeck.transform.GetChild(0).GetComponent<CardId>().SingleCardNumber)
            {
                Cards[ChildIndex].transform.localPosition = new Vector3(Cards[ChildIndex].transform.localPosition.x, 0.75f, Cards[ChildIndex].transform.localPosition.z);
                DEAL.interactable = true;
                PASS.interactable = true;
            }
            else
            {
                Cards[ChildIndex].transform.localPosition = new Vector3(Cards[ChildIndex].transform.localPosition.x, 0, Cards[ChildIndex].transform.localPosition.z);
                DEAL.interactable = false;
                PASS.interactable = true;
            }
        }
        else
        {
            Cards[ChildIndex].transform.localPosition = new Vector3(Cards[ChildIndex].transform.localPosition.x, 0.75f, Cards[ChildIndex].transform.localPosition.z);
            DEAL.interactable = true;
            PASS.interactable = true;
        }
        yield return null;
    }

    public void TurnOnPassButton()
    {
        PASS.interactable = true;
    }

    public void ButtonToDeal() {
        if (PlayingDeck.GetComponent<PlayingDeckScript>().Single)
        {
            if (PassCounter < 3)
            {
                PlayingDeck.GetComponent<PlayingDeckScript>().CleanTheChild();
                for (int i = 0; i < Cards.Length; i++)
                {
                    if (Cards[i].transform.localPosition.y == 0.75f)
                    {
                        Cards[i].transform.parent = PlayingDeck.transform;
                        Cards[i].GetComponent<CardId>().MoveTheCards();
                    }
                }
                this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
                PlayingDeck.GetComponent<PlayingDeckScript>().SingleCard();
                PlayingDeck.GetComponent<PlayingDeckScript>().ArrangeTheCards();
                CheckRemainingCards();
                if (NextPlayer.GetComponent<BotAI>() != null)
                {
                    NextPlayer.GetComponent<BotAI>().PlayTurn(0);
                    turn = false;
                }
            }
            else
            {
                PlayingDeck.GetComponent<PlayingDeckScript>().CleanTheChild();
                for (int i = 0; i < Cards.Length; i++)
                {
                    if (Cards[i].transform.localPosition.y == 0.75f)
                    {
                        Cards[i].transform.parent = PlayingDeck.transform;
                        Cards[i].GetComponent<CardId>().MoveTheCards();
                    }
                }
                this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
                CheckRemainingCards();
                PlayingDeck.GetComponent<PlayingDeckScript>().SingleCard();
                PlayingDeck.GetComponent<PlayingDeckScript>().ArrangeTheCards();
                if (NextPlayer.GetComponent<BotAI>() != null)
                {
                    NextPlayer.GetComponent<BotAI>().PlayTurn(0);
                    turn = false;
                }
            }
        }
        DEAL.interactable = false;
        PASS.interactable = false;
    }

    public void ButtonToPass()
    {
        Debug.Log(this.transform.name + " = Pass");
        Text.SetActive(false);
        Text.GetComponent<Text>().text = "Pass ( " + (PassCounter + 1).ToString() + " )";
        Text.SetActive(true);
        this.GetComponent<ArrangeCardOnDeck>().ArrangeCard();
        PlayingDeck.GetComponent<PlayingDeckScript>().ArrangeTheCards();
        if (NextPlayer.GetComponent<BotAI>() != null)
        {
            NextPlayer.GetComponent<BotAI>().PlayTurn(PassCounter + 1);
            turn = false;
        }
        DEAL.interactable = false;
        PASS.interactable = false;
    }
}
