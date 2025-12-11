using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResonatorPuzzle : BasePuzzleManager
{
    [SerializeField]
    private bool _activated = false;
    [SerializeField]
    private Recorder _recorder;
    [SerializeField]
    private bool _isSolved = false;

    public NoteComparisonStarted noteComparisonStartedEvent;
    public string resonatorNote;
    public MusicalScale puzzleScale;
    public string targetInterval;

    private void Awake()
    {
        if (_recorder == null)
        {
            _recorder = GetComponent<Recorder>();
        }

        Debug.Assert(_recorder != null, "ResonatorPuzzle requires a Recorder component.");
    }

    public override void Interact(GameObject interactor)
    {
        throw new System.NotImplementedException();
    }

    public override void MarkSolved()
    {
        throw new System.NotImplementedException();
    }

    public override void OnPuzzleActivated()
    {
        if (!_activated && !IsSolved)
        {
            _activated = true;
            Debug.Log("Resonator Puzzle Activated: Starting resonator sound and recording player input.");

            _recorder.ToggleRecordingState();

            AudioManager.Instance.Environment_PlayLoopingNote(resonatorNote);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            noteComparisonStartedEvent.OnNoteComparisonStarted += OnNoteComparisonStarted;
            OnPuzzleActivated();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            noteComparisonStartedEvent.OnNoteComparisonStarted -= OnNoteComparisonStarted;

            // Stop resonator sound and stop recording player input
            Debug.Log("Player exited Resonator Puzzle area: Stopping resonator sound and recording.");
            _activated = false;
            
            _recorder.ToggleRecordingState();
            AudioManager.Instance.Environment_StopLoopingNote();
        }
    }
    private void OnNoteComparisonStarted(List<string> notesRecorded)
    {
        Debug.Log($"Entering OnNoteComparisonStarted in ResonatorPuzzle... {notesRecorded.Count} notes recorded.");

       // if (notesRecorded.Count > 0)
            StartCoroutine(StartLookup(notesRecorded));
    }

    private IEnumerator StartLookup(List<string> notesRecorded)
    {
        Debug.Log("Comparing recorded notes to target interval...");

        _isSolved = NoteLookup.NoteLookUp(notesRecorded[0], targetInterval, puzzleScale.NotesInScale);

        if (_isSolved)
        {
            Debug.Log("Note sequence matched! Puzzle solved.");
            yield return true;
        }
    }
}
