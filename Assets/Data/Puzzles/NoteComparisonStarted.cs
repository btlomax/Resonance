using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NoteComparisonStarted", menuName = "Scriptable Objects/NoteComparisonStarted")]
public class NoteComparisonStarted : ScriptableObject
{
    public Action<List<string>> OnNoteComparisonStarted;

    public void RaiseEvent(List<string> recordedNotes)
    {
        OnNoteComparisonStarted?.Invoke(recordedNotes);
    }
}
