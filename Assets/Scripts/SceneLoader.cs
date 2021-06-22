using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private int numScenes;

    [SerializeField] private Image UIFade;
    [SerializeField] private Animator anim;


    public void LoadNextScene()
    {

        StartCoroutine(Fading());
    }

    public void FastLoadNextScene()
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

    private IEnumerator Fading()
    {
        anim.SetBool("fade", true);
        yield return new WaitUntil(() => UIFade.color.a == 1);

        FastLoadNextScene();
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
