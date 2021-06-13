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
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }

    }
}
