using UnityEngine;

public abstract class BasePuzzleManager : Interactable
{
   public bool IsSolved { get; protected set; }

    public abstract void OnPuzzleActivated();

    protected void MarkSolved()
    {
        IsSolved = true;
        Debug.Log($"Puzzle solved! Is solved: {IsSolved}");

        // Trigger logic for when the puzzle is solved

        // PuzzleSolvedEvent?.Invoke(this, EventArgs.Empty);
    }
}
