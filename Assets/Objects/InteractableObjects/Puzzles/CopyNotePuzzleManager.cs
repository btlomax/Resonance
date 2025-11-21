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
    public float delayBetweenNotes = 0.5f;

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
        OnPuzzleActivated();
    }

    /// <summary>
    /// Executes the logic to activate the puzzle, initiating the arpeggio sequence.
    /// </summary>
    /// <remarks>This method starts a coroutine to play an arpeggio sequence with a specified delay between
    /// notes. Ensure that the puzzle is in a valid state to be activated before calling this method.</remarks>
    public override void OnPuzzleActivated()
    {
        StartCoroutine(ArpeggioRoutine(delayBetweenNotes));
    }

    /// <summary>
    /// Plays a sequence of notes on the singing stones with a specified delay between each note.
    /// </summary>
    /// <remarks>Each stone in the sequence plays its note, waits for the specified delay, and then stops the
    /// note before moving to the next stone.</remarks>
    /// <param name="delay">The time, in seconds, to wait between playing each note.</param>
    /// <returns>An enumerator that performs the arpeggio routine when iterated.</returns>
    private IEnumerator ArpeggioRoutine(float delay)
    {
        foreach (var stone in singingStones)
        {
            stone.PlayNote();
            yield return new WaitForSeconds(delay);
            stone.StopNote();
        }
    }
}
