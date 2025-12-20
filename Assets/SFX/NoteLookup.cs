using System.Collections;
using UnityEngine;

public static class NoteLookup
{
    private static string _targetNote;
    public static bool NoteLookUp(string playerNote, string targetInterval, NoteScriptObj[] scale)
    {
        if(string.IsNullOrEmpty(playerNote))
            return false;

        Debug.Log($"Looking up player note: {playerNote} for target interval: {targetInterval} using the {scale} scale");

        switch (targetInterval)
        {
            case "Octave":
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
                return false;
        }

        if(playerNote == _targetNote)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
