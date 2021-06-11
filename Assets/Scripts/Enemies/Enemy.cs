using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public int attack;
    public int maxHp = 50; // changed from Hp, will need to be checked in other places, can be reverted
    public int currentHp = 50;
    public int armor = 0;
    public int armorDecay = 5;
    public int bleedDecay = 1;
    public int bleed = 0;
    public string target;
    public Sprite art;
    public PlayerCharacter enemy;



    // Change Enemy Variables
    //public void Attack()
    //{
    //    enemy.currentHp -= attack;
    //}


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
        {
            currentHp -= amountDamage - armor;
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
    }

    public void ChangeTarget(string character)
    {
        target = character;
    }

    public void EndOfTurn()
    {
        armor -= armorDecay; // armor goes down every turn
        currentHp -= bleed; // bleed affects inside armor
        bleed -= bleedDecay; // bleed decays after damage
    }


    void Start()
    {
        portrait.sprite = art;
    }
}