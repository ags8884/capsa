using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardId : MonoBehaviour {

    public enum IDCards { SPADE = 4, HEART = 3, CLUB = 2, DIAMOND = 1};
    public enum NumberCards { TWO = 2, THREE = 3, FOUR = 4, FIVE = 5, SIX = 6, SEVEN = 7, EIGHT = 8, NINE = 9, TEN = 10, JACK = 11, QUEEN = 12, KING = 13, AS = 14};
    public enum SingleNumberCards {THREE = 3, FOUR = 4, FIVE = 5, SIX = 6, SEVEN = 7, EIGHT = 8, NINE = 9, TEN = 10, JACK = 11, QUEEN = 12, KING = 13, AS = 14, TWO = 15};

    public IDCards CardIdentity;
    public NumberCards CardNumber;
    public SingleNumberCards SingleCardNumber;

    public float Speed;

	// Use this for initialization
	void Start ()
    {
        this.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    public void MoveTheCards() {
        StartCoroutine(SCMovetheCards(this.transform.localPosition.z - (this.transform.GetSiblingIndex() * 0.05f)));
    }

    IEnumerator SCMovetheCards( float distancebetweencards) {
        float timeToStart = Time.time;

        while (this.transform.localPosition.x != 0 || this.transform.localPosition.y != 0)
        {

            this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, new Vector3(0, 0, distancebetweencards), (Time.time - timeToStart) * Speed);

            this.transform.Rotate(Vector3.forward * ((Time.time - timeToStart) * (Speed * 100)));

            if (this.transform.parent.name != "P1Deck")
            {
                if (this.transform.localPosition.x == 0 && this.transform.localPosition.y == 0)
                {
                    this.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                }
            }
            else
            {
                if (this.transform.localPosition.x == 0 && this.transform.localPosition.y == 0)
                {
                    this.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
                }
            }

            yield return null;
        }

        if (this.transform.parent.name == "PlayingDeck" && this.transform.localRotation.y != 180 && this.transform.localPosition.x == 0 && this.transform.localPosition.y == 0)
        {
            this.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }
}
