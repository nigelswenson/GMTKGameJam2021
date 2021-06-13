using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
Lich Behavior
    attacks and takes self damage each turn
    either aoe or st if someone is low
    damage and self damage increases when low
OR
Vampire Behavior
    Always attacks and heals
    bleed cuts affects of heals in 2
    Heals increase when low
*/
public class Lich : Enemy
{
    public int attack = 8;
    public int selfharm = 4;
    public float scalar = 1;
    public string action;

    override public void SetBehavior()
    {
        if (currentHp > maxHp / 2)
        {
            scalar = 1;
        }
        else // Is inraged/desperate so deals more damage and hurts itself more (change healthbar color?)
        {
            scalar = 1.5F;
        }

        if (Random.Range(1, 5) != 4)
        {
            TargetLowest();
            if (target.currentHp > target.maxHp / 2)
            {
                action = "attackall";
            }
            else
            {
                action = "attack";
            }

        }
    }

    override public void DoBehavior()
    {
        if (action == "attackall")
        {
            AttackAll((int)((float)attack * scalar));
        }
        else if (action == "attack")
        {
            target.TakeDamage(2 * (int)((float)attack * scalar));
        }
        currentHp -= (int)((float)selfharm * scalar);
    }
}


/*
Floating Sword and shield? (probably too similar to characters)
Mimic? 
Always attacks W/ 2 unique attacks (Vulnerable)
Takes a break after every turn cause hes a tired boi (tough)
*/