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

    [Header("Note Events")]
    public AK.Wwise.Event Player_PlayToneEvent;
    public AK.Wwise.Event Environment_PlayToneEvent;
    public AK.Wwise.Event playSequenceEvent;

    [Header("SFX Events")]
    public AK.Wwise.Event CollectItemEvent;
    public AK.Wwise.Event MechanismActivateEvent;
    public AK.Wwise.Event AppleDetachEvent;
    public AK.Wwise.Event BarrierDeactivate;
    public AK.Wwise.Event CollectFlute;
    public AK.Wwise.Event InteractEvent;
    public AK.Wwise.Event Footstep;
    public AK.Wwise.Event ToastPop;

    //public AK.Wwise.Switch noteSwitch;

    [SerializeField]
    private AK.Wwise.RTPC noteFrequencyRTPC = null;

    /// <summary>
    /// The Copy Puzzle Sequence Player responsible for playing sequences of notes for copy puzzles.
    /// </summary>
    [SerializeField]
    private CopyPuzzleSequencePlayer _cPSP;

    private uint _playerCurrentNote = 0;
    private uint _environmentCurrentNote = 0;

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

    public void Player_PlayLoopingNote(string note)
    {
        if(_playerCurrentNote == 0)
        {
            AkUnitySoundEngine.SetSwitch("Ply_A3_A4", note, gameObject);
            _playerCurrentNote = AkUnitySoundEngine.PostEvent(Player_PlayToneEvent.Id, gameObject);
        }
    }

    public void Player_StopLoopingNote()
    {
        AkUnitySoundEngine.StopPlayingID(_playerCurrentNote);
        _playerCurrentNote = 0;
    }

    public void Environment_PlayLoopingNote(string note)
    {
        if (_environmentCurrentNote == 0)
        {
            AkUnitySoundEngine.SetSwitch("Env_Looping_A3_A4", note, gameObject);
            _environmentCurrentNote = AkUnitySoundEngine.PostEvent(Environment_PlayToneEvent.Id, gameObject);
        }
    }

    public void Environment_StopLoopingNote()
    {
        AkUnitySoundEngine.StopPlayingID(_environmentCurrentNote);
        _environmentCurrentNote = 0;
        Debug.Log("AudioManager: Stopped environment looping note.");
    }

    public void PlayCopyPuzzleSequence(List<string> noteSequence)
    {
        Debug.Log("AudioManager: Starting copy puzzle sequence.");
        _cPSP.StartSequenceCoroutine(noteSequence);
    }

    

    public void PlayOneShot(AK.Wwise.Event sfxEvent, GameObject source = null)
    {
        sfxEvent?.Post(source != null ? source : gameObject);
    }
}
