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


    public void Behavior()
    {
        size += growRate; // grow every turn
        // scale slime pixel size here

        if (currentHp > maxHp / 2) // attack or grow
        {
            if (size < 5) // grow
            {
                size += growSize;
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
                target.TakeDamage(attack * size / 10);
            }

        }
        else  // Shield or heal 
        {
            if (size < 5) // Shield
            {
                armor = armorSize * size / 10;
            }
            else // Heal
            {
                currentHp += healSize * size / 10;
                SetHp();
            }
        }
    }
}