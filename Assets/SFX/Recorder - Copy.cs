using System.Collections.Generic;
using UnityEngine;

public class RecorderNonInteract : MonoBehaviour
{
    [Header("Recorder Settings")]
    public NotePlayedEventChannel notePlayedEvent;
    public NoteComparisonStarted noteComparisonStartedEvent;

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

        noteComparisonStartedEvent.RaiseEvent(_recordedNotes);
    }

    private void OnNotePlayed(string noteName)
    {
        if(!_isRecording)
            return;

        _recordedNotes.Add(noteName);
        Debug.Log($"Recorded note: {noteName}");
    }
}
