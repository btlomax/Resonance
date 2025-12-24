using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicalScale", menuName = "Scriptable Objects/MusicalScale")]
public class MusicalScale : ScriptableObject
{
    public string ScaleName;
    public NoteScriptObj[] NotesInScale;

    public NoteScriptObj GetRelativeMinorRoot()
    {
        return NotesInScale[5];
    }

    public NoteScriptObj[] GetRelativeMinorScale()
    {
        NoteScriptObj[] minorScale = new NoteScriptObj[NotesInScale.Length];

        int startIndex = 5; // Relative minor starts at the 6th note of the major scale

        for (int i = 0; i < NotesInScale.Length; i++)
        {
            minorScale[i] = NotesInScale[(startIndex + i) % NotesInScale.Length];
        }

        return minorScale;
    }

    public string GetNameOfMinorScale()
    {
        string note = NotesInScale[5].noteTitle;

        string notePrefix = note.Substring(0, note.Length - 1); // Remove the octave number

        return $"{notePrefix}m";
    }
}
