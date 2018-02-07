using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PlayerController))]
[CanEditMultipleObjects]
public class PlayerControllerEditor : Editor
{
	private static float m_bounciness = 0.6f;
	private static float m_maxBounceGain = 2.0f;
	private static float m_maxBounceHeight = 50.0f;
	private static float m_verticalSpeed = 17.0f;
	private static float m_horizontalSpeed = 5.0f;
	private static float m_rotationSpeed = 1.0f;
	private static float m_maxSpeed = 60.0f;

    public override void OnInspectorGUI()
    {
        PlayerController myTarget = (PlayerController)target;

		EditorGUILayout.PropertyField(serializedObject.FindProperty("Collider"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("CurrentAcceleration"));

		myTarget.Bounciness = m_bounciness = EditorGUILayout.Slider("Bounciness", m_bounciness, 0.0f, 1.0f);
		myTarget.MaxBounceGain = m_maxBounceGain = EditorGUILayout.Slider("Bounce Gain", m_maxBounceGain, 0.0f, 5.0f);
		myTarget.MaxBounceHeight = m_maxBounceHeight = EditorGUILayout.Slider("Maximum Bounce Height", m_maxBounceHeight, 0.0f, 100.0f);
		myTarget.VerticalSpeed = m_verticalSpeed = EditorGUILayout.Slider("Vertical Speed", m_verticalSpeed, 0.0f, 50.0f);
		myTarget.HorizontalSpeed = m_horizontalSpeed = EditorGUILayout.Slider("Horizontal Speed", m_horizontalSpeed, 0.0f, 50.0f);
		myTarget.RotationSpeed = m_rotationSpeed = EditorGUILayout.Slider("Rotation Speed", m_rotationSpeed, 0.0f, 10.0f);
		myTarget.MaxSpeed = m_maxSpeed = EditorGUILayout.Slider("Maximum Speed", m_maxSpeed, 0.0f, 200.0f);

    }
}