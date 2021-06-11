using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Card")]
public class Card : ScriptableObject
{

    public string cardName;
    public string description;
    public Sprite art;

    public bool doesDraw = false;
    public int cardsToDraw = 0;

    public bool doesDamage = false;
    public int damageDealt = 0;

    public bool doesHeal = false;
    public int healingDone = 0;
}
