using UnityEngine;

public class ResonatorPuzzle : BasePuzzleManager
{
    [SerializeField]
    private bool _activated = false;
    [SerializeField]
    private Recorder _recorder;

    public NoteComparisonStarted noteComparisonStartedEvent;

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

            _recorder.Interact(gameObject);

            // Start resonator sound and start recording player input - call WWise here
        }
    }

    private void Awake()
    {
        if(_recorder == null)
        {
            _recorder = GetComponent<Recorder>();
        }

        Debug.Assert(_recorder != null, "ResonatorPuzzle requires a Recorder component.");
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
            _recorder.Interact(gameObject);

            // Stop Wwise here
        }
    }
}
