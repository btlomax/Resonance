using UnityEngine;

/// <summary>
/// Represents a mechanism that emits a visual beam in response to musical notes or player interaction within the game
/// environment.
/// </summary>
/// <remarks>The <see cref="BeamEmitter"/> can emit beams either automatically when a specific note is played or
/// manually through direct activation. It supports color customization based on musical scale degrees and can interact
/// with player-triggered events. This component is typically used in musical or puzzle-based gameplay scenarios where
/// visual feedback is tied to audio input or player actions.</remarks>
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

    private Renderer _emitterGemRenderer;
    private Material _emitterGemMaterial;

    public bool EmitFromNotePlayed = true;
    public GameObject emitterTeleportPad;

    [Header("Musical Settings")]
    [Tooltip("Only needed if player directly interacts with the emitter")]
    public string resonantNote;
    public NotePlayedEventChannel onNotePlayed;
    public ScaleDegreeToColour scaleDegreeToColour;
    public MusicalScale musicalScale;

    private void Awake()
    {
        _emitterLight = GetComponentInChildren<Light>();
        _emitterGemRenderer = _emitterGem.GetComponentInChildren<Renderer>();

        if(_emitterGemRenderer != null)
            _emitterGemMaterial = _emitterGemRenderer.material;

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

    /// <summary>
    /// Emits a beam from the origin if the specified note matches the resonant note.
    /// </summary>
    /// <remarks>If the incoming note does not match the resonant note, no beam is emitted and the method
    /// returns without effect. The color of the emitted beam is determined by the note and current settings; if note
    /// color transmission is enabled, the beam color corresponds to the note's scale degree, otherwise it is
    /// white.</remarks>
    /// <param name="incomingNote">The note to evaluate for beam emission. The beam is emitted only if this value matches the resonant note.</param>
    public void EmitFromNote(string incomingNote)
    {
        if(_beamOrigin == null)
            return;

        if (incomingNote != resonantNote)
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
        Beam emittedBeam = new Beam(ray, _beamColor);
        _beamRenderer.beamOrigin = _beamOrigin;

        BeamManager.Instance.ProcessBeam(emittedBeam);

        _beamRenderer.RenderBeam(emittedBeam, _beamDuration);
    }

    /// <summary>
    /// Emits a beam from the current origin without requiring any associated note.
    /// </summary>
    /// <remarks>This method draws and processes a beam using the configured origin, color, and duration. No
    /// additional data or note is associated with the emitted beam.</remarks>
    public void Emit_NoNote()
    {
        if (_beamOrigin == null)
            return;

        Ray ray = new Ray(_beamOrigin.position, _beamOrigin.forward);
        Debug.DrawRay(_beamOrigin.position, _beamOrigin.forward * 10f, _beamColor, _beamDuration);
        Beam emittedBeam = new Beam(ray, _beamColor);
        _beamRenderer.beamOrigin = _beamOrigin;
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

            other.GetComponent<PlayerController>().emitterTeleport = emitterTeleportPad.transform;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        onNotePlayed.OnNotePlayed -= EmitFromNote;
        _emitterLight.enabled = false;
    }

    public override void ActivateMechanism()
    {
        var trigger = GetComponent<BoxCollider>().enabled = true;
        Emit_NoNote();
    }
}
