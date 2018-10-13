using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CutScenePanel : ScriptableObject
{
    public Image Background;

    public bool HasDialogue;

    public string CharacterName;

    public Image CharacterPortrait;

    public string Dialogue;

    public float WaitDuration;

    public AudioSource SoundEffect;

    public AudioSource Music;

    public List<CutSceneImage> Images = new List<CutSceneImage>();


}
