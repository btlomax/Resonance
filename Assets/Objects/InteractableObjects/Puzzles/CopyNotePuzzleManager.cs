using Assets.Player.Contracts;
using NUnit.Framework;
using System.Collections;
using UnityEngine;

public class CopyNotePuzzleManager : BasePuzzleManager
{
    [Header("Copy Note Puzzle Settings")]
    public SingingStone[] singingStones;
    public float delayBetweenNotes = 0.5f;

    private void Awake()
    {
        singingStones = GetComponentsInChildren<SingingStone>();
    }

    public override void Interact(GameObject interactor)
    {
        Debug.Log($"{interactor.name} has activated the Copy Note Puzzle Manager.");

        OnPuzzleActivated();
    }

    public override void OnPuzzleActivated()
    {
        PlayArpeggio();
    }

    private void PlayArpeggio()
    {
        Debug.Log("Playing arpeggio sequence of singing stones.");
        StartCoroutine(ArpeggioRoutine(delayBetweenNotes));
    }

    private IEnumerator ArpeggioRoutine(float delay)
    {
        foreach (var stone in singingStones)
        {
            stone.PlayNote();
            Debug.Log("Note played");
            yield return new WaitForSeconds(delay);
        }
    }
}
