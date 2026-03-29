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
                foreach(var obj in numberOfCollectables)
                {
                    Renderer renderer = obj.GetComponent<Renderer>();
                    renderer.material.color = scaleDegreeToColour.scaleDegreeColours[i];
                    renderer.material.SetColor("_EmissionColor", scaleDegreeToColour.scaleDegreeColours[i] * 0.5f); // Adjust emission intensity as needed
                }
            }
        }
    }

    public abstract void OnTriggerEnter(Collider other);

    public abstract void OnTriggerExit(Collider other);

    public abstract void OnNotePlayed(string noteName);

    public abstract void TriggerCollection();
    public abstract void Collect(GameObject gameObject);
}
