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

    public NoteComparisonStarted noteComparisonStartedEvent;
    public string resonatorNote;

    private void Awake()
    {
        if (_recorder == null)
        {
            _recorder = GetComponent<Recorder>();
        }

        noteComparisonStartedEvent.OnNoteComparisonStarted += OnNoteComparisonStarted;

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
            OnPuzzleActivated();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            // Stop resonator sound and stop recording player input
            Debug.Log("Player exited Resonator Puzzle area: Stopping resonator sound and recording.");
            _activated = false;
            
            _recorder.ToggleRecordingState();
            AudioManager.Instance.Environment_StopLoopingNote();
        }
    }
    private void OnNoteComparisonStarted(List<string> notesRecorded)
    {
        StartCoroutine(PuzzleActivateDelay(3));

        if (notesRecorded.Count == 0)
            return;

        // Need to figure out how to compare two notes and work out the interval between them

        Debug.Log("Note sequence matched! Puzzle solved.");

       // MarkSolved();
    }

    private IEnumerator PuzzleActivateDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        _activated = false;
    }
}
