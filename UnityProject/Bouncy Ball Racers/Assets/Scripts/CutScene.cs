using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CutScene : MonoBehaviour
{

    public string nextScene;

    public List<CutScenePanel> panels = new List<CutScenePanel>();

    // Use this for initialization 
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (false)
        {
            StartCoroutine(LoadSceneAsync());
        }
    }

    IEnumerator LoadSceneAsync()
    {

        AsyncOperation asyncScene = SceneManager.LoadSceneAsync(nextScene);

        while (!asyncScene.isDone)
        {
            yield return null;
        }
    }
}
