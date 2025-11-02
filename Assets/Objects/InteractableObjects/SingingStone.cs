using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SingingStone : Interactable
{
    [Header("Singing Stone Settings")]
    public NoteScriptObj note;
    public BasePuzzleManager PuzzleManager;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;

        audioSource.clip = ToneGenerator.CreateSineWave(note.noteFrequency, note.noteLength);
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
        if (!audioSource.isPlaying)
        {
            OnFocusEnter();
            audioSource.Play();
            OnFocusExit();
        }
    }
}
