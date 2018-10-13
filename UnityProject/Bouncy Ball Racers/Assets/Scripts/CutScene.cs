using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

// Class used to handle the behavior of cutscenes, stores all of the cutscene data and then uses that data to properly
// display the cutscene.
public class CutScene : MonoBehaviour
{

    // The scene to go to after the cutscene has finished
    public string NextScene;

    // The individual cutscene panels that contains information that is to be displayed on the panel.
    public List<CutScenePanel> Panels = new List<CutScenePanel>();

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

        AsyncOperation asyncScene = SceneManager.LoadSceneAsync(NextScene);

        while (!asyncScene.isDone)
        {
            yield return null;
        }
    }
}
