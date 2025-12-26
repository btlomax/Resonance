using UnityEngine;

public class BeamEmitter : MonoBehaviour
{
    [Header("Beam Settings")]
    [SerializeField] private Transform _beamOrigin;
    private Color _beamColor = Color.white;
    [SerializeField] private BeamRenderer _beamRenderer;
    [SerializeField] float _beamDuration = 5f;

    public string resonantNote;
    public NotePlayedEventChannel onNotePlayed;
    public ScaleDegreeToColour scaleDegreeToColour;
    public MusicalScale musicalScale;

    public void Emit(string incomingNote)
    {
        if(_beamOrigin == null)
        {
            Debug.LogError("Beam Origin is not assigned.");
            return;
        }

        if(incomingNote != resonantNote || string.IsNullOrEmpty(incomingNote))
        {
            Debug.Log("Incoming note " + incomingNote + " does not match resonant note " + resonantNote + ". Beam not emitted.");
            return;
        }

        for (int i = 0; i < musicalScale.NotesInScale.Length; i++)
        {
            if (musicalScale.NotesInScale[i].noteTitle == incomingNote)
            {
                _beamColor = scaleDegreeToColour.scaleDegreeColours[i];
            }
        }

        Ray ray = new Ray(_beamOrigin.position, _beamOrigin.forward);
        Debug.DrawRay(_beamOrigin.position, _beamOrigin.forward * 10f, _beamColor, _beamDuration);
        Beam emittedBeam = new Beam(ray, _beamColor);

        Debug.Log("Emitting beam from " + _beamOrigin.position + " in direction " + _beamOrigin.forward);

        BeamManager.Instance.ProcessBeam(emittedBeam);

        // Beam colour not changing here
        _beamRenderer.RenderBeam(emittedBeam, _beamDuration);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player entered BeamEmitter trigger. Subscribing to note played events.");
            onNotePlayed.OnNotePlayed += Emit;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log("Player exited BeamEmitter trigger. Unsubscribing from note played events.");
        onNotePlayed.OnNotePlayed -= Emit;
    }
}
