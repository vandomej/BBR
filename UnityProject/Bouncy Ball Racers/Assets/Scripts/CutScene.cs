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

    // The vertical portion of the screen that the dialog boxes will take up.
    public float DialogVerticalArea = 0.382f;

    // The horizontal padding for the dialog boxes
    public float DialogHorizontalPadding = 20.0f;

    // The vertical padding for the dialog boxes
    public float DialogVerticalPadding = 5.0f;

    // Keeps track of which panel is currently displayed
    private int panelIndex = 0;

    // Use this for initialization 
    void Start()
    {
        panelIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0))
        {
            panelIndex++;
        }

        if (panelIndex >= Panels.Count)
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
