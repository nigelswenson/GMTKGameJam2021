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
public class Guard : Enemy
{
    public int attack = 2;
    public int startArmor = 15;
    public int shield = 4;
    public int shieldScaling = 5;
    public int shieldDecay = 2;
    public int startBleed = 5;
    public int startHealth = 100;
    public string action;


    override public void SetBehavior()
    {
        SetArmorDecay(shieldDecay);
        armor += shield;
        TargetLowest();
        action = "attack";
    }


    override public void DoBehavior()
    {
        if (action == "attack")
        {
            target.TakeDamage(attack + (int)((float)armor * shieldScaling / 10));
        }
    }

    //   void Start()
    //   {
    //       Bleed(startBleed);
    //       Armor(startArmor);
    //       SetMaxHp(startHealth);
    //       Heal(startHealth);
    //   }
}