using Assets.Player.Contracts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Need to look at how best to make sequences easier. Need to be able to specify delays between notes being played sequentially
/// </summary>

public class CopyNotePuzzleManager : BasePuzzleManager
{
    [Header("Copy Note Puzzle Settings")]
    public SingingStone[] singingStones;
    public List<string> noteSequence = new List<string>();
    public NoteComparisonStarted noteComparisonStartedEvent;
    public GameObject successObject;

    public float delayBetweenNotes = 0.5f;

    [SerializeField]
    private bool _activated = false;

    [SerializeField]
    private Recorder _recorder;

    private void Awake()
    {
        singingStones = GetComponentsInChildren<SingingStone>();
    }

    private void OnEnable()
    {
        noteComparisonStartedEvent.OnNoteComparisonStarted += OnNoteComparisonStarted;
    }

    /// <summary>
    /// When the player interacts with the puzzle manager, start the arpeggio sequence
    /// </summary>
    /// <param name="interactor"></param>
    public override void Interact(GameObject interactor)
    {
        if(!_activated || !IsSolved)
            OnPuzzleActivated();

        _activated = true;
    }

    /// <summary>
    /// Executes the logic to activate the puzzle, initiating the arpeggio sequence.
    /// </summary>
    /// <remarks>This method starts a coroutine to play an arpeggio sequence with a specified delay between
    /// notes. Ensure that the puzzle is in a valid state to be activated before calling this method.</remarks>
    public override void OnPuzzleActivated()
    {
        Debug.Log("Copy Note Puzzle Activated: Starting arpeggio sequence.");
        AudioManager.Instance.PlayCopyPuzzleSequence(noteSequence);

        StartCoroutine(PuzzleActivateDelay(3));
    }

    private IEnumerator PuzzleActivateDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        _activated = false;
    }

    private IEnumerator BridgeAppearDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        successObject.SetActive(true);
    }

    private void OnNoteComparisonStarted(List<string> notesRecorded)
    {
        if(!_activated || IsSolved)
            return;

        StartCoroutine(PuzzleActivateDelay(3));

        if (notesRecorded.Count == 0)
            return;

        if(notesRecorded.Count != noteSequence.Count)
        {
            Debug.Log("Note sequence length mismatch. Puzzle failed.");
            return;
        }

        for(int i = 0; i < noteSequence.Count; i++)
        {
            if(notesRecorded[i] != noteSequence[i])
            {
                Debug.Log($"Note mismatch at index {i}. Expected: {noteSequence[i]}, Recorded: {notesRecorded[i]}. Puzzle failed.");
                return;
            }
        }

        Debug.Log("Note sequence matched! Puzzle solved.");

        MarkSolved();
    }

    public override void MarkSolved()
    {
        StartCoroutine(BridgeAppearDelay(5));
    }
}
