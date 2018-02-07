using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CutScene : MonoBehaviour {

	public string nextScene;

	public CutScenePanel[] images;

	private bool isTextAnimating = false;

	// Use this for initialization
	void Start () {
		isTextAnimating = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.S))
        {
			StartCoroutine(LoadSceneAsync());
		}
	}

	IEnumerator LoadSceneAsync() {

        AsyncOperation asyncScene = SceneManager.LoadSceneAsync(nextScene);

		while (!asyncScene.isDone) {
			yield return null;
		}
	}
}
