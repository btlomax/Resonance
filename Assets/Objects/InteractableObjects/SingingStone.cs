using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SingingStone : Interactable
{
    [Header("Singing Stone Settings")]
    public NoteScriptObj note;
    public float noteLength = 0.4f;

    private AudioSource _audioSource;
    private BasePuzzleManager _puzzleManager;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();

        _puzzleManager = GetComponentInParent<BasePuzzleManager>();

        _audioSource.playOnAwake = false;
    }

    public override void Interact(GameObject interactor)
    {
        Debug.Log($"The Singing Stone hums a melodious tune as {interactor.name} interacts with it.");

        PlayNote();
    }

    public void PlayNote()
    {
        AudioManager.Instance.PlayNote(note.noteFrequency);
    }
}
