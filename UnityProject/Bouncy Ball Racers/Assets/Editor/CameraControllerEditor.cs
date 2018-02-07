using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CameraController))]
public class CameraControllerEditor : Editor {

	private static float m_scale = 6.0f;

	public override void  OnInspectorGUI()
	{
		CameraController myTarget = (CameraController)target;

		EditorGUILayout.PropertyField(serializedObject.FindProperty("PlayerTransform"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("Displacement"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("FocalPoint"));

		myTarget.elasticity = m_scale = EditorGUILayout.Slider("Elasticity", m_scale, 1.0f, 20.0f);
		//myTarget.cameraHeightClamp = EditorGUILayout.Slider("Camera Vertical Displacement", 5.0f, 1.0f, 15.0f)

	}
}
