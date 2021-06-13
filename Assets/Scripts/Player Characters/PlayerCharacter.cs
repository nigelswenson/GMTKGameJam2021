using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Character")]
public class PlayerCharacter : ScriptableObject
{
    public string characterName;
    public int maxHp = 50;
    public int currentHp = 50;
    public int armor = 0;
    public int armorDecay = 5;
    public int bleedDecay = 1;
    public int bleed = 0;
    public int actions = 10;
    public Sprite art;
    public int actionsRemaining = 0;
    public GameObject playerArea;
    public Color cardColor;

    public List<Card> deckData = new List<Card>();
    [HideInInspector]
    public List<GameObject> deck = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> discardPile = new List<GameObject>();

    private void UpdateHp()
    {
        var displays = FindObjectsOfType<CharacterDisplay>();
        foreach (CharacterDisplay character in displays)
        {
            character.SetHp();
        }
    }

    private void UpdateBleed()
        {
        var displays = FindObjectsOfType<CharacterDisplay>();
        foreach (CharacterDisplay display in displays)
        {
            display.SetBleed(display.character.bleed);
        }
    }

    private void UpdateArmor()
    {
        var displays = FindObjectsOfType<CharacterDisplay>();
        foreach (CharacterDisplay display in displays)
        {
            display.SetArmor(display.character.armor);
        }
    }

    public void Heal(int amountHealed)
    {
        currentHp += amountHealed;
        if (currentHp >= maxHp)
        {
            currentHp = maxHp;
        }
        else if (currentHp <= 0)
        {
            currentHp = 0
        }
        UpdateHp();
    }

    public void Armor(int amountShielded)
    {
        armor += amountShielded;
        UpdateArmor();
    }
    public void Bleed(int amountBleed)
    {
        bleed = amountBleed;
        UpdateBleed();
    }
    public void TakeDamage(int amountDamage)
    {
        if (amountDamage - armor >= 0)
        {
            currentHp -= amountDamage - armor;
            if (currentHp <= 0)
            {
                currentHp = 0;
                
            }
            UpdateHp();
            armor = 0;
        }
        else
        {
            armor -= amountDamage;
        }
        UpdateArmor();
    }
    public void TakePenDamage(int amountDamage)
    {
        currentHp -= amountDamage;
        if (currentHp <= 0)
        {
            currentHp = 0;
            UpdateHp();
        }
    }
    public void EndTurn()
    {
        armor -= armorDecay; // armor goes down every turn
        if (armor < 0)
        {
            armor = 0;
            UpdateArmor();
        }
        TakePenDamage(bleed); // bleed affects inside armor
        UpdateHp();
        bleed -= bleedDecay; // bleed decays after damage
        if (bleed < 0)
        {
            bleed = 0;
        }
        UpdateBleed();
        actions = 1;
    }
    public void ActionAdd(int actionAdd)
    {
        actions += actionAdd;
    }
}