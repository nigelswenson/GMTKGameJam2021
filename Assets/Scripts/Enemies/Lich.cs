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

    public void Behavior()
    {
        if (Random.Range(1, 5) != 4)
        {
            if (currentHp > maxHp / 2)
            {
                TargetLowest();
                if (target.currentHp > target.maxHp / 2)
                {
                    AttackAll(attack);
                }
                else
                {
                    target.TakeDamage(attack * 2);
                }
                currentHp -= selfharm;
            }
            else // Is inraged/desperate so deals more damage and hurts itself more (change healthbar color?)
            {
                TargetLowest();
                if (target.currentHp > target.maxHp / 2)
                {
                    AttackAll((int)((float)attack * 1.5));
                }
                else
                {
                    target.TakeDamage(attack * 3);
                }
                currentHp -= (int)((float)selfharm * 1.5);
            }
        }
    }
}



/*
Floating Sword and shield? (probably too similar to characters)
Mimic? 
Always attacks W/ 2 unique attacks (Vulnerable)
Takes a break after every turn cause hes a tired boi (tough)
*/