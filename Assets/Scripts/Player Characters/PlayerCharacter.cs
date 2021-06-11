using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Character")]
public class PlayerCharacter : ScriptableObject
{
    public string characterName;
    public int maxHp = 50;
    public int currentHp = maxHp;
    public int armor = 0;
    public int armorDecay = 5;
    public int bleedDecay = 1;
    public int bleed = 0;
    public Sprite art;
    public List<Card> deckData = new List<Card>();


    public List<Card> deckData = new List<Card>();
    [HideInInspector]
    public List<GameObject> deck = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> discardPile = new List<GameObject>();

    public void Heal(int amountHealed)
    {
        currentHp += amountHealed;
        if (currentHp >= maxHp)
        {
            currentHp = maxHp;
        }
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
            currentHp -= amountDamage - armor;
        armor = 0;
        else
            armor -= amountDamage;
    }
    public void TakePenDamage(int amountDamage)
    {
        currentHp -= amountDamage;
    }
    public void EndOfTurn()
    {
        armor -= armorDecay; // armor goes down every turn
        currentHp -= bleed; // bleed affects inside armor
        bleed -= bleedDecay; // bleed decays after damage
    }
}