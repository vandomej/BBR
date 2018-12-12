using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using BBRUtilities;

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
        EditorGUILayout.Slider(serializedObject.FindProperty("DialogHorizontalPadding"), 0.0f, 20.0f, "Dialog Horizontal Padding");
        EditorGUILayout.Slider(serializedObject.FindProperty("DialogVerticalArea"), 0.0f, 1.0f, "Dialog Vertical Area");
        EditorGUILayout.Slider(serializedObject.FindProperty("DialogVerticalPadding"), 0.0f, 40.0f, "Dialog Vertical Padding");
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

            //Preview area
            var previewArea = EditorGUILayout.BeginVertical();
            GUILayout.Space(EditorGUIUtility.singleLineHeight * 12);

            GUI.Box(previewArea, string.Empty);
            if (panel.Background != null)
            {
                GUI.DrawTexture(previewArea, panel.Background.texture, ScaleMode.StretchToFill);
            }

            List<CutSceneImage> temp = new List<CutSceneImage>(panel.Images);
            temp.Sort(BBRUtilities.Comparer.Get<CutSceneImage>((i1, i2) => i1.ZIndex - i2.ZIndex));
            for (int j = 0; j < temp.Count; j++)
            {
                var image = temp[j];
                if (image.Image != null)
                {
                    var imageX = previewArea.x + (previewArea.width * image.Position.x);
                    var imageY = previewArea.y + (previewArea.height * image.Position.y);
                    var imageRect = new Rect(
                        imageX,
                        imageY,
                        (image.hScale * previewArea.width),
                        (image.vScale * previewArea.height)
                    );
                    GUI.DrawTexture(imageRect, image.Image.texture, ScaleMode.StretchToFill);
                }
            }

            if (panel.HasDialogue)
            {
                var dialogRect = new Rect(
                    previewArea.x + myTarget.DialogHorizontalPadding,
                    previewArea.y + ((previewArea.height - (previewArea.height * myTarget.DialogVerticalArea)) + myTarget.DialogVerticalPadding),
                    previewArea.width - (2 * myTarget.DialogHorizontalPadding),
                    (previewArea.height * myTarget.DialogVerticalArea) - (2 * myTarget.DialogVerticalPadding)
                );
                GUI.Box(dialogRect, panel.Dialogue);
                if (panel.CharacterPortrait != null)
                {
                    var portraitRect = new Rect(
                        dialogRect.x - 10,
                        dialogRect.y - 10,
                        35,
                        40
                    );
                    GUI.DrawTexture(portraitRect, panel.CharacterPortrait.texture, ScaleMode.StretchToFill);
                }

            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.PropertyField(obj.FindProperty("Background"));
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

                EditorGUILayout.BeginHorizontal();
                image.Position.x = EditorGUILayout.Slider("Horizontal Position", image.Position.x, 0.0f, 1.0f);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Slider(imageObject.FindProperty("hScale"), 0.0f, 1.0f, "Horizontal Scale");

                EditorGUILayout.BeginHorizontal();
                image.Position.y = EditorGUILayout.Slider("Vertical Position", image.Position.y, 0.0f, 1.0f);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Slider(imageObject.FindProperty("vScale"), 0.0f, 1.0f, "Vertical Scale");

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
                var image = ScriptableObject.CreateInstance<CutSceneImage>();
                image.hScale = 1.0f;
                image.vScale = 1.0f;
                panel.Images.Add(image);
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
            if (myTarget.Panels.Count != 0)
            {
                panel.Background = myTarget.Panels[myTarget.Panels.Count - 1].Background;
            }
            myTarget.Panels.Add(panel);
        }
        EditorGUILayout.EndHorizontal();

        // Applying all of the changed properties that took place during the function.
        serializedObject.ApplyModifiedProperties();

    }
}
