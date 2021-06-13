using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// This script should choose from a collection of actions below
// and affect the board through functions in PlayerCharacter.cs



public class Slime : Enemy
{
    public int size = 10;
    public int growRate = 1;
    public int growSize = 5;
    public int armorSize = 5;
    public int healSize = 5;
    public int attack = 5;
    public string action;


    override public void SetBehavior()
    {


        size += growRate; // grow every turn
        // scale slime pixel size here
        if (currentHp > maxHp / 2) // attack or grow
        {
            if (size < 5) // grow
            {
                action = "grow";
            }
            else // attack 
            {
                if (Random.Range(1, 5) != 4)
                {
                    TargetLowest(); // functions in Enemy Class
                }
                else
                {
                    TargetRandom();
                }
                action = "attack";
            }

        }
        else  // Shield or heal 
        {
            if (size < 5) // Shield
            {
                action = "shield";
                FindObjectOfType<BattleManager>().EnableHealImage();
            }
            else // Heal
            {
                action = "heal";
                FindObjectOfType<BattleManager>().EnableHealImage();
            }
        }

        Debug.Log("Slime is about to" + action);
    }


    override public void DoBehavior()
    {
        if (action == "attack")
        {
            target.TakeDamage(attack + (size / 10));
            FindObjectOfType<BattleManager>().sfx.PlayDamage();
        }
        else if (action == "heal")
        {
            currentHp += healSize + (size / 10);
            SetHp();
            FindObjectOfType<BattleManager>().sfx.PlayHeal();
        }
        else if (action == "shield")
        {
            armor = armorSize + (size / 10);
            FindObjectOfType<BattleManager>().sfx.PlayArmor();
        }
        else // grow
        {
            size += growSize;
        }
    }
}