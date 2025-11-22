using Assets.Data.Notes.EnvironmentNotes;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnvironmentNote", menuName = "Scriptable Objects/EnvironmentNote")]
public class EnvironmentNoteSection : ScriptableObject
{
    public string SectionName;
    public List<NoteScriptObj> notesToPlay = new List<NoteScriptObj>();
}
