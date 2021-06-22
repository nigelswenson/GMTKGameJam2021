﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public int maxHp; // changed from Hp, will need to be checked in other places, can be reverted
    public int currentHp;
    public int armor = 0;
    public int armorDecay = 5;
    public int bleedDecay = 1;
    public int startBleed = 0;
    public int startArmor = 0;
    public int bleed = 0;
    public PlayerCharacter target;
    public Sprite art;
    public Image portrait;
    public int wiggleSpeed = 1;
    public Color32 attackColor = new Color32 (255, 59, 82, 255);
    public Color32 healColor = new Color32(118, 231, 114, 255);

    [HideInInspector]
    public bool isAlive = true;

    //cached reference
    public BattleManager battleManager;


    private void Start()
    {
        portrait.sprite = art;
        SetHp();
        SetBleed(bleed);
        SetArmor();
    }

    //Condition indicators
    public void SetBleed(int bleed)
    {
        FindObjectOfType<BattleManager>().SetEnemyBleed(bleed);
    }

    public void SetArmor()
    {
        FindObjectOfType<BattleManager>().SetEnemyArmor();
    }

    // Set armor decay value (could be nice to make this for updating all variables if we're bored)
    public void SetArmorDecay(int decay)
    {
        armorDecay = decay;
    }

    public void SetMaxHp(int maxHealth)
    {
        maxHp = maxHealth;
    }

    // Target Lowest Healthbar
    public void TargetLowest()
    {
        int lowHealth = 100000;
        foreach (PlayerCharacter partyMember in FindObjectOfType<BattleManager>().party)
        {
            if (partyMember.currentHp < lowHealth && partyMember.currentHp > 0)
            {
                lowHealth = partyMember.currentHp;
                target = partyMember;
            }
        }
    }

    // Target Random Member
    public void TargetRandom()
    {
        var targetList = FindObjectOfType<BattleManager>().party;
        foreach (PlayerCharacter target in targetList)
        {
            if (target.currentHp <=0)
            {
                targetList.Remove(target);
            }
        }
            target = FindObjectOfType<BattleManager>().party[Random.Range(1, 3)];
    }

    // Attack All Members
    public void AttackAll(int damage)
    {
        foreach (PlayerCharacter partyMember in FindObjectOfType<BattleManager>().party)
        {
            partyMember.TakeDamage(damage);
        }
    }

    // Bleed All Members
    public void BleedAll(int bleed)
    {
        foreach (PlayerCharacter partyMember in FindObjectOfType<BattleManager>().party)
        {
            partyMember.Bleed(bleed);
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
        SetArmor();
    }
    // Set bleed counter for amount given
    public void Bleed(int amountBleed)
    {
        bleed += amountBleed;
        SetBleed(bleed);
    }

    // Take damage for amount given
    public void TakeDamage(int amountDamage)
    {
        if (amountDamage - armor >= 0)
        {
            currentHp -= amountDamage - armor;
            SetHp();
            armor = 0;
            SetArmor();
        }
        else
        {
            armor -= amountDamage;
            SetArmor();
        }
        if (currentHp <= 0)
        {
            FindObjectOfType<BattleManager>().EnemyDeath();
        }
    }
    // Take premitigation damage for amount given
    public void TakePenDamage(int amountDamage)
    {
        currentHp -= amountDamage;
        SetHp();
    }

    // Change target to one given, currently won't work because it happens
    // before enemy's turn and will be overwritten
    public void ChangeTarget(PlayerCharacter character)
    {
        target = character;
    }

    public virtual void SetBehavior()
    {
        battleManager = FindObjectOfType<BattleManager>();
    }

    public virtual void DoBehavior()
    {
        battleManager.DisableTargetIndicator();
        battleManager.DisableHealImage();
    }

    // Update numbers at the end of enemy's turn
    public void Upkeep()
    {
        armor -= armorDecay; // armor goes down every turn
        if (armor < 0)
        {
            armor = 0;
        }
        SetArmor();
        currentHp -= bleed; // bleed affects inside armor
        if (currentHp <= 0)
        {
            FindObjectOfType<BattleManager>().EnemyDeath();
        }
        SetHp();
        bleed -= bleedDecay; // bleed decays after damage
        if (bleed < 0)
        {
            bleed = 0;
        }
        SetBleed(bleed);
    }

    public void SetHp()
    {
        FindObjectOfType<BattleManager>().SetEnemyHp();
    }

    public virtual void EnemySetup()
    {
        currentHp = maxHp;
        bleed = startBleed;
        armor = startArmor;
        SetArmor();
        SetBleed(bleed);
        SetHp();
    }

    /*private void Die()
    {
        Debug.Log("Died");
        isAlive = false;
        portrait.enabled = false;
        yield return new WaitForSeconds(1);

        FindObjectOfType<SceneLoader>().LoadNextScene();

        //change enemy in battleManager
    } */   
}