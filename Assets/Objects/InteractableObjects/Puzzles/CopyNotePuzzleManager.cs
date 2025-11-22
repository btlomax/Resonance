using Assets.Player.Contracts;
using NUnit.Framework;
using System.Collections;
using UnityEngine;

/// <summary>
/// Need to look at how best to make sequences easier. Need to be able to specify delays between notes being played sequentially
/// </summary>

public class CopyNotePuzzleManager : BasePuzzleManager
{
    [Header("Copy Note Puzzle Settings")]
    public SingingStone[] singingStones;
    public string[] noteSequence;

    public float delayBetweenNotes = 0.5f;

    [SerializeField]
    private bool _activated = false;

    private void Awake()
    {
        singingStones = GetComponentsInChildren<SingingStone>();
    }

    /// <summary>
    /// When the player interacts with the puzzle manager, start the arpeggio sequence
    /// </summary>
    /// <param name="interactor"></param>
    public override void Interact(GameObject interactor)
    {
        if(!_activated)
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

        StartCoroutine(ResetActivated(3));
    }

    private IEnumerator ResetActivated(float delay)
    {
        yield return new WaitForSeconds(delay);

        _activated = false;
    }
}
