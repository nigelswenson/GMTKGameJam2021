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
    public int selfharm = 4;
    public int diceRoll = 1;
    public float scalar = 1;
    public string action1 = "sleep";
    public string action2;
    public int sleepTimer = 1;
    public bool tired = false;
    public bool cranky = true;
    public int oneAttack = 12;
    public int allAttack = 6;
    public int oneBleed = 6;
    public int allBleed = 4;
    public int oneArmor = 10;
    public int sleepyShield = 40;

    int damage;


    override public void SetBehavior()
    {
        base.SetBehavior();
        if (action1 == "fallasleep" || action1 == "sleep")
        { }
        else // Has woken up and is displeased
        {
            if (Random.Range(1, 5) != 4) // Choose to either target lowest or target random
            {
                TargetLowest();
            }
            else
            {
                TargetRandom();
            }
            if (tired == false)
            {
                if (currentHp < maxHp / 6) // really low, so aggressive attack
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
                    battleManager.EnableTargetIndicator(target, oneAttack.ToString());
                }
                else if (diceRoll > 10 && diceRoll < 16) // mid chance for aoe attack and shield
                {
                    action1 = "attackall";
                    action2 = "armor";
                    battleManager.EnableTargetIndicator(null, allAttack.ToString());
                    FindObjectOfType<BattleManager>().EnableHealImage();
                }
                else // mid chance for bleed and shield
                {
                    action1 = "bleedall";
                    action2 = "armor";
                    battleManager.EnableTargetIndicator(null, null);
                    FindObjectOfType<BattleManager>().EnableHealImage();
                }
                if (cranky == true) // Really aggressive attack, when first woken up & when low 
                {
                    action1 = "attackall";
                    action2 = "bleedall";
                    cranky = false;

                    battleManager.EnableTargetIndicator(null, allAttack.ToString());
                }
                Debug.Log("Undead Hero is about to " + action1 + " and " + action2);
            }
            else
            {
                action1 = "rest";
                tired = false;
            }
        }
    }


    override public void DoBehavior()
    {
        if (action1 == "fallasleep")
        {
            armor += sleepyShield;
            sleepTimer = 1;
            action1 = "sleep";
            battleManager.ShowBattleText(enemyName + " gains " + sleepyShield + " armor and drifts to sleep");
        }
        else if (action1 == "sleep")
        {
            if (sleepTimer > 0)
            {
                sleepTimer -= 1;
                battleManager.ShowBattleText(enemyName + " rests solemnly");
            }
            else
            {
                battleManager.ShowBattleText(enemyName + " suddenly awakens");
                cranky = true;
                tired = false;
                action1 = "awake";
            }
        }

        else if (tired == true)
        {
            tired = true;
        }
        else
        {
            var string1 = "";
            var string2 = "";
            if (action1 == "attackall" || action2 == "attackall")
            {
                AttackAll(allAttack);
                FindObjectOfType<BattleManager>().sfx.PlayDamage();
                string1 = (enemyName + " dealt " + allAttack + " damage to all allies");
                battleManager.SetBlink(attackColor);

            }
            if (action1 == "bleedall" || action2 == "bleedall")
            {
                BleedAll(allBleed);
                FindObjectOfType<BattleManager>().sfx.PlayBleed();

                
                if(string1 == "")
                {
                    string1 = (enemyName + " applied " + allBleed + " bleed to all allies");
                }
                else
                {
                    string2 = ("applied " + allBleed + " bleed to all allies");
                }

                battleManager.SetBlink(attackColor);

            }
            if (action1 == "armor" || action2 == "armor")
            {
                armor += oneArmor;
                FindObjectOfType<BattleManager>().sfx.PlayArmor();

                if (string1 == "")
                {
                    string1 = (enemyName + " gained " + oneArmor + " armor");
                }
                else
                {
                    string2 = ("gained " + oneArmor + " armor");
                }

                battleManager.SetBlink(healColor);
            }
            if (action1 == "bleed" || action2 == "bleed")
            {
                target.Bleed(oneBleed);
                FindObjectOfType<BattleManager>().sfx.PlayBleed();
                battleManager.ShowBattleText(enemyName + " dealt " + oneBleed + " to " + target.characterName);

                if (string1 == "")
                {
                    string1 = (enemyName + " dealt " + oneBleed + " to " + target.characterName);
                }
                else
                {
                    string2 = ("dealt " + oneBleed + " to " + target.characterName);
                }
                battleManager.SetBlink(attackColor);
            }
            if (action1 == "attack" || action2 == "attack")
            {
                target.TakeDamage(oneAttack);
                FindObjectOfType<BattleManager>().sfx.PlayDamage();
                battleManager.ShowBattleText(enemyName + " dealt " + oneAttack + " damage to " + target.characterName);

                if (string1 == "")
                {
                    string1 = (enemyName + " dealt " + oneAttack + " damage to " + target.characterName);
                }
                else
                {
                    string2 = ("dealt " + oneAttack + " damage to " + target.characterName);
                }
                battleManager.SetBlink(attackColor);
            }
            if(string1 != "" && string2 != "")
            {
                battleManager.ShowBattleText(string1 + " and " + string2);
            }
            else
            {
                battleManager.ShowBattleText(string1);
            }

        }
        base.DoBehavior();
    }

    public override void EnemySetup()
    {
        base.EnemySetup();
        action1 = "asleep";
        cranky = true;
        tired = false;
    }
}