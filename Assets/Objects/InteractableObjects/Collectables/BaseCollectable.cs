using UnityEngine;

public class BaseCollectable : MonoBehaviour
{
    public GameObject[] numberOfCollectables;
    public NotePlayedEventChannel notePlayedEvent;
    public ScaleDegreeToColour scaleDegreeToColour;
    public string targetNote;
    public MusicalScale collectableScale;

    public void Awake()
    {
        for(int i = 0; i < collectableScale.NotesInScale.Length; i++)
        {
            if (collectableScale.NotesInScale[i].noteTitle == targetNote)
            {
                Debug.Log($"Collectable colour is {scaleDegreeToColour.scaleDegreeColours[i]} for note {targetNote}.");
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        notePlayedEvent.OnNotePlayed += OnNotePlayed;
    }
    public void OnCollisioExit(Collision collision)
    {
        notePlayedEvent.OnNotePlayed -= OnNotePlayed;
    }

    public void OnNotePlayed(string noteName)
    {
        Debug.Log($"Collectable received note played event: {noteName}");

        foreach(var note in collectableScale.NotesInScale)
        {
            if(noteName == targetNote)
            {
                Debug.Log($"Collected {gameObject.name}!");
                break;
            }
        }
    }
}
