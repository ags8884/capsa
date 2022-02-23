using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardCheckerDeck : MonoBehaviour {


    public CardId[] Cards;
    public Dictionary<int, int> PairsDic;
    public Dictionary<int, int> RoyalFlushDic;
    public List<int> CardNumberList;
    public List<int> CardSuitList;


    string SuitName;

    public void CheckAllCard() {
        StartCoroutine(OnePair()); // temporary line
    }

    IEnumerator OnePair()
    {
        Cards = new CardId[this.transform.childCount];

        PairsDic = new Dictionary<int, int>();

        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i] = this.transform.GetChild(i).GetComponent<CardId>();

            if (!PairsDic.ContainsKey((int)Cards[i].CardNumber))
            {
                PairsDic.Add((int)Cards[i].CardNumber, 1);
            }
            else
            {
                int count = 0;
                PairsDic.TryGetValue((int)Cards[i].CardNumber, out count);
                PairsDic.Remove((int)Cards[i].CardNumber);
                PairsDic.Add((int)Cards[i].CardNumber, count + 1);
            }
        }

        foreach (KeyValuePair<int, int> entry in PairsDic)
        {
            if (entry.Value >= 2)
            {
               Debug.Log(this.gameObject.name + " : Compatible To Be Pairs is " + entry.Key + " x " + entry.Value);
            }
        }

        StartCoroutine(ThreeOfKind()); // temporary line
        yield return null;
    }

    IEnumerator ThreeOfKind()
    {
        Cards = new CardId[this.transform.childCount];

        PairsDic = new Dictionary<int, int>();

        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i] = this.transform.GetChild(i).GetComponent<CardId>();

            if (!PairsDic.ContainsKey((int)Cards[i].CardNumber))
            {
                PairsDic.Add((int)Cards[i].CardNumber, 1);
            }
            else
            {
                int count = 0;
                PairsDic.TryGetValue((int)Cards[i].CardNumber, out count);
                PairsDic.Remove((int)Cards[i].CardNumber);
                PairsDic.Add((int)Cards[i].CardNumber, count + 1);
            }
        }

        foreach (KeyValuePair<int, int> entry in PairsDic)
        {
            if (entry.Value >= 3)
            {
                Debug.Log(this.gameObject.name + " : Compatible To Be Three Of Kinds is " + entry.Key + " x " + entry.Value);
            }
        }

        StartCoroutine(FourOfKind()); // temporary line
        yield return null;
    }

    IEnumerator FourOfKind()
    {
        Cards = new CardId[this.transform.childCount];

        PairsDic = new Dictionary<int, int>();

        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i] = this.transform.GetChild(i).GetComponent<CardId>();

            if (!PairsDic.ContainsKey((int)Cards[i].CardNumber))
            {
                PairsDic.Add((int)Cards[i].CardNumber, 1);
            }
            else
            {
                int count = 0;
                PairsDic.TryGetValue((int)Cards[i].CardNumber, out count);
                PairsDic.Remove((int)Cards[i].CardNumber);
                PairsDic.Add((int)Cards[i].CardNumber, count + 1);
            }
        }

        foreach (KeyValuePair<int, int> entry in PairsDic)
        {
            if (entry.Value >= 4)
            {
               Debug.Log(this.gameObject.name + " : Compatible To Be Four Of Kinds is " + entry.Key + " x " + entry.Value);
            }
        }

        StartCoroutine(Straight()); // temporary line
        yield return null;
    }


    IEnumerator Straight()
    {
        Cards = new CardId[this.transform.childCount];

        CardNumberList = new List<int>(new int[Cards.Length]);

        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i] = this.transform.GetChild(i).GetComponent<CardId>();

            CardNumberList[i] = (int)Cards[i].CardNumber;
        }

        CardNumberList = CardNumberList.Distinct().ToList();
        CardNumberList.Sort();

        for (int i = 0; i < CardNumberList.Count; i++) {
            if (CardNumberList.Count - i >= 5)
            {
                if (CardNumberList[i] + 1 == CardNumberList[i + 1] && CardNumberList[i + 1] + 1 == CardNumberList[i + 2] && CardNumberList[i + 2] + 1 == CardNumberList[i + 3] && CardNumberList[i + 3] + 1 == CardNumberList[i + 4])
                {
                    Debug.Log(this.gameObject.name + " : Compatible To Be Straight is " + CardNumberList[i] + " & " + CardNumberList[i + 1] + " & " + CardNumberList[i + 2] + " & " + CardNumberList[i + 3] + " & " + CardNumberList[i + 4]);
                }
            }
        }

        StartCoroutine(Flush());
        yield return null;
    }

    IEnumerator Flush()
    {
        Cards = new CardId[this.transform.childCount];

        RoyalFlushDic = new Dictionary<int, int>();

        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i] = this.transform.GetChild(i).GetComponent<CardId>();

            if (!RoyalFlushDic.ContainsKey((int)Cards[i].CardIdentity))
            {
                RoyalFlushDic.Add((int)Cards[i].CardIdentity, 1);
            }
            else if (RoyalFlushDic.ContainsKey((int)Cards[i].CardIdentity))
            {
                int count = 0;
                RoyalFlushDic.TryGetValue((int)Cards[i].CardIdentity, out count);
                RoyalFlushDic.Remove((int)Cards[i].CardIdentity);
                RoyalFlushDic.Add((int)Cards[i].CardIdentity, count + 1);
            }

            foreach (KeyValuePair<int, int> entry in RoyalFlushDic)
            {

                if (entry.Value == 5)
                {

                    if (entry.Key == 1)
                    {
                        SuitName = "Diamond";
                    }
                    else if (entry.Key == 2)
                    {
                        SuitName = "Club";
                    }
                    else if (entry.Key == 3)
                    {
                        SuitName = "Heart";
                    }
                    else if (entry.Key == 4)
                    {
                        SuitName = "Spade";
                    }

                    Debug.Log(this.gameObject.name + " : Compatible To Be Flush is " + SuitName + " x " + entry.Value);
                }
            }
        }

        StartCoroutine(RoyalFlush());
        yield return null;
    }

    IEnumerator RoyalFlush()
    {
        Cards = new CardId[this.transform.childCount];

        CardNumberList = new List<int>(new int[Cards.Length]);

        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i] = this.transform.GetChild(i).GetComponent<CardId>();

            CardNumberList[i] = (int)Cards[i].CardNumber;
        }

        CardNumberList = CardNumberList.Distinct().ToList();
        CardNumberList.Sort();


        for (int i = 0; i < CardNumberList.Count; i++)
        {
            if (CardNumberList.Count - i >= 5)
            {
                if (CardNumberList[i] + 1 == CardNumberList[i + 1] && CardNumberList[i + 1] + 1 == CardNumberList[i + 2] && CardNumberList[i + 2] + 1 == CardNumberList[i + 3] && CardNumberList[i + 3] + 1 == CardNumberList[i + 4])
                {
                    RoyalFlushDic = new Dictionary<int, int>();

                    for (int checkRF = 0; checkRF < Cards.Length; checkRF++)
                    {
                        Cards[checkRF] = this.transform.GetChild(checkRF).GetComponent<CardId>();

                        if (!RoyalFlushDic.ContainsKey((int)Cards[checkRF].CardIdentity) && ((int)Cards[checkRF].CardNumber == CardNumberList[i] || (int)Cards[checkRF].CardNumber == CardNumberList[i + 1] || (int)Cards[checkRF].CardNumber == CardNumberList[i + 2] || (int)Cards[checkRF].CardNumber == CardNumberList[i + 3] || (int)Cards[checkRF].CardNumber == CardNumberList[i + 4]))
                        {
                            RoyalFlushDic.Add((int)Cards[checkRF].CardIdentity, 1);
                        }
                        else if(RoyalFlushDic.ContainsKey((int)Cards[checkRF].CardIdentity) && ((int)Cards[checkRF].CardNumber == CardNumberList[i] || (int)Cards[checkRF].CardNumber == CardNumberList[i + 1] || (int)Cards[checkRF].CardNumber == CardNumberList[i + 2] || (int)Cards[checkRF].CardNumber == CardNumberList[i + 3] || (int)Cards[checkRF].CardNumber == CardNumberList[i + 4]))
                        {
                            int count = 0;
                            RoyalFlushDic.TryGetValue((int)Cards[checkRF].CardIdentity, out count);
                            RoyalFlushDic.Remove((int)Cards[checkRF].CardIdentity);
                            RoyalFlushDic.Add((int)Cards[checkRF].CardIdentity, count + 1);
                        }
                    }

                    foreach (KeyValuePair<int, int> entry in RoyalFlushDic)
                    {

                        if (entry.Value >= 5)
                        {

                            if (entry.Key == 1)
                            {
                                SuitName = "Spade";
                            } else if (entry.Key == 2)
                            {
                                SuitName = "Heart";
                            }
                            else if (entry.Key == 3)
                            {
                                SuitName = "Club";
                            }
                            else if (entry.Key == 4)
                            {
                                SuitName = "Diamond";
                            }

                            Debug.Log(this.gameObject.name + " : Compatible To Be RoyalFlush is "+ SuitName + " x " + entry.Value + " and Cards Number : " + CardNumberList[i] + " & " + CardNumberList[i + 1] + " & " + CardNumberList[i + 2] + " & " + CardNumberList[i + 3] + " & " + CardNumberList[i + 4]);
                        }
                    }
                }
            }
        }

        yield return null;
    }
}
