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
    
    private void Awake()
    {
        _recorder = GetComponent<Recorder>();
    }

    public override void Interact(GameObject interactor)
    {
        _recorder.ToggleSimpleRecordingState();

        if(_recorder.isRecording)
        {
            recordingText.SetText("Recording!");
            return;
        }

        recordingText.SetText("Not recording...");
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
