using UnityEngine;

public class BeamEmitter : MonoBehaviour
{
    [Header("Beam Settings")]
    [SerializeField] private Transform _beamOrigin;
    [SerializeField] private Color _beamColor;
    [SerializeField] private BeamRenderer _beamRenderer;
    [SerializeField] private float _beamDuration = 5f;
    [SerializeField] private GameObject _emitterGem;
    [SerializeField] private Light _emitterLight;
    [SerializeField] private bool _transmitNoteColour;

    public string resonantNote;
    public NotePlayedEventChannel onNotePlayed;
    public ScaleDegreeToColour scaleDegreeToColour;
    public MusicalScale musicalScale;

    private void Awake()
    {
        _emitterLight = GetComponentInChildren<Light>();
    }

    public void Emit(string incomingNote)
    {
        if(_beamOrigin == null)
            return;

        if(incomingNote != resonantNote || string.IsNullOrEmpty(incomingNote))
        {
            Debug.Log("Incoming note " + incomingNote + " does not match resonant note " + resonantNote + ". Beam not emitted.");
            return;
        }

        if(_transmitNoteColour)
        {
            for (int i = 0; i < musicalScale.NotesInScale.Length; i++)
            {
                if (musicalScale.NotesInScale[i].noteTitle == incomingNote)
                {
                    _beamColor = scaleDegreeToColour.scaleDegreeColours[i];
                }
            }
        }
        else
            _beamColor = Color.white;

        Ray ray = new Ray(_beamOrigin.position, _beamOrigin.forward);
        Debug.DrawRay(_beamOrigin.position, _beamOrigin.forward * 10f, _beamColor, _beamDuration);
        Beam emittedBeam = new Beam(ray, _beamColor);

        var renderer = _emitterGem.GetComponentInChildren<Renderer>();
        renderer.material.color = _beamColor;

        _emitterLight.color = _beamColor;

        BeamManager.Instance.ProcessBeam(emittedBeam);

        // Beam colour not changing here
        _beamRenderer.RenderBeam(emittedBeam, _beamDuration);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            onNotePlayed.OnNotePlayed += Emit;
            _emitterGem.GetComponentInChildren<Renderer>().material.color = Color.white;
            _emitterLight.enabled = true;
            _emitterLight.color = Color.white;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        onNotePlayed.OnNotePlayed -= Emit;
        _emitterGem.GetComponentInChildren<Renderer>().material.color = Color.gray;
        _emitterLight.enabled = false;
    }
}
