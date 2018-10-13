using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(CutScene))]
[CanEditMultipleObjects]
public class CutSceneEditor : Editor
{

    private SerializedProperty nextScene;

    void OnEnable()
    {
        nextScene = serializedObject.FindProperty("nextScene");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CutScene myTarget = (CutScene)target;
        // myTarget.panels = new List<CutScenePanel>();

        EditorGUILayout.PropertyField(nextScene);
        foreach (var panel in myTarget.panels)
        {
            SerializedObject obj = new SerializedObject(panel);
            EditorGUILayout.PropertyField(obj.FindProperty("Background"));
            EditorGUILayout.PropertyField(obj.FindProperty("HasDialogue"));
            EditorGUILayout.PropertyField(obj.FindProperty("CharacterName"));
            EditorGUILayout.PropertyField(obj.FindProperty("CharacterPortrait"));
            EditorGUILayout.PropertyField(obj.FindProperty("Dialogue"));
            EditorGUILayout.PropertyField(obj.FindProperty("WaitDuration"));
            EditorGUILayout.PropertyField(obj.FindProperty("SoundEffect"));
            EditorGUILayout.PropertyField(obj.FindProperty("Music"));

            foreach (var image in panel.Images)
            {
                Debug.Log(image);
                SerializedObject imageObject = new SerializedObject(image);
                EditorGUILayout.PropertyField(imageObject.FindProperty("Image"));
                EditorGUILayout.PropertyField(imageObject.FindProperty("ZIndex"));
                EditorGUILayout.PropertyField(imageObject.FindProperty("Position"));
                EditorGUILayout.PropertyField(imageObject.FindProperty("Animation"));
            }
        }

        if (GUILayout.Button("Add Panel"))
        {
            myTarget.panels.Add(ScriptableObject.CreateInstance<CutScenePanel>());
        }

        serializedObject.ApplyModifiedProperties();

    }
}
