using UnityEngine;
using UnityEngine.UI;
using System.Collections;


// Class responsible for holding data regarding an image in a panel.
public class CutSceneImage : ScriptableObject
{
    // The sprite image that is added to the panel
    public Image Image;

    // The layer index for the respective image (the higher the ZIndex, the further up front the image will be 
    // displayed).
    public int ZIndex;

    // The position in the scene that the image will take (should change this to Rect).
    public Vector2 Position;

    // The animation chosen for this image that will take effect at the start of the panel displaying.
    public CutSceneAnimation Animation;

}