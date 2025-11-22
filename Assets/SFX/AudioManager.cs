using Assets.SFX;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// I need several "channels" to play different notes simultaneously. Each note played should be independent, allowing for overlapping sounds without cutting each other off.
/// In WWise, I have several events set up to play tones, each with its own RTPC for frequency control. These are my channels.
/// When an object needs to play a tone, it just calls the AudioManager and gives it the name of the event to post ie "Kick" "Bass" "Lead" etc.
/// </summary>
public class AudioManager : MonoBehaviour, IPlayNote
{
    public static AudioManager Instance { get; private set; }

    private bool _noteFinished = false;

    [Header("Wwise Events")]
    public AK.Wwise.Event playToneEvent;
    public AK.Wwise.Event playSequenceEvent;

    //public AK.Wwise.Switch noteSwitch;

    [SerializeField]
    private AK.Wwise.RTPC noteFrequencyRTPC = null;

    /// <summary>
    /// The Copy Puzzle Sequence Player responsible for playing sequences of notes for copy puzzles.
    /// </summary>
    [SerializeField]
    private CopyPuzzleSequencePlayer _cPSP;

    private uint _currentNote = 0;

    public AudioManager()
    {
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log("AudioManager initialized.");
    }

    public void PlayLoopingNote(string note)
    {
        if(_currentNote == 0)
        {
            AkUnitySoundEngine.SetSwitch("Ply_A3_A4", note, gameObject);
            _currentNote = AkUnitySoundEngine.PostEvent(playToneEvent.Id, gameObject);
        }
    }

    public void StopLoopingNote()
    {
        AkUnitySoundEngine.StopPlayingID(_currentNote);
        _currentNote = 0;
    }

    public void PlayCopyPuzzleSequence(List<string> noteSequence)
    {
        Debug.Log("AudioManager: Starting copy puzzle sequence.");
        _cPSP.StartSequenceCoroutine(noteSequence);
    }
}
