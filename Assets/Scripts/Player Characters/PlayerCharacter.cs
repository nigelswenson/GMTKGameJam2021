using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Character")]
public class PlayerCharacter : ScriptableObject
{
    public string characterName;
    public int maxHp;
    public int currentHp;
    public Sprite art;
    public bool isalive = true;

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
    
    public void Damage(int amountDamaged)
    {
        currentHp -= amountDamaged;
        if (currentHp <= 0)
        {
            currentHp = 0;
            isalive = false;
        }
    }   
}
