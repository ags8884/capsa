using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerBehaviour : MonoBehaviour {

    public Transform P1Deck, SelectionDeck;
    public Material CardNonSelected, CardSelected;
    private PlayerBehaviour PB;
    private Transform Selected;
    // Use this for initialization
    void Start()
    {
        PB = P1Deck.GetComponent<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Selected != null)
        {
            Selected.GetComponent<MeshRenderer>().material = CardNonSelected;
            Selected = null;
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            if (hit.transform.parent == P1Deck)
            {
                Selected = hit.transform;
                Selected.GetComponent<MeshRenderer>().material = CardSelected;
                if (Input.GetButtonDown("Fire1"))
                {
                    Debug.Log("You selected the " + hit.transform.name);
                    hit.transform.localPosition = new Vector3(hit.transform.localPosition.x, 0.75f, hit.transform.localPosition.z);
                    PB.Clicked((int)hit.transform.GetComponent<CardId>().CardIdentity, (int)hit.transform.GetComponent<CardId>().SingleCardNumber, hit.transform.GetSiblingIndex());
                    hit.transform.parent = SelectionDeck;
                }
            }
            else if (hit.transform.parent == SelectionDeck)
            {
                Selected = hit.transform;
                Selected.GetComponent<MeshRenderer>().material = CardSelected;
                if (Input.GetButtonDown("Fire1"))
                {
                    Debug.Log("You selected the " + hit.transform.name);
                    hit.transform.localPosition = new Vector3(hit.transform.localPosition.x, 0, hit.transform.localPosition.z);
                    PB.Clicked((int)hit.transform.GetComponent<CardId>().CardIdentity, (int)hit.transform.GetComponent<CardId>().SingleCardNumber, hit.transform.GetSiblingIndex());
                    hit.transform.parent = P1Deck;
                }
            }
        }
    }
}
