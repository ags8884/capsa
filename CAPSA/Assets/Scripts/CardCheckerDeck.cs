using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardCheckerDeck : MonoBehaviour {


    public GameObject[] Cards;
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
        Cards = new GameObject[this.transform.childCount];

        PairsDic = new Dictionary<int, int>();

        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i] = this.transform.GetChild(i).gameObject;

            if (!PairsDic.ContainsKey((int)Cards[i].GetComponent<CardId>().CardNumber))
            {
                PairsDic.Add((int)Cards[i].GetComponent<CardId>().CardNumber, 1);
            }
            else
            {
                int count = 0;
                PairsDic.TryGetValue((int)Cards[i].GetComponent<CardId>().CardNumber, out count);
                PairsDic.Remove((int)Cards[i].GetComponent<CardId>().CardNumber);
                PairsDic.Add((int)Cards[i].GetComponent<CardId>().CardNumber, count + 1);
            }
        }

        foreach (KeyValuePair<int, int> entry in PairsDic)
        {
            if (entry.Value >= 2)
            {
               // Debug.Log(this.gameObject.name + " : Compatible To Be Pairs is " + entry.Key + " x " + entry.Value);
            }
        }

        StartCoroutine(ThreeOfKind()); // temporary line
        yield return null;
    }

    IEnumerator ThreeOfKind()
    {
        Cards = new GameObject[this.transform.childCount];

        PairsDic = new Dictionary<int, int>();

        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i] = this.transform.GetChild(i).gameObject;

            if (!PairsDic.ContainsKey((int)Cards[i].GetComponent<CardId>().CardNumber))
            {
                PairsDic.Add((int)Cards[i].GetComponent<CardId>().CardNumber, 1);
            }
            else
            {
                int count = 0;
                PairsDic.TryGetValue((int)Cards[i].GetComponent<CardId>().CardNumber, out count);
                PairsDic.Remove((int)Cards[i].GetComponent<CardId>().CardNumber);
                PairsDic.Add((int)Cards[i].GetComponent<CardId>().CardNumber, count + 1);
            }
        }

        foreach (KeyValuePair<int, int> entry in PairsDic)
        {
            if (entry.Value >= 3)
            {
               // Debug.Log(this.gameObject.name + " : Compatible To Be Three Of Kinds is " + entry.Key + " x " + entry.Value);
            }
        }

        StartCoroutine(FourOfKind()); // temporary line
        yield return null;
    }

    IEnumerator FourOfKind()
    {
        Cards = new GameObject[this.transform.childCount];

        PairsDic = new Dictionary<int, int>();

        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i] = this.transform.GetChild(i).gameObject;

            if (!PairsDic.ContainsKey((int)Cards[i].GetComponent<CardId>().CardNumber))
            {
                PairsDic.Add((int)Cards[i].GetComponent<CardId>().CardNumber, 1);
            }
            else
            {
                int count = 0;
                PairsDic.TryGetValue((int)Cards[i].GetComponent<CardId>().CardNumber, out count);
                PairsDic.Remove((int)Cards[i].GetComponent<CardId>().CardNumber);
                PairsDic.Add((int)Cards[i].GetComponent<CardId>().CardNumber, count + 1);
            }
        }

        foreach (KeyValuePair<int, int> entry in PairsDic)
        {
            if (entry.Value >= 4)
            {
               // Debug.Log(this.gameObject.name + " : Compatible To Be Four Of Kinds is " + entry.Key + " x " + entry.Value);
            }
        }

        StartCoroutine(Straight()); // temporary line
        yield return null;
    }


    IEnumerator Straight()
    {
        Cards = new GameObject[this.transform.childCount];

        CardNumberList = new List<int>(new int[Cards.Length]);

        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i] = this.transform.GetChild(i).gameObject;

            CardNumberList[i] = (int)Cards[i].GetComponent<CardId>().CardNumber;
        }

        CardNumberList = CardNumberList.Distinct().ToList();
        CardNumberList.Sort();

        for (int i = 0; i < CardNumberList.Count; i++) {
            if (CardNumberList.Count - i >= 5)
            {
                if (CardNumberList[i] + 1 == CardNumberList[i + 1] && CardNumberList[i + 1] + 1 == CardNumberList[i + 2] && CardNumberList[i + 2] + 1 == CardNumberList[i + 3] && CardNumberList[i + 3] + 1 == CardNumberList[i + 4])
                {
                    //Debug.Log(this.gameObject.name + " : Compatible To Be Straight is " + CardNumberList[i] + " & " + CardNumberList[i + 1] + " & " + CardNumberList[i + 2] + " & " + CardNumberList[i + 3] + " & " + CardNumberList[i + 4]);
                }
            }
        }

        StartCoroutine(Flush());
        yield return null;
    }

    IEnumerator Flush()
    {
        Cards = new GameObject[this.transform.childCount];

        RoyalFlushDic = new Dictionary<int, int>();

        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i] = this.transform.GetChild(i).gameObject;

            if (!RoyalFlushDic.ContainsKey((int)Cards[i].GetComponent<CardId>().CardIdentity))
            {
                RoyalFlushDic.Add((int)Cards[i].GetComponent<CardId>().CardIdentity, 1);
            }
            else if (RoyalFlushDic.ContainsKey((int)Cards[i].GetComponent<CardId>().CardIdentity))
            {
                int count = 0;
                RoyalFlushDic.TryGetValue((int)Cards[i].GetComponent<CardId>().CardIdentity, out count);
                RoyalFlushDic.Remove((int)Cards[i].GetComponent<CardId>().CardIdentity);
                RoyalFlushDic.Add((int)Cards[i].GetComponent<CardId>().CardIdentity, count + 1);
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

                  //  Debug.Log(this.gameObject.name + " : Compatible To Be Flush is " + SuitName + " x " + entry.Value);
                }
            }
        }

        StartCoroutine(RoyalFlush());
        yield return null;
    }

    IEnumerator RoyalFlush()
    {
        Cards = new GameObject[this.transform.childCount];

        CardNumberList = new List<int>(new int[Cards.Length]);

        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i] = this.transform.GetChild(i).gameObject;

            CardNumberList[i] = (int)Cards[i].GetComponent<CardId>().CardNumber;
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
                        Cards[checkRF] = this.transform.GetChild(checkRF).gameObject;

                        if (!RoyalFlushDic.ContainsKey((int)Cards[checkRF].GetComponent<CardId>().CardIdentity) && ((int)Cards[checkRF].GetComponent<CardId>().CardNumber == CardNumberList[i] || (int)Cards[checkRF].GetComponent<CardId>().CardNumber == CardNumberList[i + 1] || (int)Cards[checkRF].GetComponent<CardId>().CardNumber == CardNumberList[i + 2] || (int)Cards[checkRF].GetComponent<CardId>().CardNumber == CardNumberList[i + 3] || (int)Cards[checkRF].GetComponent<CardId>().CardNumber == CardNumberList[i + 4]))
                        {
                            RoyalFlushDic.Add((int)Cards[checkRF].GetComponent<CardId>().CardIdentity, 1);
                        }
                        else if(RoyalFlushDic.ContainsKey((int)Cards[checkRF].GetComponent<CardId>().CardIdentity) && ((int)Cards[checkRF].GetComponent<CardId>().CardNumber == CardNumberList[i] || (int)Cards[checkRF].GetComponent<CardId>().CardNumber == CardNumberList[i + 1] || (int)Cards[checkRF].GetComponent<CardId>().CardNumber == CardNumberList[i + 2] || (int)Cards[checkRF].GetComponent<CardId>().CardNumber == CardNumberList[i + 3] || (int)Cards[checkRF].GetComponent<CardId>().CardNumber == CardNumberList[i + 4]))
                        {
                            int count = 0;
                            RoyalFlushDic.TryGetValue((int)Cards[checkRF].GetComponent<CardId>().CardIdentity, out count);
                            RoyalFlushDic.Remove((int)Cards[checkRF].GetComponent<CardId>().CardIdentity);
                            RoyalFlushDic.Add((int)Cards[checkRF].GetComponent<CardId>().CardIdentity, count + 1);
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

                           // Debug.Log(this.gameObject.name + " : Compatible To Be RoyalFlush is "+ SuitName + " x " + entry.Value + " and Cards Number : " + CardNumberList[i] + " & " + CardNumberList[i + 1] + " & " + CardNumberList[i + 2] + " & " + CardNumberList[i + 3] + " & " + CardNumberList[i + 4]);
                        }
                    }
                }
            }
        }

        yield return null;
    }
}
