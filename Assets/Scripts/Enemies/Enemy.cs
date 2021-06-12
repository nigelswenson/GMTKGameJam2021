﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public int maxHp; // changed from Hp, will need to be checked in other places, can be reverted
    public int currentHp = 50;
    public int armor = 0;
    public int armorDecay = 5;
    public int bleedDecay = 1;
    public int bleed = 0;
    public PlayerCharacter target;
    public Sprite art;
    public Image portrait;
    public PlayerCharacter enemy;
    //[SerializeField] List<PlayerCharacter> party = FindObjectOfType<BattleManager>().party;
    //PlayerCharacter party = FindObjectOfType<BattleManager>().party;
    public List<PlayerCharacter> party = new List<PlayerCharacter>();

    // Set armor decay value (could be nice to make this for updating all variables if we're bored)
    public void SetArmorDecay(int decay)
    {
        armorDecay = decay;
    }
    // Target Lowest Healthbar
    public void TargetLowest()
    {
        int lowHealth = 100000;
        foreach (PlayerCharacter partyMember in party)
        {
            if (partyMember.currentHp < lowHealth)
            {
                lowHealth = partyMember.currentHp;
                target = partyMember;
            }
        }
    }

    // Target Random Member
    public void TargetRandom()
    {
        target = party[Random.Range(1, 3)];
    }

    // Attack All Members
    public void AttackAll(int damage)
    {
        foreach (PlayerCharacter partyMember in party)
        {
            partyMember.TakeDamage(damage);
        }
    }

    // Heal self for amount given
    public void Heal(int amountHealed)
    {
        currentHp += amountHealed;
        if (currentHp >= maxHp)
        {
            currentHp = maxHp;
        }
    }

    // Shield self for amount given
    public void Armor(int amountShielded)
    {
        armor += amountShielded;
    }
    // Set bleed counter for amount given
    public void Bleed(int amountBleed)
    {
        bleed = amountBleed;
    }

    // Take damage for amount given
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
    // Take premitigation damage for amount given
    public void TakePenDamage(int amountDamage)
    {
        currentHp -= amountDamage;
    }

    // Change target to one given, currently won't work because it happens
    // before enemy's turn and will be overwritten
    public void ChangeTarget(PlayerCharacter character)
    {
        target = character;
    }

    // Update numbers at the end of enemy's turn
    public void EndTurn()
    {
        armor -= armorDecay; // armor goes down every turn
        if (armor < 0)
        {
            armor = 0;
        }
        currentHp -= bleed; // bleed affects inside armor
        bleed -= bleedDecay; // bleed decays after damage
        if (bleed < 0)
        {
            bleed = 0;
        }

    }


    void Start()
    {
        portrait.sprite = art;
    }
}