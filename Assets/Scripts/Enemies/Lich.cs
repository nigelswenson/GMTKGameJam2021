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
    public int damage;

    override public void SetBehavior()
    {
        base.SetBehavior();
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
                damage = (int)((float)attack * scalar);
                battleManager.EnableTargetIndicator(null, damage.ToString());
            }
            else
            {
                action = "attack";
                damage = 2 * (int)((float)attack * scalar);
                battleManager.EnableTargetIndicator(target, damage.ToString());
            }

        }
    }

    override public void DoBehavior()
    {
        if (action == "attackall")
        {
            AttackAll(damage);
            FindObjectOfType<BattleManager>().sfx.PlayDamage();
            battleManager.ShowBattleText(enemyName + " dealt " + damage + " damage to all allies");
        }
        else if (action == "attack")
        {
            target.TakeDamage(damage);
            FindObjectOfType<BattleManager>().sfx.PlayDamage();
            battleManager.ShowBattleText(enemyName + " dealt " + damage + " damage to " + target.characterName);
        }
        currentHp -= (int)((float)selfharm * scalar);
        
        base.DoBehavior();
    }
}


/*
Floating Sword and shield? (probably too similar to characters)
Mimic? 
Always attacks W/ 2 unique attacks (Vulnerable)
Takes a break after every turn cause hes a tired boi (tough)
*/