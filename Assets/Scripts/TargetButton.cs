using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetButton : MonoBehaviour
{
    public string targetCharacterName;

    public void SetTarget()
    {
        Debug.Log("onclick working");
        FindObjectOfType<BattleManager>().SetTarget(targetCharacterName);
    }    
}
