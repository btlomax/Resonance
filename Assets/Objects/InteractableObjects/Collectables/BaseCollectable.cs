using Assets.Objects.InteractableObjects.Collectables;
using UnityEngine;

public abstract class BaseCollectable : MonoBehaviour, ICollectable
{
    public GameObject[] numberOfCollectables;
    public NotePlayedEventChannel notePlayedEvent;
    public ScaleDegreeToColour scaleDegreeToColour;
    public string targetNote;
    public MusicalScale collectableScale;
    public GameObject player;
    public float flightSpeed;

    public void Awake()
    {
        for(int i = 0; i < collectableScale.NotesInScale.Length; i++)
        {
            if (collectableScale.NotesInScale[i].noteTitle == targetNote)
            {
                Debug.Log($"Collectable colour is {scaleDegreeToColour.scaleDegreeColours[i]} for note {targetNote}.");
                Renderer renderer = GetComponent<Renderer>();
                renderer.material.color = scaleDegreeToColour.scaleDegreeColours[i];
            }
        }
    }

    public abstract void OnTriggerEnter(Collider other);

    public abstract void OnTriggerExit(Collider other);

    public abstract void OnNotePlayed(string noteName);

    public abstract void TriggerCollection();
    public abstract void Collect();
}
