﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card card;

    public Text nameText;
    public Text descriptionText;
    public Image art;
    public PlayerCharacter owner;
    
    // Start is called before the first frame update
    void Start()
    {
        //nameText.text = card.cardName;
        descriptionText.text = card.description;

        art.sprite = card.art;
    }

    public void SetColor(Color color)
    {
        gameObject.GetComponent<Image>().color = color;
    }    

    public void SetFontSize(int size)
    {
        descriptionText.fontSize = size;
    }
}
