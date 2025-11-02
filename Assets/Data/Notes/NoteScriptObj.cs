using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/NoteScriptObj")]
public class NoteScriptObj : ScriptableObject
{
    public string noteTitle;
    public float noteFrequency;
    public float noteLength;
}
