using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableHolder : MonoBehaviour
{
    public int lastSceneIndex;

    // Start is called before the first frame update
    void Awake()
    {
        int variableHolderCount = FindObjectsOfType<VariableHolder>().Length;
        if (variableHolderCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
