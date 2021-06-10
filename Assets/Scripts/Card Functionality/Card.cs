﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Card")]
public class Card : ScriptableObject
{

    public string cardName;
    public string description;
    public Sprite art;

    public int damage;
    public int healing;
    public int damageModifier;
}
