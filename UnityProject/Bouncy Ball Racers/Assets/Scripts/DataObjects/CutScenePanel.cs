using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// Class used to represent all of the data needed on one screen of the cutscene, formed via a visual novel style.
public class CutScenePanel : ScriptableObject
{
    // The background image which the characters and objects are placed on.
    public Image Background;

    // Indicates whether the panel has dialogue or rather is intended as a pause, when true the Dialogue property will
    // be displayed on the screen, otherwise the screen will display for the time specified in the WaitDuration 
    // property.
    public bool HasDialogue;

    // The name of the character that is talking during the panel, it will be displayed in the corner of the dialogue 
    // box next to the characters portrait.
    public string CharacterName;

    // An image representing the portrait of the character, image is supposed to be small and will be displayed next to
    // the dialogue box.
    public Image CharacterPortrait;

    // The dialogue text that will display for the currently talking character.
    public string Dialogue;

    // The time in seconds that the panel will wait until it automatically moves onto the next panel.
    public float WaitDuration;

    // An optional sound effect to be played during the start of the cutscene, will only be played once for its whole
    // duration.
    public AudioSource SoundEffect;

    // Music that will be played in the background of the cutscene, will continue looping until the scene ends, or
    // another track is specified to play during another panel.
    public AudioSource Music;

    // The images to be displayed ontop of the background during the panel. (includes things such as characters, and 
    // objects)
    public List<CutSceneImage> Images = new List<CutSceneImage>();


}
