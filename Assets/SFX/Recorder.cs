using System.Collections.Generic;
using UnityEngine;

public class Recorder : Interactable
{
    public NotePlayedEventChannel notePlayedEvent;

    [SerializeField]
    private List<string> _recordedNotes = new List<string>();

    private bool _isRecording = false;

    public List<string> GetRecordedNotes() => new List<string>(_recordedNotes);
    public bool IsRecording { get => _isRecording; }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    public void StartRecording()
    {
        notePlayedEvent.OnNotePlayed += OnNotePlayed;
        _recordedNotes.Clear();

        _isRecording = true;
        Debug.Log($"Started Recording Notes: {_isRecording}");
    }

    public void StopRecording()
    {
        notePlayedEvent.OnNotePlayed -= OnNotePlayed;

        Debug.Log("Stopped Recording Notes");
        _isRecording = false;

        // Fire off comparison event or logic here
    }

    private void OnNotePlayed(string noteName)
    {
        if(!_isRecording)
            return;

        _recordedNotes.Add(noteName);
        Debug.Log($"Recorded note: {noteName}");
    }

    public override void Interact(GameObject interactor)
    {
        if(!_isRecording)
            StartRecording();
        else
            StopRecording();
    }
}
