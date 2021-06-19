using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardZoom : MonoBehaviour
{
    private GameObject canvas;
    private GameObject zoomCard;
    private GameObject cardArt;
    private GameObject cardDescription;

    public void Awake()
    {
        canvas = GameObject.Find("Main Canvas");
    }

    public void OnHoverEnter()
    {
        //gameobject refers to the object this script is attached to
        zoomCard = Instantiate(gameObject, new Vector2(Input.mousePosition.x, Input.mousePosition.y + 250), Quaternion.identity);
        zoomCard.transform.SetParent(canvas.transform, false);
        zoomCard.GetComponent<RectTransform>().sizeDelta = new Vector2(301, 432);

        cardArt = zoomCard.transform.Find("CardArt").gameObject;
        cardDescription = zoomCard.transform.Find("Description").gameObject;

        cardArt.GetComponent<RectTransform>().sizeDelta = new Vector2(252, 224);
        cardArt.GetComponent<RectTransform>().position = new Vector2(cardArt.GetComponent<RectTransform>().position.x, cardArt.GetComponent<RectTransform>().position.y-55);
        cardDescription.GetComponent<RectTransform>().sizeDelta = new Vector2(236, 152);
        cardDescription.GetComponent<RectTransform>().position = new Vector2(cardDescription.GetComponent<RectTransform>().position.x, cardDescription.GetComponent<RectTransform>().position.y - 50);
        cardDescription.GetComponent<Text>().fontSize = 37;
        //sets the layer of the zoom card to something else so it can't collide with anything on the game layer
        zoomCard.layer = LayerMask.NameToLayer("Zoom");
    }

    public void OnHoverExit()
    {
        Destroy(zoomCard);
    }
}


