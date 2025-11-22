using Assets.SFX;
using System.Collections;
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

    [SerializeField]
    private EnvironmentMusicPlayer _environmentMusicPlayer;

    private uint _currentNote = 0;
    private ToneGenerator _toneGenerator;

    public AudioManager()
    {
        _toneGenerator = new ToneGenerator();
        Debug.Log("Tone generator created.");
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
        float frequency = _toneGenerator.ConvertNoteToFrequency(note);
        Debug.Log($"Playing note with frequency: {note} Hz");
       
        if(_currentNote == 0)
        {
            AkUnitySoundEngine.SetRTPCValue("Note_Frequency", frequency);
            _currentNote = AkUnitySoundEngine.PostEvent(playToneEvent.Id, gameObject);
        }
    }

    public void StopLoopingNote()
    {
        playToneEvent.Stop(gameObject);
        _currentNote = 0;
    }

    public void PlayEnvironmentNoteSequence(NoteScriptObj[] section)
    {
        Debug.Log("Entered PlayEnvironmentNoteSequence in AudioManager.");

        PlayNoteSequence(section);
        Debug.Log("Note sequence has been played.");
    }

    public void StartSequenceCoroutine(NoteScriptObj[] notesToPlay)
    {
        StartCoroutine(PlayNoteSequence(notesToPlay));
    }

    public IEnumerator PlayNoteSequence(NoteScriptObj[] notesToPlay)
    {
        Debug.Log("Playing note sequence...");

        foreach (var note in notesToPlay)
        {
            _noteFinished = false;

            AkUnitySoundEngine.SetSwitch("A3_A4", note.noteTitle, gameObject);
            Debug.Log($"Event ID : {playSequenceEvent.Id} - Playing note: {note.noteTitle} (Frequency: {note.noteFrequency} Hz)");
            AkUnitySoundEngine.PostEvent(playSequenceEvent.Id, gameObject, (uint)AkCallbackType.AK_EndOfEvent, NoteFinishedCallback, null);

            yield return new WaitUntil(() => _noteFinished);
        }
    }

    private void NoteFinishedCallback(object in_cookie, AkCallbackType type, AkCallbackInfo info)
    {
        if (type == AkCallbackType.AK_EndOfEvent)
            _noteFinished = true;
    }
}
