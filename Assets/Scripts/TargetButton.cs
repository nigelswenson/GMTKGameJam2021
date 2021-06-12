using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetButton : MonoBehaviour
{
    public string targetCharacterName;

    public void SetTarget()
    {
        FindObjectOfType<BattleManager>().SetTarget(targetCharacterName);
    }    
}
