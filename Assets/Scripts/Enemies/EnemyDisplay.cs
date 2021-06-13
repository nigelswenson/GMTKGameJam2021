using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDisplay : MonoBehaviour
{
    public Enemy enemy;

    public Text namePlate;
    public Image portrait;
    public Image bleedImage;
    public Text bleedCount;
    public Image armorImage;
    public Text armorCount;

    public void SetBleed(int bleed)
    {
        if (bleed >= 0)
        {
            bleedImage.enabled = false;
            bleedCount.enabled = false;
        }
        else
        {
            bleedCount.text = bleed.ToString();
            bleedImage.enabled = true;
            bleedCount.enabled = true;
        }
    }

    public void SetArmor(int armor)
    {
        Debug.Log("Set Armor");
        if (armor <= 0)
        {
            armorImage.enabled = false;
            armorCount.enabled = false;
        }
        else
        {
            armorCount.text = armor.ToString();
            armorImage.enabled = true;
            armorCount.enabled = true;
        }
    }
}
