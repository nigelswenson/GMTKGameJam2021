using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDisplay : MonoBehaviour
{

    public PlayerCharacter character;
    public Text namePlate;
    public GameObject cardArea;
    public Slider hpSlider;
    public Text hpText;
    public Image bleedImage;
    public Text bleedCount;


    // Start is called before the first frame update
    void Start()
    {
        namePlate.text = character.characterName;
        hpSlider.maxValue = character.maxHp;
        SetHp();
        SetBleed(0);
    }

    public void SetHp()
    {
        hpSlider.value = character.currentHp;
        hpText.text = character.currentHp + "/" + character.maxHp;
    }    
    public void SetBleed(int bleed)
    {
        if(bleed >= 0)
        {
            bleedImage.enabled = false;
            bleedCount.enabled = false;
        }
        else
        {
            bleedCount.text = bleed.ToString();
            bleedImage.enabled = true;
            bleedCount.enabled = true;
        }

    }    
}
