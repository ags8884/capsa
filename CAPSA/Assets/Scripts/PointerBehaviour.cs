using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform.parent.name == "P1Deck")
                {
                    if (hit.transform.localPosition.y != 0)
                    {
                        Debug.Log("You selected the " + hit.transform.name);
                        hit.transform.localPosition = new Vector3(hit.transform.localPosition.x, 0, hit.transform.localPosition.z);
                    }
                    else
                    {
                        Debug.Log("You selected the " + hit.transform.name);
                        hit.transform.parent.GetComponent<PlayerBehaviour>().ArrangeTheCard();
                        hit.transform.localPosition = new Vector3(hit.transform.localPosition.x, 0.75f, hit.transform.localPosition.z);
                        hit.transform.parent.GetComponent<PlayerBehaviour>().Clicked((int)hit.transform.GetComponent<CardId>().CardIdentity, (int)hit.transform.GetComponent<CardId>().SingleCardNumber, hit.transform.GetSiblingIndex());
                    }
                }
            }
        }
    }
}
