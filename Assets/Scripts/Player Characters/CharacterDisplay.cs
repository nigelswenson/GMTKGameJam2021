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
    public Image armorImage;
    public Text armorCount;
    public Image targetIndicator;
    public Text targetDamage;


    // Start is called before the first frame update
    void Start()
    {
        namePlate.text = character.characterName;
        hpSlider.maxValue = character.maxHp;
        SetHp();
        SetBleed(0);
        SetArmor(0);
        targetIndicator.enabled = false;
    }

    public void SetHp()
    {
        hpSlider.value = character.currentHp;
        hpText.text = character.currentHp + "/" + character.maxHp;
    }    
    public void SetBleed(int bleed)
    {
        if(bleed <= 0)
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

    public void SetArmor(int armor)
    {
        if (armor <= 0)
        {
            armorImage.enabled = false;
            armorCount.enabled = false;
        }
        else
        {
            armorCount.text = armor.ToString();
            armorImage.enabled = true;
            armorCount.enabled = true;
        }
    }

    public void EnableTargetIndicator(string damage = null)
    {
        targetIndicator.enabled = true;
        if (damage != null)
        {
            targetDamage.enabled = true;
            targetDamage.text = damage;
        }
    }

    public void DisableTargetIndicator()
    {
        targetIndicator.enabled = false;
        targetDamage.enabled = false;
    }
}
