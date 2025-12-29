using UnityEngine;

public class BeamEmitter : BaseMechanism
{
    [Header("Beam Settings")]
    [SerializeField] private Transform _beamOrigin;
    [SerializeField] private Color _beamColor;
    [SerializeField] private BeamRenderer _beamRenderer;
    [SerializeField] private float _beamDuration = 5f;
    [SerializeField] private GameObject _emitterGem;
    [SerializeField] private Light _emitterLight;
    [SerializeField] private bool _transmitNoteColour;

    public bool EmitFromNotePlayed = true;

    [Header("Musical Settings")]
    [Tooltip("Only needed if player directly interacts with the emitter")]
    public string resonantNote;
    public NotePlayedEventChannel onNotePlayed;
    public ScaleDegreeToColour scaleDegreeToColour;
    public MusicalScale musicalScale;

    private void Awake()
    {
        _emitterLight = GetComponentInChildren<Light>();

        for (int i = 0; i < musicalScale.NotesInScale.Length; i++)
        {
            if (musicalScale.NotesInScale[i].noteTitle == resonantNote)
            {
                Renderer renderer = _emitterGem.GetComponentInChildren<Renderer>();
                renderer.material.SetColor("_BaseColour", scaleDegreeToColour.scaleDegreeColours[i]);
                _emitterLight.color = scaleDegreeToColour.scaleDegreeColours[i];
            }
        }
    }

    public void EmitFromNote(string incomingNote)
    {
        if(_beamOrigin == null)
            return;

        if(incomingNote != resonantNote)
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

        BeamManager.Instance.ProcessBeam(emittedBeam);

        _beamRenderer.RenderBeam(emittedBeam, _beamDuration);
    }

    public void Emit_NoNote()
    {
        if (_beamOrigin == null)
            return;

        Ray ray = new Ray(_beamOrigin.position, _beamOrigin.forward);
        Debug.DrawRay(_beamOrigin.position, _beamOrigin.forward * 10f, _beamColor, _beamDuration);
        Beam emittedBeam = new Beam(ray, _beamColor);
        BeamManager.Instance.ProcessBeam(emittedBeam);
        _beamRenderer.RenderBeam(emittedBeam, _beamDuration);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && EmitFromNotePlayed)
        {
            onNotePlayed.OnNotePlayed += EmitFromNote;
            _emitterGem.GetComponentInChildren<Renderer>().material.color = Color.white;
            _emitterLight.enabled = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        onNotePlayed.OnNotePlayed -= EmitFromNote;
       // _emitterGem.GetComponentInChildren<Renderer>().material.color = Color.gray;
        _emitterLight.enabled = false;
    }

    public override void ActivateMechanism()
    {
        Emit_NoNote();
    }
}
