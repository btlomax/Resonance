using UnityEngine;

public class RecorderInteractable : Interactable
{
    private Recorder _recorder;
    
    private void Awake()
    {
        _recorder = GetComponent<Recorder>();
    }

    public override void Interact(GameObject interactor)
    {
        _recorder.ToggleRecordingState();
    }
}
