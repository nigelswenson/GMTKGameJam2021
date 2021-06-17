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


    public override void Start()
    {
        base.Start();
        battleManager = FindObjectOfType<BattleManager>();
    }
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
            var damage = attack + (size / 10);
            target.TakeDamage(damage);
            battleManager.ShowBattleText(enemyName + " dealt " + damage + " damage to " + target.characterName);
            battleManager.sfx.PlayDamage();
        }
        else if (action == "heal")
        {
            var heal = healSize + (size / 10);
            currentHp += heal;
            SetHp();
            battleManager.ShowBattleText(enemyName + " healed " + heal + " damage");
            battleManager.sfx.PlayHeal();
        }
        else if (action == "shield")
        {
            var armorGain = armorSize + (size / 10);
            Armor(armorGain);
            battleManager.ShowBattleText(enemyName + " gained " + armorGain + " armor");
            battleManager.sfx.PlayArmor();
        }
        else // grow
        {
            size += growSize;
        }
    }
}