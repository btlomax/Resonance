using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    [Header("Recorder Settings")]
    public NotePlayedEventChannel notePlayedEvent;
    public NoteComparisonStarted noteComparisonStartedEvent;

    [SerializeField]
    private List<string> _recordedNotes = new List<string>();

    public bool isRecording = false;

    public List<string> GetRecordedNotes() => new List<string>(_recordedNotes);
    public bool IsRecording { get => isRecording; }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    private void StartRecording()
    {
        notePlayedEvent.OnNotePlayed += OnNotePlayed;
        _recordedNotes.Clear();

        isRecording = true;
        noteComparisonStartedEvent.RaiseEvent(_recordedNotes);

        Debug.Log($"Started Recording Notes: {isRecording}");
    }

    private void StopRecording()
    {
        notePlayedEvent.OnNotePlayed -= OnNotePlayed;
        noteComparisonStartedEvent.RaiseEvent(_recordedNotes);
        Debug.Log("Stopped Recording Notes");
        isRecording = false;
    }

    private void OnNotePlayed(string noteName)
    {
        if(!isRecording)
            return;

        _recordedNotes.Add(noteName);
        Debug.Log($"Recorded note: {noteName}");
    }

    /// <summary>
    /// Start recording if not recording, stop recording if currently recording.
    /// </summary>
    public void ToggleRecordingState()
    {
        if(!isRecording)
            StartRecording();
        else
            StopRecording();
    }
}
