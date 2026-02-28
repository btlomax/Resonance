using Assets.Data.GameManagement;
using Assets.Objects.PuzzleObjects;
using Assets.Objects.PuzzleObjects.Optical;
using UnityEngine;

public class BeamReceiver : MonoBehaviour, IOpticalElement
{
    [Header("Receiver Settings")]
    public TriggerInteractable linkedTrigger;
    public string targetNote;
    public bool requireExactMatch;
    public MusicalScale puzzleScale;
    public ScaleDegreeToColour scaleDegreeToColour;

    [SerializeField] private bool _isReceiving;
    [SerializeField] private bool _wasHitThisFrame;
    [SerializeField] private Color targetColor;
    [SerializeField] private float intensity;

    private Renderer _receiverLightRenderer;
    private Material _receiverLightMaterial;

    void Awake()
    {
        _receiverLightRenderer = GetComponent<Renderer>();
        if (_receiverLightRenderer != null)
            _receiverLightMaterial = _receiverLightRenderer.material;

        for (int i = 0; i < puzzleScale.NotesInScale.Length; i++)
        {
            if (puzzleScale.NotesInScale[i].noteTitle == targetNote)
            {
                _receiverLightMaterial.color = scaleDegreeToColour.scaleDegreeColours[i];
                targetColor = scaleDegreeToColour.scaleDegreeColours[i];
            }
        }
    }

    void Update()
    {
        _wasHitThisFrame = false;
    }

    void LateUpdate()
    {
        if(_isReceiving && !_wasHitThisFrame)
        {
            _isReceiving = false;
        }

        _wasHitThisFrame = false;
    }

    public bool Interact(Ray incomingRay, RaycastHit hit, BeamContext beam, out Ray outgoingRay)
    {
        outgoingRay = default;

        _wasHitThisFrame = true;
        _receiverLightMaterial.EnableKeyword("_EMISSION");
        _receiverLightMaterial.SetColor("_EmissionColor", beam.color * intensity);

        if (!_isReceiving)
        {
            _isReceiving = true;
            ReceiveBeam(beam.color);
        }

        _receiverLightMaterial.DisableKeyword("_EMISSION");
        return false;
    }

    private void ReceiveBeam(Color color)
    {
        Debug.Log("Beam received with color: " + color);
        if(requireExactMatch)
        {
            if (MatchColourWithoutAlpha(color, targetColor))
            {
                Debug.Log("Received correct color beam.");
                linkedTrigger.TriggerAction("ActivateMechanism");
                GameEventDispatcher.Instance.TriggerEvent(GameUI_Event.OpenedDoor);
                return;
            }
            else
                return;
        }

        linkedTrigger.TriggerAction("ActivateMechanism");
        GameEventDispatcher.Instance.TriggerEvent(GameUI_Event.OpenedDoor);
        return;
    }

    private bool MatchColourWithoutAlpha(Color a, Color b)
    {
        bool result = false;

        if (a.r == b.r && a.g == b.g && a.b == b.b)
        {
            result = true;
        }
       
        return result;
    }
}
