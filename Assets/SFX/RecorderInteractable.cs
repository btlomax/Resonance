using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecorderInteractable : Interactable
{
    private Recorder _recorder;

    public TMP_Text recordingText;

    [Header("For future use")]
    public Image recordingLightOn;
    public Image recordingLightOff;

    public GameObject recordingLight;
    public Color recordingLightEmissionColour;
    public float intensity;
    private Renderer _recordingLightRenderer;
    private Material _recordingLightMaterial;
    
    private void Awake()
    {
        _recorder = GetComponent<Recorder>();
        _recordingLightRenderer = recordingLight.GetComponent<Renderer>();

        if( _recordingLightRenderer != null )
            _recordingLightMaterial = _recordingLightRenderer.material;
    }

    public override void Interact(GameObject interactor)
    {
        _recorder.ToggleSimpleRecordingState();

        if(_recorder.isRecording)
        {
            recordingText.SetText("Recording!");
            _recordingLightMaterial.EnableKeyword("_EMISSION");
            _recordingLightMaterial.SetColor("_EmissionColor", recordingLightEmissionColour * intensity);

            return;
        }

        recordingText.SetText("Not recording...");
        _recordingLightMaterial.DisableKeyword("_EMISSION");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            recordingText.enabled = true;
            recordingText.SetText("Not recording...");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        recordingText.enabled = false;
    }
}
