using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private int numScenes;

    public void LoadNextScene()
    {

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex < numScenes)
        {
            SetLastIndex();
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            SetLastIndex();
            SceneManager.LoadScene(0);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }    

    public void LoadSpecificScene(string sceneName)
    {
        SetLastIndex();
        SceneManager.LoadScene(sceneName);
    }

    public void LoadLastScene()
    {
        SceneManager.LoadScene(FindObjectOfType<VariableHolder>().lastSceneIndex);
    }

    private void SetLastIndex()
    {
        FindObjectOfType<VariableHolder>().lastSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }    


}
