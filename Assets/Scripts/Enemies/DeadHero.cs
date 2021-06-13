using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
Dead Hero Behavior
Starts out asleep for x turns w/ armor
    wakes up if armor breaks or end of turns
    attacks 2 times per turn
    only attacks every other turn

*/
public class DeadHero : Enemy
{
    public int attack = 8;
    public int selfharm = 4;
    public int diceRoll = 1;
    public float scalar = 1;
    public string action1 = "fallasleep";
    public string action2;
    public int sleepTimer = 0;
    public bool tired = false;
    public bool cranky = true;
    public bool logical = true;

    override public void SetBehavior()
    {
        if (action1 == "fallasleep")
        {
            armor += 15;
        }
        else if (action1 == "sleep") // still asleep
        {
            if (sleepTimer > 0) // OR IF ARMOR == 0, needs to be implemented, needed?
            {
                sleepTimer -= 1;
                action1 = "sleep";
            }
            else
            {
                cranky = true;
                tired = false;
            }
        }
        else // Has woken up and is displeased
        {
            if (tired == false)
            {
                if (currentHp > maxHp / 6) // really low, so aggressive attack
                {
                    cranky = true;
                }
                diceRoll = Random.Range(1, 20);
                if (diceRoll > 1 && diceRoll < 5) // low chance for aggressive attack
                {
                    cranky = true;
                }
                else if (diceRoll == 1) // low chance to fall back asleep and get lots of armor
                {
                    action1 = "fallasleep";
                }
                else if (diceRoll > 4 && diceRoll < 11) // mid chance for bleed and attack
                {
                    action1 = "bleed";
                    action2 = "attack";
                }
                else if (diceRoll > 10 && diceRoll < 16) // mid chance for aoe attack and shield
                {
                    action1 = "attackall";
                    action2 = "armor";
                }
                else // mid chance for bleed and shield
                {
                    action1 = "bleedall";
                    action2 = "armor";
                }
                if (cranky == true) // Really aggressive attack, when first woken up & when low 
                {
                    action1 = "attackall";
                    action1 = "bleedall";
                    cranky = false;
                }
            }
            else
            {
                action1 = "rest";
            }
            if (Random.Range(1, 5) != 4) // Choose to either target lowest or target random
            {
                TargetLowest();
            }
            else
            {
                TargetRandom();
            }
            Debug.Log("Undead Hero is about to" + action1 + "and " + action2);

        }

    }


    override public void DoBehavior()
    {
        if (action1 == "fallasleep")
        {
            sleepTimer = 2;
            action1 = "sleep";
        }
        else if (action1 == "sleep")
        { }
        else
        {
            if (action1 == "attackall" || action2 == "attackall")
            {
                AttackAll(10);
            }
            if (action1 == "bleedall" || action2 == "bleedall")
            {
                BleedAll(3);
            }
            if (action1 == "armor" || action2 == "armor")
            {
                armor += 5;
            }
            if (action1 == "bleed" || action2 == "bleed")
            {
                target.Bleed(3);
            }
            if (action1 == "attack" || action2 == "attack")
            {
                target.TakeDamage(10);
            }
        }

    }
}