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
    public int shield = 4;
    public int shieldScaling = 5;
    public int shieldDecay = 2;


    public void Behavior()
    {
        SetArmorDecay(shieldDecay);
        armor += shield;
        TargetLowest();
        target.TakeDamage(attack + (int)((float)armor * shieldScaling/10));
    }
}


