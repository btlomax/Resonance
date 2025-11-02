using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/CopyNoteData")]
public class CopyNoteData : ScriptableObject
{
    public GameObject[] singingStones;

    public bool ReversePlayback = false;
}
