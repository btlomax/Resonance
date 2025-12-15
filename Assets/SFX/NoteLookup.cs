using System.Collections;
using UnityEngine;

public static class NoteLookup
{
    private static string _targetNote;
    public static bool NoteLookUp(string playerNote, string targetInterval, string[] scale)
    {
        if(string.IsNullOrEmpty(playerNote))
            return false;

        Debug.Log($"Looking up player note: {playerNote} for target interval: {targetInterval} using the {scale} scale");

        switch (targetInterval)
        {
            case "Octave":
                _targetNote = scale[0];
                break;
            case "Second":
                _targetNote = scale[1];
                break;
            case "Minor Third":
                _targetNote = scale[2];
                break;
            case "Major Third":
                _targetNote = scale[2];
                break;
            case "Fourth":
                _targetNote = scale[3];
                break;
            case "Fifth":
                _targetNote = scale[4];
                break;
            case "Sixth":
                _targetNote = scale[5];
                break;
            case "Minor Seventh":
                _targetNote = scale[6];
                break;
            case "Major Seventh":
                _targetNote = scale[6];
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
