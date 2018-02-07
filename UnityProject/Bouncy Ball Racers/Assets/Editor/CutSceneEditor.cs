using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CutScene))]
public class CutSceneEditor : Editor {

	public override void OnInspectorGUI()
	{
		CutScene myTarget = (CutScene)target;
	}
}
