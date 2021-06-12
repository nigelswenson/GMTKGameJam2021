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

public void Heal(int amountHealed)
    {
        currentHp += amountHealed;
        if (currentHp >= maxHp)
        {
            currentHp = maxHp;
        }
        UpdateHp();
    }

    public void Armor(int amountShielded)
    {
        armor += amountShielded;
    }
    public void Bleed(int amountBleed)
    {
        bleed = amountBleed;
    }
    public void TakeDamage(int amountDamage)
    {
        if (amountDamage - armor >= 0)
        {
            currentHp -= amountDamage - armor;
            UpdateHp();
            armor = 0;
        }
        else
        {
            armor -= amountDamage;
        }
    }
    public void TakePenDamage(int amountDamage)
    {
        currentHp -= amountDamage;
        UpdateHp();
    }
    public void EndTurn()
    {
        armor -= armorDecay; // armor goes down every turn
        if (armor < 0)
        {
            armor = 0;
        }
        currentHp -= bleed; // bleed affects inside armor
        UpdateHp();
        bleed -= bleedDecay; // bleed decays after damage
        if (bleed < 0)
        {
            bleed = 0;
        }
        actions = 1;
    }
    public void ActionAdd(int actionAdd)
    {
        actions += actionAdd;
    }
}