using UnityEngine;

public abstract class BasePuzzleManager : Interactable
{
    public bool IsSolved { get; protected set; }

    public abstract void OnPuzzleActivated();

    public abstract void MarkSolved();
}
