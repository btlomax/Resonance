using System.Collections;
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
    public bool isContinousRecording = false;
    public bool puzzleSolved = false;

    public List<string> GetRecordedNotes() => new List<string>(_recordedNotes);

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    /// <summary>
    /// When a note is played, add it to the recorded notes list.
    /// </summary>
    private void StartSimpleRecording()
    {
        notePlayedEvent.OnNotePlayed += OnNotePlayed;
        _recordedNotes.Clear();

        isRecording = true;

        Debug.Log($"Started Recording Notes: {isRecording}");
    }

    /// <summary>
    /// When recording is stopped, raise the note comparison event with the recorded notes.
    /// This is used for puzzles to compare the recorded notes against the expected sequence.
    /// </summary>
    private void StopSimpleRecording()
    {
        noteComparisonStartedEvent.RaiseEvent(_recordedNotes);

        notePlayedEvent.OnNotePlayed -= OnNotePlayed;
        Debug.Log("Stopped Recording Notes");
        isRecording = false;
    }

    /// <summary>
    /// Start recording if not recording, stop recording if currently recording.
    /// </summary>
    public void ToggleSimpleRecordingState()
    {
        if(puzzleSolved)
            return;

        if (!isRecording)
            StartSimpleRecording();
        else
            StopSimpleRecording();
    }

    public void ToggleContinousRecording()
    {  
        if(puzzleSolved)
            return;

        if (!isContinousRecording)
            StartContinousRecording();
        else
            StopContinousRecording();
    }   
    private void OnNotePlayed(string noteName)
    {
        if(isRecording || isContinousRecording)
        {
            _recordedNotes.Add(noteName);
            Debug.Log($"Recorded note: {noteName}");
        }
    }
    private void StartContinousRecording()
    {
        isContinousRecording = true;
        _recordedNotes.Clear();
        notePlayedEvent.OnNotePlayed += OnNotePlayed;
       // var _recordingCoroutine = StartCoroutine(ContinousRecord());

        // Need to constantly raise the note comparison event until stopped.
        // Could use a coroutine or a repeating invoke.
    }

    private void StopContinousRecording()
    {
        isContinousRecording = false;
        notePlayedEvent.OnNotePlayed -= OnNotePlayed;
        Debug.Log("Stopped Continous Recording Notes");
    }

    public void Update()
    {
        if(!isContinousRecording || isRecording)
            return;

        if (_recordedNotes.Count == 1 && isContinousRecording)
        {
            StartCoroutine(Delay(3f));

            notePlayedEvent.OnNotePlayed -= OnNotePlayed;
            noteComparisonStartedEvent.RaiseEvent(_recordedNotes);
            Debug.Log("Updated Recording: Raised Note Comparison Event");
        }
    }

    private IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
}
