using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SingingStone : Interactable
{
    [Header("Singing Stone Settings")]
    public NoteScriptObj note;
    public BasePuzzleManager PuzzleManager;
    public float noteLength = 0.4f;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();

        _audioSource.playOnAwake = false;

        _audioSource.clip = ToneGenerator.CreateSineWave(note.noteFrequency, noteLength);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact(GameObject interactor)
    {
        Debug.Log($"The Singing Stone hums a melodious tune as {interactor.name} interacts with it.");

        PlayNote();
    }

    public void PlayNote()
    {
        if (!_audioSource.isPlaying)
        {
            OnFocusEnter();
            _audioSource.Play();
            OnFocusExit();
        }
    }
}
