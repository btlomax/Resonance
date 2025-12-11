using UnityEngine;

[CreateAssetMenu(fileName = "MusicalScale", menuName = "Scriptable Objects/MusicalScale")]
public class MusicalScale : ScriptableObject
{
    public string ScaleName;
    public string[] NotesInScale;
}
