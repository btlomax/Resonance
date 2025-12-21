using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NotePlayedEventChannel", menuName = "Scriptable Objects/NotePlayedEventChannel")]
public class NotePlayedEventChannel : ScriptableObject
{
    public Action<string> OnNotePlayed;

    public void RaiseEvent(string noteName)
    {
        OnNotePlayed?.Invoke(noteName);
    }
}
