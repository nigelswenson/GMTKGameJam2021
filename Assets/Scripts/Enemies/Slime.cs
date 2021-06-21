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
    public int baseArmor = 5;
    public int armorSize;
    public int baseHeal = 5;
    public int healSize;
    public int baseAttack = 5;
    public int attack;
    public int randomValue = 2;
    public string action;

    override public void SetBehavior()
    {
        base.SetBehavior();
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
                attack = Random.Range(baseAttack - randomValue, baseAttack + randomValue);
                if (Random.Range(1, 5) != 4)
                {
                    TargetLowest(); // functions in Enemy Class
                    battleManager.EnableTargetIndicator(target, attack.ToString());
                }
                else
                {
                    TargetRandom();
                    battleManager.EnableTargetIndicator(target, attack.ToString());
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
        if (currentHp > 0)
        {
            if (action == "attack")
            {
                target.TakeDamage(attack);
                battleManager.ShowBattleText(enemyName + " dealt " + attack + " damage to " + target.characterName);
                battleManager.sfx.PlayDamage();
                Debug.Log(gameObject);
                battleManager.SetBlink(attackColor);
            }
            else if (action == "heal")
            {
                currentHp += healSize;
                SetHp();
                battleManager.ShowBattleText(enemyName + " healed " + healSize + " damage");
                battleManager.EnableHealImage();
                battleManager.sfx.PlayHeal();
                battleManager.SetBlink(healColor);
            }
            else if (action == "shield")
            {
                Armor(armorSize);
                battleManager.ShowBattleText(enemyName + " gained " + armorSize + " armor");
                battleManager.EnableHealImage();
                battleManager.sfx.PlayArmor();
                battleManager.SetBlink(healColor);
            }
            else // grow
            {
                size += growSize;
            }
        }

        base.DoBehavior();
    }
}