using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Card")]
public class Card : ScriptableObject
{

    public string cardName;
    public string description;
    public Sprite art;
    public List<string> methodList = new List<string>();
    public bool isTargeted = false;

    public int cardsToDraw = 0;

    public int damageDealt = 0;

    public int healingDone = 0;

    public int armorAdded = 0;
    
    public int bleedAdded = 0;
    
    public int actionAdded = 0;
    
    public string target = "";
}
