using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages the logic and state for a resonator-based musical puzzle, requiring players to play specific notes or
/// intervals to solve it.
/// </summary>
/// <remarks>The <see cref="ResonatorPuzzle"/> class coordinates user interaction, note comparison, and puzzle
/// state transitions for a musical puzzle scenario. It integrates with audio playback, recording, and UI prompt systems
/// to provide feedback and progression. The puzzle is solved when the player correctly matches the target note or
/// interval as defined by the puzzle configuration. <para> This class is typically attached to a puzzle GameObject in
/// the scene and requires a <see cref="Recorder"/> component to function. It also interacts with other components such
/// as <see cref="TriggerInteractable"/>, <see cref="Light"/>, and UI managers. </para></remarks>
public class ResonatorPuzzle : BasePuzzleManager
{
    [SerializeField]
    private bool _activated = false;
    [SerializeField]
    private Recorder _recorder;
    [SerializeField]
    private bool _isSolved = false;
    [SerializeField]
    private bool _correctNotePlayed = false;
    [SerializeField]
    private GameObject activeUIPrompt;

    [Header("Resonator Puzzle Settings")]
    public NoteComparisonStarted noteComparisonStartedEvent;
    public string resonatorNote;
    public MusicalScale puzzleScale;
    public string targetNote;

    //Make this a dropdown list
    public string targetInterval;
    public TriggerInteractable triggerable;
    public GameObject resonatorGem;
    public ScaleDegreeToColour scaleDegreeToColour;
    public Light resonatorLight;

    private void Awake()
    {
        if (_recorder == null)
        {
            _recorder = GetComponent<Recorder>();
        }

        targetNote = NoteManager.NoteLookUp(targetInterval, puzzleScale.NotesInScale);

        for (int i = 0; i < puzzleScale.NotesInScale.Length; i++)
        {
            if (puzzleScale.NotesInScale[i].noteTitle == targetNote)
            {
                Renderer renderer = resonatorGem.GetComponent<Renderer>();
                renderer.material.SetColor("_BaseColour", scaleDegreeToColour.scaleDegreeColours[i]);
                resonatorLight.color = scaleDegreeToColour.scaleDegreeColours[i];
            }
        }
        
        Debug.Assert(_recorder != null, "ResonatorPuzzle requires a Recorder component.");
    }

    public override void Interact(GameObject interactor)
    {
        throw new System.NotImplementedException();
    }

    public override void OnPuzzleActivated()
    {
        if (!_activated && !_isSolved)
        {
            _activated = true;
            Debug.Log("Resonator Puzzle Activated: Starting resonator sound and recording player input.");

            AudioManager.Instance.Environment_PlayLoopingNote(resonatorNote);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !_isSolved)
        {
            if(activeUIPrompt == null)
            {
                activeUIPrompt = WorldUIManager.Instance.CreateResonatorUI(UIPromptLocation, puzzleScale.ScaleName, resonatorLight.color);
            }

            noteComparisonStartedEvent.OnNoteComparisonStarted += OnNoteComparisonStarted;
            _recorder.ToggleContinousRecording(); // Start recording player input
            OnPuzzleActivated();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") && !_isSolved)
        {
            if(activeUIPrompt != null)
            {
                WorldUIManager.Instance.HidePrompt(activeUIPrompt);
                activeUIPrompt = null;
            }

            noteComparisonStartedEvent.OnNoteComparisonStarted -= OnNoteComparisonStarted;

            // Stop resonator sound and stop recording player input
            Debug.Log("Player exited Resonator Puzzle area: Stopping resonator sound and recording.");
            _activated = false;
            
            _recorder.ToggleContinousRecording(); // Stop recording if active
            AudioManager.Instance.Environment_StopLoopingNote();
        }
    }

    /// <summary>
    /// This is the method that receives the recorded notes from the Recorder
    /// </summary>
    /// <param name="notesRecorded"></param>
    private void OnNoteComparisonStarted(List<string> notesRecorded)
    {
        if(_isSolved)
            return;

        Debug.Log($"Entering OnNoteComparisonStarted in ResonatorPuzzle... {notesRecorded.Count} notes recorded.");

        if (notesRecorded.Count > 0 && notesRecorded.Count == 1)
            StartCoroutine(StartLookup(notesRecorded[0]));
    }

    private IEnumerator StartLookup(string noteRecorded)
    {
        _recorder.ToggleContinousRecording(); // Stop recording to prevent interference during comparison

        Debug.Log("Comparing recorded notes to target interval...");

        StartCoroutine(DelayAndLookup(3.0f, noteRecorded));
       
       yield return null;
    }

    private IEnumerator WaitAndRestart(float waitTime)
    {
        AudioManager.Instance.Environment_StopLoopingNote();
        yield return new WaitForSeconds(waitTime);
        if (_activated && !_isSolved)
        {
            Debug.Log("Restarting resonator sound and recording for another attempt.");
            AudioManager.Instance.Environment_PlayLoopingNote(resonatorNote);
            _recorder.ToggleContinousRecording(); // Restart recording
        }
    }

    private IEnumerator DelayAndLookup(float delay, string note)
    {
        yield return new WaitForSeconds(delay);
        AudioManager.Instance.Environment_StopLoopingNote();

        _correctNotePlayed = NoteManager.NoteComparison(targetNote, note);

        if (_correctNotePlayed)
        {
            Debug.Log("Note sequence matched! Puzzle solved.");
            MarkSolved();
        }
        else
        {
            Debug.Log("Note sequence did not match. Try again.");

            yield return WaitAndRestart(5.0f); // Wait for 5 seconds before allowing another attempt

        }
    }

    public override void MarkSolved()
    {
        Debug.Log("Resonator Puzzle marked as solved.");
        _isSolved = true;
        _recorder.puzzleSolved = true;

        resonatorLight.enabled = false;

        if (activeUIPrompt != null)
        {
            WorldUIManager.Instance.HidePrompt(activeUIPrompt);
            activeUIPrompt = null;
        }

        triggerable.TriggerAction("ActivateMechanism");
    }
}
