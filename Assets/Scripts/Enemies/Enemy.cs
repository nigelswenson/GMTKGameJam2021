using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public int hp;
    public int attack;
    public Sprite art;
    public bool isalive = true;

    public Text namePlate;
    public Slider hpBar;
    public Image portrait;

    public PlayerCharacter enemy;


    public bool Damage(int amountDamaged)
    {
        enemy.currentHp -= amountDamaged;
        if (enemy.currentHp <= 0)
        {
            isalive = true;
            return isalive;
        }
        else
        {
            isalive = false;
            return isalive;
        }
    }

    void Start()
    {
        portrait.sprite = art;
        namePlate.text = enemyName;

    }
}
