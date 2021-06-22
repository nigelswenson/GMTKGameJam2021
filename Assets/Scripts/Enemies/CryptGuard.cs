using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
Crypt Guard
    Always has base armor every turn
    Very light armor decay (less than he gains)
    Does shit damage, but scales with armor

*/
public class CryptGuard : Enemy
{
    public int attack = 2;
    public int shield = 4;
    public int shieldScaling = 5;
    public int startHealth = 100;
    public string action;
    int damage;


    override public void SetBehavior()
    {
        base.SetBehavior();
            damage = attack + (int)((float)armor * shieldScaling / 10);
            armor += shield;
            SetArmor();
            TargetRandom();
            action = "attack";
            FindObjectOfType<BattleManager>().EnableTargetIndicator(target, damage.ToString());
    }


    override public void DoBehavior()
    {
        if(currentHp > 0)
        {
            if (action == "attack")
            {
                target.TakeDamage(damage);
                FindObjectOfType<BattleManager>().sfx.PlayDamage();
                battleManager.ShowBattleText(enemyName + " dealt " + damage + " damage to " + target.characterName);
                battleManager.SetBlink(attackColor);
            }
        }
        base.DoBehavior();
    }

    //   void Start()
    //   {
    //       Bleed(startBleed);
    //       Armor(startArmor);
    //       SetMaxHp(startHealth);
    //       Heal(startHealth);
    //   }
}