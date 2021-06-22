using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardZoom : MonoBehaviour
{
    private GameObject canvas;

    public GameObject card;
    public Image cardsArt;
    public Text cardsDescription;
    public Text startCardDescription;
    public Vector2 startCardPosition;
    public Vector2 startCardSizeDelta;
    public Vector2 startArtPosition;
    public Vector2 startArtSizeDelta;
    public Vector2 startDescriptionPosition;
    public Vector2 startDescriptionSizeDelta;
    public GameObject startParent;

    public bool beingDragged = false;

    private void Awake()
    {
        canvas = GameObject.Find("Main Canvas");
    }

    public void OnHoverEnter()
    {
        if(!beingDragged)
        {
            //set start positions to go back to on shrink
            startParent = transform.parent.gameObject;
            startCardPosition = card.GetComponent<RectTransform>().position;
            startCardSizeDelta = card.GetComponent<RectTransform>().sizeDelta;
            startArtPosition = cardsArt.GetComponent<RectTransform>().position;
            startArtSizeDelta = cardsArt.GetComponent<RectTransform>().sizeDelta;
            startDescriptionPosition = cardsDescription.GetComponent<RectTransform>().position;
            startDescriptionSizeDelta = cardsDescription.GetComponent<RectTransform>().sizeDelta;


            transform.SetParent(canvas.transform, true);
            //set increased card size
            card.GetComponent<RectTransform>().sizeDelta = new Vector2(301, 432);
            cardsArt.GetComponent<RectTransform>().sizeDelta = new Vector2(252, 224);
            cardsArt.GetComponent<RectTransform>().position = new Vector2(cardsArt.GetComponent<RectTransform>().position.x, cardsArt.GetComponent<RectTransform>().position.y - 30);
            cardsDescription.GetComponent<RectTransform>().sizeDelta = new Vector2(236, 152);
            cardsDescription.GetComponent<RectTransform>().position = new Vector2(cardsDescription.GetComponent<RectTransform>().position.x, cardsDescription.GetComponent<RectTransform>().position.y - 25);
            cardsDescription.GetComponent<Text>().fontSize = gameObject.GetComponent<CardDisplay>().card.fontSize + 17;
        }
    }

    public void SetDrag()
    {
        //determines if we were dragging the card, vs just hovering it
        beingDragged = true;
    }

    public void EndDrag()
    {
        beingDragged = false;
    }

    public void OnHoverExit()
    {
        if (!beingDragged || !gameObject.GetComponent<CardDisplay>().card.wasPlayed)
        {
            if (card.GetComponent<RectTransform>().sizeDelta != startCardSizeDelta)
            {
                //reset transform to shrink the card back down
                transform.SetParent(startParent.transform, true);
                card.GetComponent<RectTransform>().sizeDelta = startCardSizeDelta;
                cardsArt.GetComponent<RectTransform>().sizeDelta = startArtSizeDelta;
                cardsArt.GetComponent<RectTransform>().position = startArtPosition;
                cardsDescription.GetComponent<RectTransform>().sizeDelta = startDescriptionSizeDelta;
                cardsDescription.GetComponent<RectTransform>().position = startDescriptionPosition;
                cardsDescription.GetComponent<Text>().fontSize = gameObject.GetComponent<CardDisplay>().card.fontSize;
                beingDragged = false;
            }    
        }
    }
}


