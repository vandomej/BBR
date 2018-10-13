﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

// Editor class respective to the CutScene behavioral class. Handles the data hierarchy in the editor, as well as the
// modification of the data by the user.
[CustomEditor(typeof(CutScene))]
[CanEditMultipleObjects]
public class CutSceneEditor : Editor
{

    // The serialized property of the next scene the user wants to go to after the cutscene.
    private SerializedProperty NextScene;

    // When the script is first enabled, the class does some set up for some properties.
    void OnEnable()
    {
        NextScene = serializedObject.FindProperty("NextScene");
    }

    // Called everytime that the editor is redisplayed
    public override void OnInspectorGUI()
    {
        // Refreshing the values on the object
        serializedObject.Update();

        // Obtaining the object that the editor is attached to.
        CutScene myTarget = (CutScene)target;

        // Displaying a field so that the user can specify the next scene
        EditorGUILayout.PropertyField(NextScene);

        // Starting a loop for all of the panels currently on the object.
        for (int i = 0; i < myTarget.Panels.Count; i++)
        {
            EditorGUILayout.Space();

            if (GUILayout.Button("Remove Panel"))
            {
                myTarget.Panels.RemoveAt(i);
                break;
            }

            var panel = myTarget.Panels[i];
            SerializedObject obj = new SerializedObject(panel);
            obj.Update();

            EditorGUILayout.PropertyField(obj.FindProperty("CharacterName"));
            EditorGUILayout.PropertyField(obj.FindProperty("CharacterPortrait"));

            EditorGUILayout.PropertyField(obj.FindProperty("HasDialogue"));
            if (panel.HasDialogue)
            {
                EditorGUILayout.PropertyField(obj.FindProperty("Dialogue"));
            }
            else
            {
                EditorGUILayout.PropertyField(obj.FindProperty("WaitDuration"));
            }

            EditorGUILayout.PropertyField(obj.FindProperty("Background"));
            EditorGUILayout.PropertyField(obj.FindProperty("SoundEffect"));
            EditorGUILayout.PropertyField(obj.FindProperty("Music"));

            // Starting a loop for all of the images on the current panel.
            for (int j = 0; j < panel.Images.Count; j++)
            {
                EditorGUILayout.Space();

                if (GUILayout.Button("Remove Image"))
                {
                    panel.Images.RemoveAt(j);
                    break;
                }

                var image = panel.Images[j];
                SerializedObject imageObject = new SerializedObject(image);
                imageObject.Update();

                EditorGUILayout.PropertyField(imageObject.FindProperty("Image"));
                EditorGUILayout.PropertyField(imageObject.FindProperty("ZIndex"));
                EditorGUILayout.PropertyField(imageObject.FindProperty("Position"));
                EditorGUILayout.PropertyField(imageObject.FindProperty("Animation"));

                imageObject.ApplyModifiedProperties();
            }

            if (GUILayout.Button("Add Image"))
            {
                panel.Images.Add(ScriptableObject.CreateInstance<CutSceneImage>());
            }

            obj.ApplyModifiedProperties();
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Add Panel"))
        {
            myTarget.Panels.Add(ScriptableObject.CreateInstance<CutScenePanel>());
        }

        // Applying all of the changed properties that took place during the function.
        serializedObject.ApplyModifiedProperties();

    }
}
