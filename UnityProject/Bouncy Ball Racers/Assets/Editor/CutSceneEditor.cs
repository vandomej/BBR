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
        EditorGUILayout.Space();

        // Starting a loop for all of the panels currently on the object.
        for (int i = 0; i < myTarget.Panels.Count; i++)
        {
            var panel = myTarget.Panels[i];
            SerializedObject obj = new SerializedObject(panel);
            obj.Update();

            var panelArea = EditorGUILayout.BeginVertical();
            GUI.Box(panelArea, "Panel");
            GUILayout.Space(EditorGUIUtility.singleLineHeight * 2);

            //Instead of background area, just create a tiny little scene preview box, that doesn't have to display
            //the dialogue, but at least displays the sprites. Also put it into its own function.

            //Background area
            var previewArea = EditorGUILayout.BeginVertical();
            GUILayout.Space(EditorGUIUtility.singleLineHeight * 8);

            Rect leftArea = new Rect(previewArea.x, previewArea.y, previewArea.width / 3.0f, previewArea.height);
            Rect rightArea = new Rect(previewArea.x + (previewArea.width * 0.39f),
            previewArea.y,
            previewArea.width * 0.6f,
            previewArea.height);

            GUI.Box(rightArea, string.Empty);

            EditorGUI.PrefixLabel(
                new Rect(leftArea.x,
                    leftArea.y + EditorGUIUtility.singleLineHeight,
                    leftArea.width,
                    EditorGUIUtility.singleLineHeight),
                new GUIContent("Background"));

            panel.Background = (Sprite)EditorGUI.ObjectField(
                new Rect(leftArea.x,
                    leftArea.y + (2.0f * EditorGUIUtility.singleLineHeight),
                    leftArea.width,
                    EditorGUIUtility.singleLineHeight),
                panel.Background,
                typeof(Sprite),
                !EditorUtility.IsPersistent(Selection.activeObject));

            if (panel.Background != null)
            {
                GUI.DrawTexture(rightArea, panel.Background.texture, ScaleMode.ScaleToFit);
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.PropertyField(obj.FindProperty("HasDialogue"));
            if (panel.HasDialogue)
            {
                EditorGUILayout.PropertyField(obj.FindProperty("CharacterName"));
                EditorGUILayout.PropertyField(obj.FindProperty("CharacterPortrait"));
                panel.Dialogue = EditorGUILayout.TextField("Dialogue",
                panel.Dialogue, GUILayout.Height(EditorGUIUtility.singleLineHeight * 4));
            }
            else
            {
                EditorGUILayout.PropertyField(obj.FindProperty("WaitDuration"));
            }


            EditorGUILayout.PropertyField(obj.FindProperty("SoundEffect"));
            EditorGUILayout.PropertyField(obj.FindProperty("Music"));
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            GUILayout.Space(15);

            EditorGUILayout.BeginVertical();

            // Starting a loop for all of the images on the current panel.
            for (int j = 0; j < panel.Images.Count; j++)
            {
                var image = panel.Images[j];
                SerializedObject imageObject = new SerializedObject(image);
                imageObject.Update();

                EditorGUILayout.Space();
                var imageArea = EditorGUILayout.BeginVertical();
                GUI.Box(imageArea, "Image");
                GUILayout.Space(EditorGUIUtility.singleLineHeight * 2);

                EditorGUILayout.PropertyField(imageObject.FindProperty("Image"));
                EditorGUILayout.PropertyField(imageObject.FindProperty("ZIndex"));
                EditorGUILayout.PropertyField(imageObject.FindProperty("Position"));
                EditorGUILayout.PropertyField(imageObject.FindProperty("Animation"));

                if (GUILayout.Button("Remove Image"))
                {
                    panel.Images.RemoveAt(j);
                    imageObject.ApplyModifiedProperties();
                    break;
                }

                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();

                imageObject.ApplyModifiedProperties();
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add Image"))
            {
                panel.Images.Add(ScriptableObject.CreateInstance<CutSceneImage>());
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            GUILayout.Space(15);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            if (GUILayout.Button("Remove Panel"))
            {
                myTarget.Panels.RemoveAt(i);
                obj.ApplyModifiedProperties();
                break;
            }

            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            obj.ApplyModifiedProperties();
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add Panel"))
        {
            var panel = ScriptableObject.CreateInstance<CutScenePanel>();
            panel.HasDialogue = true;
            myTarget.Panels.Add(panel);
        }
        EditorGUILayout.EndHorizontal();

        // Applying all of the changed properties that took place during the function.
        serializedObject.ApplyModifiedProperties();

    }
}
