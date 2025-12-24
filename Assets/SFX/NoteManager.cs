using System.Collections;
using UnityEngine;

public static class NoteManager
{
    public static string _targetNote;
    public static string NoteLookUp(string targetInterval, NoteScriptObj[] scale)
    {
        switch (targetInterval)
        {
            case "Octave":
            case "Root":
                _targetNote = scale[0].noteTitle;
                break;
            case "Second":
                _targetNote = scale[1].noteTitle;
                break;
            case "Minor Third":
                _targetNote = scale[2].noteTitle;
                break;  
            case "Major Third":
                _targetNote = scale[2].noteTitle;
                break;
            case "Fourth":
                _targetNote = scale[3].noteTitle;
                break;
            case "Fifth":
                _targetNote = scale[4].noteTitle;
                break;
            case "Sixth":
                _targetNote = scale[5].noteTitle;
                break;
            case "Minor Seventh":
                _targetNote = scale[6].noteTitle;
                break;
            case "Major Seventh":
                _targetNote = scale[6].noteTitle;
                break;
            default:
                Debug.LogError("Invalid target interval: " + targetInterval);
                return string.Empty;
        }

        return _targetNote;
    }

    public static bool NoteComparison(string playerNote, string targetNote)
    {
        if(string.IsNullOrEmpty(playerNote) || string.IsNullOrEmpty(targetNote))
            return false;

        Debug.Log($"Comparing player note: {playerNote} with target note: {targetNote}");

        if(playerNote == targetNote)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
